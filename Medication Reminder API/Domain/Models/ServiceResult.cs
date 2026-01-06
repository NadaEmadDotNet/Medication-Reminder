namespace Medication_Reminder_API.Domain.Models
{
    public class ServiceResult
    {
        public bool Success { get; set; }    
        public string Message { get; set; } = string.Empty;
    }
}
