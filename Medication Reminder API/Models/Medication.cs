namespace Medication_Reminder_API.Models
{

        public class Medication
        {
            public int MedicationID { get; set; }
            public string Name { get; set; }
            public int Frequency { get; set; }
            public int DurationInDays { get; set; }
            public string Notes { get; set; }
            public MedicationStatus Status { get; set; }
        // public int PatientID { get; set; }  
        //public Patient Patient { get; set; }
        public ICollection<PatientMedication> PatientMedications { get; set; } = new List<PatientMedication>();

    }
}
