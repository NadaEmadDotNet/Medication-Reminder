namespace Medication_Reminder_API.Services.Interfaces
{
    public interface IPatient
    {
           Task<List<PatientDto>> GetAllPatientsAsync(string? doctorId, string? caregiverId, string? patientId);
            Task<List<PatientDto>> GetByNameAsync(string name);
            Task<List<PatientDto>> GetByIdsAsync(IEnumerable<int> ids);
            Task<PatientDto> AddAsync(PatientDto dto);
            Task<PatientDto?> EditPatientAsync(int id, PatientDto dto);
            Task<PatientDto?> DeletePatientAsync(int id);
            Task<ServiceResult> AssignMedicationToPatientAsync(int patientId, int medicationId);
        }

    }
