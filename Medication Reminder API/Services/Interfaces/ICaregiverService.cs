using Medication_Reminder_API.Models;
using System.Collections.Generic;

namespace Medication_Reminder_API.Services
{
    public interface ICaregiverService
    {
        List<Caregiver> GetAllCaregivers();
        Caregiver? GetCaregiverByName(string name);
        string AssignPatientToCaregiver(CaregiverAssignDTO dto);
       Caregiver? EditCaregiver(int id, Caregiver caregiver);
        Caregiver? DeleteCaregiver(int id);
        public List<CaregiverPatientDTO> GetPatientsWithMedications(string caregiverUserId);

    }
}
