using Medication_Reminder_API.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Medication_Reminder_API.Api.Middleware
{
    public class TokenVersionMiddleware
    {
        private readonly RequestDelegate _next;
        public TokenVersionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var tokenVersionClaim = context.User.FindFirstValue("tokenVersion");


                if (userId != null && tokenVersionClaim != null)
                {
                    var user= await userManager.FindByIdAsync(userId);
                    if (user == null ||
                        user.TokenVersion.ToString() != tokenVersionClaim ||
                        !user.IsActive)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }
            }
            // علشان يسلم الشغل للميدل وير اللي بعده أو الكنترولر..
            await _next(context);
        }

    }
}
