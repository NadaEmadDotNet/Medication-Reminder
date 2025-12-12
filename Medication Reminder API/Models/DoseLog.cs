using System.ComponentModel.DataAnnotations.Schema;

namespace Medication_Reminder_API.Models
{
    public class DoseLog
    {
        public int DoseLogID { get; set; }
        public int MedicationID { get; set; }

        public int PatientID { get; set; }
        public DateTime ScheduledTime { get; set; } // الوقت اللي المفروض ياخد فيه الجرعة
        public DateTime? TakenTime { get; set; }    // الوقت اللي فعليًا أخد فيه الجرعة
        public DoseStatus Status { get; set; }        // Taken / Missed / Skipped
        public string? Notes { get; set; }
        public Medication Medication { get; set; }
        public Patient Patient { get; set; }
    }
}
