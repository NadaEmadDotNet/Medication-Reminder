using Medication_Reminder_API.DTOS;
using Medication_Reminder_API.Models;

namespace Medication_Reminder_API.DTOS
{
    public class MedicationDTO
    {
        public string Name { get; set; }
        public int MedicationID { get; set; }
        public int Frequency { get; set; }
        public int DurationInDays { get; set; }
        public string? Notes { get; set; }
    }
}