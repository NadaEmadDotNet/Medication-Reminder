namespace Medication_Reminder_API.Models
{
    public class PatientCaregiver
    {
        public int PatientID { get; set; }       
        public int CaregiverID { get; set; }     

        public Patient Patient { get; set; }
        public Caregiver Caregiver { get; set; }
    }
}
