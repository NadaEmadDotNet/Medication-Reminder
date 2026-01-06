namespace Medication_Reminder_API.Application.DTOS
{
    public class CaregiverAssignDTO
    {
        public string UserId { get; set; }  
        public string Name { get; set; }
        public int PatientID { get; set; }   
        public string RelationToPatient { get; set; } 
    }
}
