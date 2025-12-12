using Medication_Reminder_API.Services;

public interface IDoseLogService
{
    List<DoseLogDTO> GetDosesForPatient(int patientId, DoseStatus? status = null);
    List<DoseLogDTO> GetDosesByMedicationName(int patientId, string name);
    DoseLogDTO? GetDoseById(int id);
    DoseLogDTO AddDose(AddDoseDTO dto);
    DoseLogDTO? UpdateTakenTime(int doseLogId, DateTime takenTime, IMedicationService medService);
    int GetTakenDoseCount(int medicationId);
}
