namespace Medication_Reminder_API.Models
{
    public class Patient
    {
        public int PatientID { get; set; }
        public string? UserId { get; set; } 
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public string ChronicConditions { get; set; } = "";
        public ApplicationUser? User { get; set; }
        public ICollection<DoseLog> DoseLogs { get; set; } 
        public ICollection<PatientCaregiver> PatientCaregivers { get; set; }
        public ICollection<PatientMedication> PatientMedications { get; set; }
        public ICollection<DoctorPatient> DoctorPatients { get; set; }
    }
}
