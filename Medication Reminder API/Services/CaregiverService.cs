using Medication_Reminder_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Medication_Reminder_API.Services
{
    public class CaregiverService : ICaregiverService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMedicationService _medService;
        private readonly IDoseLogService _logService;

        public CaregiverService(ApplicationDbContext context, IMedicationService medService, IDoseLogService logService)
        {
            _context = context;
            _medService = medService;
            _logService = logService;
        }

        public List<Caregiver> GetAllCaregivers() => _context.Caregivers.ToList();

        public Caregiver? GetCaregiverByName(string name) =>
            _context.Caregivers.FirstOrDefault(c => c.Name == name);

        public string AssignPatientToCaregiver(CaregiverAssignDTO dto)
        {
            if (dto == null)
                throw new ArgumentException("Data is empty.");

            // 1) جيبي الـ Caregiver بالـ UserId
            var caregiver = _context.Caregivers
                                    .FirstOrDefault(c => c.UserId == dto.UserId);

            if (caregiver == null)
            {
                caregiver = new Caregiver
                {
                    UserId = dto.UserId,
                    Name = dto.Name,
                    RelationToPatient = dto.RelationToPatient
                };
                _context.Caregivers.Add(caregiver);
                _context.SaveChanges();
            }

            // 2) 👈 ضيفي هنا check الربط قبل ما تعملي Add
            var isAlreadyLinked = _context.PatientCaregivers
                .Any(pc => pc.CaregiverID == caregiver.CaregiverID
                           && pc.PatientID == dto.PatientID);

            if (isAlreadyLinked)
            {
                return "This caregiver is already assigned to this patient.";
            }

            // 3) لو مش متربط → اعملي الربط
            var patientCaregiver = new PatientCaregiver
            {
                CaregiverID = caregiver.CaregiverID,
                PatientID = dto.PatientID
            };

            _context.PatientCaregivers.Add(patientCaregiver);
            _context.SaveChanges();

            return "Caregiver linked to patient successfully";
        }

        public Caregiver? EditCaregiver(int id, Caregiver caregiver)
        {
            var existing = _context.Caregivers.Find(id);
            if (existing == null) return null;

            existing.RelationToPatient = caregiver.RelationToPatient;
            _context.SaveChanges();
            return existing;
        }

        public Caregiver? DeleteCaregiver(int id)
        {
            var caregiver = _context.Caregivers.Find(id);
            if (caregiver == null) return null;

            _context.Caregivers.Remove(caregiver);
            _context.SaveChanges();
            return caregiver;
        }

        // 👉 Get all patients with medications for a caregiver
          public List<CaregiverPatientDTO> GetPatientsWithMedications(string caregiverUserId)
          {

            // أولًا نجيب Caregiver حسب الـ UserId (string)
            var caregiver = _context.Caregivers.FirstOrDefault(c => c.UserId == caregiverUserId);
            if (caregiver == null)
                return new List<CaregiverPatientDTO>();

            int caregiverId = caregiver.CaregiverID; // هذا int

            // بعدين نجيب المرضى
            var patients = _context.PatientCaregivers
                .Where(pc => pc.CaregiverID == caregiverId) // الآن int == int
                .Select(pc => pc.Patient)
                .Select(p => new CaregiverPatientDTO
                {
                    PatientID = p.PatientID,
                    Name = p.Name,
                    Age = p.Age,
                    Gender = p.Gender,
                    ChronicConditions = p.ChronicConditions
                })
                .ToList();

            foreach (var patient in patients)
            {
                var medications = _medService.GetAllMedicationsForPatient(patient.PatientID)
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
