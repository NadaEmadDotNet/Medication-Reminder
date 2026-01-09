namespace Medication_Reminder_API.Application.DTOS
{
    public class CaregiverPatientMedicationDTO
    {
        public int MedicationID { get; set; }
        public string Name { get; set; }
        public int Frequency { get; set; }
        public int DurationInDays { get; set; }
        public string? Notes { get; set; }
        public string? Status { get; set; }
        public List<DoseLogDTO> DoseLogs { get; set; } = new();
    }
}
