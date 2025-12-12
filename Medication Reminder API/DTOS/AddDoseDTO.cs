public class AddDoseDTO
{
    public int PatientID { get; set; }
    public int MedicationID { get; set; }
    public DateTime ScheduledTime { get; set; }
    public string? Notes { get; set; }
}