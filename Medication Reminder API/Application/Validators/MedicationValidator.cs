using FluentValidation;
using Medication_Reminder_API.Application.DTOS;
namespace Medication_Reminder_API.Application.Validators
{
    public class MedicationValidator : AbstractValidator<MedicationDTO>
    {
        public MedicationValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Medication name is required");


            RuleFor(x => x.Frequency)
                .GreaterThan(0)
                .WithMessage("Frequency must be greater than 0");
             

            RuleFor(x => x.DurationInDays)
                .GreaterThan(0)
                .WithMessage("Duration must be greater than 0");

        }
    }
}
