using AutoMapper;
using Medication_Reminder_API.DTOS;
using Medication_Reminder_API.Models;

namespace Medication_Reminder_API.Services
{
        public class MedicationService : IMedicationService
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IDoseLogService _logService; // استخدم Interface فقط

            public MedicationService(ApplicationDbContext context, IMapper mapper, IDoseLogService logService)
            {
                _context = context;
                _mapper = mapper;
                _logService = logService;
            }
        


        public List<MedicationDTO> GetAllMedications()
        {
            var meds = _context.Medications.ToList();
            return _mapper.Map<List<MedicationDTO>>(meds);
        }

        public List<MedicationDTO> GetByName(string name)
        {
            var meds = _context.Medications
                .Where(m => m.Name.Contains(name))
                .ToList();
            return _mapper.Map<List<MedicationDTO>>(meds);
        }

        public List<MedicationDTO> GetAllMedicationsForPatient(int patientId)
        {
            var meds = _context.PatientMedications
                .Where(m => m.PatientID == patientId)
                 .Select(pm => pm.Medication)
                 .ToList();
            return _mapper.Map<List<MedicationDTO>>(meds);
        }

        public MedicationDTO Add(MedicationDTO dto)
        {
            bool exists = _context.Medications
                .Any(m => m.Name.ToLower() == dto.Name.ToLower());
            if (exists)
            {
                throw new InvalidOperationException($"Medication with name '{dto.Name}' already exists for this patient.");
            }

            var med = _mapper.Map<Medication>(dto);
            med.Status = MedicationStatus.Ongoing; // Default Status

            _context.Medications.Add(med);
            _context.SaveChanges();

            return _mapper.Map<MedicationDTO>(med);
        }

        public MedicationDTO? EditMedication(int id, MedicationDTO dto)
        {
            var med = _context.Medications.Find(id);
            if (med == null) return null;

            _mapper.Map(dto, med);
            _context.SaveChanges();
            return _mapper.Map<MedicationDTO>(med);
        }

        public MedicationDTO? DeleteMedication(int id)
        {
            var med = _context.Medications.Find(id);
            if (med == null) return null;

            _context.Medications.Remove(med);
            _context.SaveChanges();
            return _mapper.Map<MedicationDTO>(med);
        }


        public MedicationDTO? UpdateStatus(int medicationId)
        {
            var med = _context.Medications.Find(medicationId);
            if (med == null) 
                return null;

            // إجمالي كل الجرعات المجدولة للدواء
            var totalScheduled = _context.DoseLogs
                .Count(d => d.MedicationID == med.MedicationID);

            // عدد الجرعات التي أخذت بالفعل
            var takenCount = _context.DoseLogs
                .Count(d => d.MedicationID == med.MedicationID && d.Status == DoseStatus.Taken);

            // تحديث الحالة بناءً على الجرعات
            if (takenCount > totalScheduled)
                med.Status = MedicationStatus.Overtaken;
            else if (takenCount == totalScheduled)
                med.Status = MedicationStatus.Completed;
            else
                med.Status = MedicationStatus.Ongoing;

            _context.SaveChanges();
            return _mapper.Map<MedicationDTO>(med);
        }

    }
}

