using Medication_Reminder_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Medication_Reminder_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : BaseController
    {
        private readonly IPatient _patientService;

        public PatientController(IPatient patientService, ApplicationDbContext context) : base(context)
        {
            _patientService = patientService;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllPatients()
        {
            var userId = GetCurrentUserId();
            var role = GetCurrentUserRole();

            var patients = await _patientService.GetAllPatientsAsync(
                doctorId: role == "Doctor" ? userId : null,
                caregiverId: role == "Caregiver" ? userId : null,
                patientId: role == "Patient" ? userId : null
            );

            if (!patients.Any())
                return NotFound("No patients found.");

            return Ok(patients);
        }

        [HttpGet("ByName")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var patients = await _patientService.GetByNameAsync(name);

            if (!patients.Any())
                return NotFound($"No patients found with name containing '{name}'.");

            return Ok(patients);
        }

        //[HttpPost]
        //[Authorize(Roles = "Admin,Doctor")]
        //public async Task<IActionResult> AddPatient([FromBody] PatientDto patientDto)
        //{
        //    if (patientDto == null)
        //        return BadRequest("Patient data is empty.");

        //    var addedPatient = await _patientService.AddAsync(patientDto);
        //    return Ok(addedPatient);
        //}

        [HttpPost("{patientId}/AssignMedication/{medicationId}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> AssignMedicationToPatient(int patientId, int medicationId)
        {
            var result = await _patientService.AssignMedicationToPatientAsync(patientId, medicationId);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> EditPatient(int id, [FromBody] PatientDto patientDto)
        {
            var updatedPatient = await _patientService.EditPatientAsync(id, patientDto);
            if (updatedPatient == null)
                return NotFound("Patient not found.");

            return Ok(updatedPatient);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var deletedPatient = await _patientService.DeletePatientAsync(id);
            if (deletedPatient == null)
                return NotFound("Patient not found.");

            return Ok(deletedPatient);
        }
    }
}
