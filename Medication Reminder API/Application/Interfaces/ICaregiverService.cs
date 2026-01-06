using Medication_Reminder_API.Application.DTOS;
using Medication_Reminder_API.Domain.Models;
using System.Collections.Generic;

namespace Medication_Reminder_API.Application.Interfaces
{
    public interface ICaregiverService
    {
        Task<List<Caregiver>> GetAllCaregiversAsync();
        Task<Caregiver?> GetCaregiverByNameAsync(string name);
        Task<string> AssignPatientToCaregiverAsync(CaregiverAssignDTO dto);
        Task<Caregiver?> EditCaregiverAsync(int id, Caregiver caregiver);
        Task<Caregiver?> DeleteCaregiverAsync(int id);
        Task<List<CaregiverPatientDTO>> GetPatientsWithMedicationsAsync(string caregiverUserId);

    }
}
