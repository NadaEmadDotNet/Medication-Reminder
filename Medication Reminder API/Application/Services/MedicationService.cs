using AutoMapper;
using AutoMapper.QueryableExtensions;
using Medication_Reminder_API.Application.DTOS;
using Medication_Reminder_API.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
namespace Medication_Reminder_API.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDoseLogService _logService;
        private readonly IMemoryCache _cache;


        public MedicationService(ApplicationDbContext context, IMapper mapper, IDoseLogService logService, IMemoryCache memoryCache)
        {
            _context = context;
            _mapper = mapper;
            _logService = logService;
            _cache = memoryCache;
        }


        // wITHOUT CACHING 

        /* public async Task<List<MedicationDTO>> GetAllMedicationsAsync()
          {
              var meds = await _context.Medications
                  .AsNoTracking()
                  .ToListAsync();

              return _mapper.Map<List<MedicationDTO>>(meds);
          }*/


        // WITH CACHING and PAGINATION
        public async Task<PagedResult<MedicationDTO>> GetAllMedicationsAsync(int page = 1, int pageSize = 5)
        {
            const string cacheKey = "all_medications";

            // 1️⃣ جلب كل البيانات من الكاش أو DB
            if (!_cache.TryGetValue(cacheKey, out List<MedicationDTO> cachedMeds))
            {
                var meds = await _context.Medications
                    .AsNoTracking()
                    .ToListAsync();

                cachedMeds = _mapper.Map<List<MedicationDTO>>(meds);

                _cache.Set(cacheKey, cachedMeds, TimeSpan.FromMinutes(10));
            }
            var totalcount= cachedMeds.Count;
            var totalpages= (int)Math.Ceiling(totalcount / (double)pageSize);
            var pagedMeds = cachedMeds
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return new PagedResult<MedicationDTO>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalcount,
                TotalPages = totalpages,
                Data = pagedMeds
            };
        }


        public async Task<List<MedicationDTO>> GetByNameAsync(string name)
        {
            var meds = await _context.Medications
                .Where(m => m.Name.Contains(name))
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<MedicationDTO>>(meds);
        }

        public async Task<List<MedicationDTO>> GetAllMedicationsForPatientAsync(int patientId)
        {
            var meds = await _context.PatientMedications
                .Where(pm => pm.PatientID == patientId)
                .Select(pm => pm.Medication)
                .AsNoTracking()
                .ProjectTo<MedicationDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return _mapper.Map<List<MedicationDTO>>(meds);
        }

        public async Task<MedicationDTO> AddAsync(MedicationDTO dto)
        {
            bool exists = await _context.Medications
                .AnyAsync(m => m.Name.ToLower() == dto.Name.ToLower());

            if (exists)
                throw new InvalidOperationException($"Medication with name '{dto.Name}' already exists.");

            var med = _mapper.Map<Medication>(dto);
            med.Status = MedicationStatus.Ongoing;

            await _context.Medications.AddAsync(med);
            await _context.SaveChangesAsync();
            _cache.Remove("all_medications");


            return _mapper.Map<MedicationDTO>(med);
        }

        public async Task<MedicationDTO?> EditMedicationAsync(int id, MedicationDTO dto)
        {
            var med = await _context.Medications.FindAsync(id);
            if (med == null) return null;

            _mapper.Map(dto, med);
            await _context.SaveChangesAsync();
            _cache.Remove("all_medications");

            return _mapper.Map<MedicationDTO>(med);
        }

        public async Task<MedicationDTO?> DeleteMedicationAsync(int id)
        {
            var med = await _context.Medications.FindAsync(id);
            if (med == null) return null;

            _context.Medications.Remove(med);
            await _context.SaveChangesAsync();
            _cache.Remove("all_medications");

            return _mapper.Map<MedicationDTO>(med);
        }

        public async Task<MedicationDTO?> UpdateStatusAsync(int medicationId)
        {
            var med = await _context.Medications.FindAsync(medicationId);
            if (med == null) return null;

            var totalScheduled = await _context.DoseLogs
                .CountAsync(d => d.MedicationID == med.MedicationID);

            var takenCount = await _context.DoseLogs
                .CountAsync(d => d.MedicationID == med.MedicationID && d.Status == DoseStatus.Taken);

            med.Status = takenCount > totalScheduled
                ? MedicationStatus.Overtaken
                : (takenCount == totalScheduled
                    ? MedicationStatus.Completed
                    : MedicationStatus.Ongoing);

            await _context.SaveChangesAsync();
            _cache.Remove("all_medications");
            return _mapper.Map<MedicationDTO>(med);
        }
    }
}
