using Medication_Reminder_API.Application.DTOS;
using Medication_Reminder_API.Infrastructure;
using Microsoft.AspNetCore.Identity;

public interface IUserService
{
      Task<ApplicationUser> Createuser(CreateUserDTO dto);
     Task<List<UserDto>> GetAllUsersAsync();
    Task<ServiceResult> ChangeUserStatusAsync(string userId, UpdateUserStatusDto dto, UserManager<ApplicationUser> userManager);
}
