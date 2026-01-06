using Medication_Reminder_API.Application.DTOS;
using Medication_Reminder_API.Infrastructure;

public interface IUserService
{
    public  Task<ApplicationUser> Createuser(CreateUserDTO dto);
    public Task<List<UserDto>> GetAllUsersAsync();
}
