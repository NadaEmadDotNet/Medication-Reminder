namespace Medication_Reminder_API.Models
{
    public class Caregiver
    {
            public int CaregiverID { get; set; }     // PK
             public string UserId { get; set; }  
             public string Name { get; set; }
            public string RelationToPatient { get; set; }
            public ApplicationUser User { get; set; }
        public ICollection<PatientCaregiver> PatientCaregivers { get; set; }
    }
 }