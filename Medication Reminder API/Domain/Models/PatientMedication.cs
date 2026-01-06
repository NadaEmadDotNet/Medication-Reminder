using Medication_Reminder_API.Domain.Models;

public class PatientMedication
{
    public int PatientID { get; set; }
    public Patient Patient { get; set; }

    public int MedicationID { get; set; }
    public Medication Medication { get; set; }
}