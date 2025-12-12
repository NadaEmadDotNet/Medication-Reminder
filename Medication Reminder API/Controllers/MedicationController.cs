using AutoMapper;
using Medication_Reminder_API.DTOS;
using Medication_Reminder_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Medication_Reminder_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MedicationsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IMedicationService _medService;

        public MedicationsController(ApplicationDbContext context, IMapper mapper, IMedicationService medService)
            : base(context)
        {
            _mapper = mapper;
            _medService = medService;
        }

        [HttpGet]
        [Authorize(Roles = "Doctor, Admin")]
        public IActionResult GetAllMedications()
        {
            var meds = _medService.GetAllMedications();
            return Ok(meds);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Doctor, Admin")]
        public IActionResult GetByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name is required.");

            var meds = _medService.GetByName(name);
            if (!meds.Any())
                return NotFound($"No medications found containing '{name}'.");

            return Ok(meds);
        }

        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = "Patient,Doctor,Caregiver, Admin")]
        public IActionResult GetAllMedicationsForPatient(int patientId)
        {
            if (!CanAccessPatient(patientId)) return Forbid();

            var meds = _medService.GetAllMedicationsForPatient(patientId);
            if (!meds.Any())
                return NotFound("No medications found for this patient.");

            return Ok(meds);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")]
        public IActionResult Add([FromBody] MedicationDTO dto)
        {
            var med = _medService.Add(dto);
            return CreatedAtAction(nameof(GetByName), new { name = med.Name }, med);
        }

        [HttpPut("{medicationId}")]
        [Authorize(Roles = "Doctor,Admin")]
        public IActionResult EditMedication(int medicationId, [FromBody] MedicationDTO medication)
        {
            var updatedMed = _medService.EditMedication(medicationId, medication);
            if (updatedMed == null)
                return NotFound("Medication not found.");

            return Ok(updatedMed);
        }

        [HttpDelete("{medicationId}")]
        [Authorize(Roles = "Doctor,Admin")]
        public IActionResult DeleteMedication(int medicationId)
        {
            var deletedMed = _medService.DeleteMedication(medicationId);
            if (deletedMed == null)
                return NotFound("Medication not found.");

            return NoContent();
        }
    }
}
