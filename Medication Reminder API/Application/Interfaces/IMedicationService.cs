using Medication_Reminder_API.Application.DTOS;

namespace Medication_Reminder_API.Application.Interfaces
{
    public interface IMedicationService
    {
        Task<PagedResult<MedicationDTO>> GetAllMedicationsAsync(int page, int pageSize);
        Task<List<MedicationDTO>> GetByNameAsync(string name);
        Task<List<MedicationDTO>> GetAllMedicationsForPatientAsync(int patientId);
        Task<MedicationDTO> AddAsync(MedicationDTO dto);
        Task<MedicationDTO?> EditMedicationAsync(int id, MedicationDTO dto);
        Task<MedicationDTO?> DeleteMedicationAsync(int id);
        Task<MedicationDTO?> UpdateStatusAsync(int medicationId);
    }
}
