using Medication_Reminder_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Medication_Reminder_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class CareGiverController : ControllerBase
    {
        private readonly ICaregiverService _caregiverService;
        private readonly IMapper _mapper;

        public CareGiverController(ICaregiverService caregiverService, IMapper mapper)
        {
            _caregiverService = caregiverService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            var caregivers = _caregiverService.GetAllCaregivers();
            if (!caregivers.Any())
                return NotFound("No caregivers found.");

            return Ok(caregivers);
        }

        [HttpGet("MyPatients")]
        [Authorize(Roles = "Caregiver")]
        public IActionResult GetMyPatients()
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var patients = _caregiverService.GetPatientsWithMedications(userId);

            if (!patients.Any())
                return NotFound("No patients assigned.");

            return Ok(patients);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Doctor,Admin")]
        public IActionResult GetByName([FromQuery] string name)
        {
            var caregiver = _caregiverService.GetCaregiverByName(name);

            if (caregiver == null)
                return NotFound($"No caregiver found with name '{name}'.");

            return Ok(caregiver);
        }


        [HttpPost("AssignPatient")]
        [Authorize(Roles = "Admin")]
        public IActionResult AssignPatient([FromBody] CaregiverAssignDTO dto)
        {
            try
            {
                var result = _caregiverService.AssignPatientToCaregiver(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor,Admin")]
        public IActionResult Edit(int id, [FromBody] Caregiver caregiver)
        {
            var updated = _caregiverService.EditCaregiver(id, caregiver);

            if (updated == null)
                return NotFound("Caregiver not found.");

            return Ok(updated);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Doctor,Admin")]
        public IActionResult Delete(int id)
        {
            var deleted = _caregiverService.DeleteCaregiver(id);

            if (deleted == null)
                return NotFound("Caregiver not found.");

            return NoContent();
        }
    }
}
