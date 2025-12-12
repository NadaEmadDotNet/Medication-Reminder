using Medication_Reminder_API.DTOS;

namespace Medication_Reminder_API.Services
{
    public interface IMedicationService
    {
        List<MedicationDTO> GetAllMedications();
        List<MedicationDTO> GetByName(string name);
        List<MedicationDTO> GetAllMedicationsForPatient(int patientId);

        MedicationDTO Add(MedicationDTO dto);
        MedicationDTO EditMedication(int id, MedicationDTO dto);
        MedicationDTO DeleteMedication(int id);

        MedicationDTO UpdateStatus(int medicationId);
    }
}
