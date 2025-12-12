
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Medication_Reminder_API.Services
{
    public class PatientService : IPatient
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PatientService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<PatientDto>> GetAllPatientsAsync(
        string? doctorId,
        string? caregiverId,
        string? patientId)
        {
            IQueryable<Patient> patients = _context.Patients; 

            if (doctorId != null)
            {
                patients = _context.DoctorPatients
                    .Where(dp => dp.Doctor.UserId == doctorId)
                    .Select(dp => dp.Patient);
            }
            else if (caregiverId != null)
            {
                patients = _context.PatientCaregivers
                    .Where(pc => pc.Caregiver.UserId == caregiverId)
                    .Select(pc => pc.Patient);
            }
            else if (patientId != null)
            {
                patients = _context.Patients
                    .Where(p => p.UserId == patientId);
            }

            return await patients
                .ProjectTo<PatientDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }



        public async Task<List<PatientDto>> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new List<PatientDto>();

            var patients = await _context.Patients
                .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            return _mapper.Map<List<PatientDto>>(patients);
        }

        //علشان ارجع المجموعة كلها اللي مرتبطة بأكثر من واحد
        public async Task<List<PatientDto>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var patients = await _context.Patients
                .Where(p => ids.Contains(p.PatientID))
                .ProjectTo<PatientDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return patients;
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
            if (patient == null)
                return null;

            _mapper.Map(dto, patient);
            await _context.SaveChangesAsync();
            return _mapper.Map<PatientDto>(patient);
        }


        public async Task<PatientDto?> DeletePatientAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return null;

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
    }
}
