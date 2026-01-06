using AutoMapper.QueryableExtensions;
using Medication_Reminder_API.Application.DTOS;
using Medication_Reminder_API.Application.Interfaces;
using Medication_Reminder_API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Medication_Reminder_API.Services
{
    public class CaregiverService : ICaregiverService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMedicationService _medService;
        private readonly IDoseLogService _logService;
        private readonly IMapper _mapper;

        public CaregiverService(ApplicationDbContext context, IMedicationService medService, IDoseLogService logService, IMapper mapper)
        {
            _context = context;
            _medService = medService;
            _logService = logService;
            _mapper = mapper;
        }

        public async Task<List<Caregiver>> GetAllCaregiversAsync() =>
     await _context.Caregivers.ToListAsync();

        public async Task<Caregiver?> GetCaregiverByNameAsync(string name) =>
            await _context.Caregivers.FirstOrDefaultAsync(c => c.Name == name);

        public async Task<string> AssignPatientToCaregiverAsync(CaregiverAssignDTO dto)
        {
            if (dto == null)
                throw new ArgumentException("Data is empty.");

            var caregiver = await _context.Caregivers
                .FirstOrDefaultAsync(c => c.UserId == dto.UserId);

            if (caregiver == null)
            {
                caregiver = new Caregiver
                {
                    UserId = dto.UserId,
                    Name = dto.Name,
                    RelationToPatient = dto.RelationToPatient
                };
                await _context.Caregivers.AddAsync(caregiver);
                await _context.SaveChangesAsync();
            }

            var isAlreadyLinked = await _context.PatientCaregivers
                .AnyAsync(pc => pc.CaregiverID == caregiver.CaregiverID
                           && pc.PatientID == dto.PatientID);



            if (isAlreadyLinked)
                return "This caregiver is already assigned to this patient.";


            var patientCaregiver = new PatientCaregiver
            {
                CaregiverID = caregiver.CaregiverID,
                PatientID = dto.PatientID
            };

            await _context.PatientCaregivers.AddAsync(patientCaregiver);
            await _context.SaveChangesAsync(); 

            return "Caregiver linked to patient successfully";
        }

        public async Task<Caregiver?> EditCaregiverAsync(int id, Caregiver caregiver)
        {
            var existing = await _context.Caregivers.FindAsync(id);
            if (existing == null) return null;

            existing.RelationToPatient = caregiver.RelationToPatient;
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<Caregiver?> DeleteCaregiverAsync(int id)
        {
            var caregiver = await _context.Caregivers.FindAsync(id);
            if (caregiver == null) return null;

            _context.Caregivers.Remove(caregiver);
            await _context.SaveChangesAsync();
            return caregiver;
        }


        public async Task<List<CaregiverPatientDTO>> GetPatientsWithMedicationsAsync(string caregiverUserId)
        {
            var caregiver = await _context.Caregivers
                .FirstOrDefaultAsync(c => c.UserId == caregiverUserId);

            if (caregiver == null)
                return new List<CaregiverPatientDTO>();

            int caregiverId = caregiver.CaregiverID;

            var patients = await _context.PatientCaregivers
                .Where(pc => pc.CaregiverID == caregiverId)
                .Select(pc => pc.Patient)
                .ProjectTo<CaregiverPatientDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            foreach (var patient in patients)
            {
                var medications = (await _medService
                    .GetAllMedicationsForPatientAsync(patient.PatientID))
                    .Select(m => new PatientMedicationDTO
                    {
                        MedicationID = m.MedicationID,
                        Name = m.Name,
                    })
                    .ToList();

                patient.Medications = medications;
            }

            return patients;
        }

    }
}
