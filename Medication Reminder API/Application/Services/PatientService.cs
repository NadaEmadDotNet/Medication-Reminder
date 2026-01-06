using AutoMapper;
using AutoMapper.QueryableExtensions;
using Medication_Reminder_API.Application.DTOS;
using Medication_Reminder_API.Application.Interfaces;
using Medication_Reminder_API.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medication_Reminder_API.Services
{
    public class PatientService : IPatient
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public PatientService(ApplicationDbContext context, IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<PatientDto>> GetAllPatientsAsync(
            string? doctorId, string? caregiverId, string? patientId,
            int page = 1, int pageSize = 10)
        {
            IQueryable<Patient> patients = _context.Patients
                .Where(p => p.IsVisible && p.IsActive)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(doctorId))
            {
                patients = _context.DoctorPatients
                    .Where(dp => dp.Doctor.UserId == doctorId)
                    .Select(dp => dp.Patient)
                        .Where(p => p.IsVisible && p.IsActive).OrderBy(p=>p.Name)
                    .AsNoTracking();
            }
            else if (!string.IsNullOrEmpty(caregiverId))
            {
                patients = _context.PatientCaregivers
                    .Where(pc => pc.Caregiver.UserId == caregiverId)
                    .Select(pc => pc.Patient)
                    .Where(p => p.IsVisible && p.IsActive)
                    .AsNoTracking();
            }
            else if (!string.IsNullOrEmpty(patientId))
            {
                patients = patients.Where(p => p.UserId == patientId);
            }


            var totalCount = await patients.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var data = await patients
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<PatientDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResult<PatientDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Data = data
            };
        }


        public async Task<List<PatientDto>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Patients
                .Where(p => p.IsVisible && p.IsActive && ids.Contains(p.PatientID))
                .AsNoTracking()
                .ProjectTo<PatientDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<PatientDto>> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new List<PatientDto>();

            return await _context.Patients
                .Where(p =>p.IsActive&&p.IsVisible&& EF.Functions.Like(p.Name, $"%{name}%"))
                .OrderBy(p => p.Name)
                .AsNoTracking()
                .ProjectTo<PatientDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<PatientDto> AddAsync(PatientDto dto)
        {
            var patient = _mapper.Map<Patient>(dto);
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto?> EditPatientAsync(int id, PatientDto dto)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return null;

            _mapper.Map(dto, patient);
            await _context.SaveChangesAsync();

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto?> DeletePatientAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return null;

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<ServiceResult> AssignMedicationToPatientAsync(int patientId, int medicationId)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            var med = await _context.Medications.FindAsync(medicationId);

            if (patient == null || med == null)
                return new ServiceResult { Success = false, Message = "Patient or Medication not found." };

            bool alreadyAssigned = await _context.PatientMedications
                .AnyAsync(pm => pm.PatientID == patientId && pm.MedicationID == medicationId);

            if (alreadyAssigned)
                return new ServiceResult { Success = false, Message = "This medication has already been assigned to the patient." };

            var link = new PatientMedication
            {
                PatientID = patientId,
                MedicationID = medicationId
            };

            await _context.PatientMedications.AddAsync(link);
            await _context.SaveChangesAsync();

            return new ServiceResult { Success = true, Message = "Medication assigned successfully." };
        }
        public async Task<PatientDto> ChangePatientStatusAsync(int patientId, UpdatePatientStatusDto dto)
        {
            var patient = await _context.Patients.FindAsync(patientId);
            if (patient == null)
                throw new KeyNotFoundException("Patient not found");

            patient.IsActive = dto.IsActive;
            patient.IsVisible = dto.IsVisible;
            await _context.SaveChangesAsync();
            return _mapper.Map<PatientDto>(patient); 
        }

    }
}
