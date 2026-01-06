using Microsoft.AspNetCore.Identity;

namespace Medication_Reminder_API.Infrastructure
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
