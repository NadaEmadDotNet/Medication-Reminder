using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Medication_Reminder_API.DTOS;
using Medication_Reminder_API.Services.Interfaces;

namespace Medication_Reminder_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO dto)
        {
            if (dto == null)
                return BadRequest("User data is required.");

            try
            {
                // السيرفيس نفسه مسؤول عن إضافة ال Identity User + إضافة record في الجدول المناسب
                var user = await _userService.Createuser(dto);

                return Ok(new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    dto.Role
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("AllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            if (!users.Any())
                return NotFound("No users found.");

            return Ok(users);
        }
    }
}
