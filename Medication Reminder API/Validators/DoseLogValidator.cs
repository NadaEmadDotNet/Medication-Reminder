namespace Medication_Reminder_API.Validators
{
    public class DoseLogDTOValidator : AbstractValidator<DoseLogDTO>
    {
        public DoseLogDTOValidator()
        {
            // PatientName لازم يكون موجود وطوله بين 2 و 100 حرف
            RuleFor(x => x.PatientName)
                .NotEmpty().WithMessage("Patient name is required.")
                .Length(2, 100).WithMessage("Patient name must be between 2 and 100 characters.");


            RuleFor(x => x.MedicationName)
                .NotEmpty().WithMessage("Medication name is required.")
                .Length(2, 100).WithMessage("Medication name must be between 2 and 100 characters.");


            // ScheduledTime لازم يكون وقت صحيح (مستقبلي أو حالي)
            RuleFor(x => x.ScheduledTime)
                .NotEmpty().WithMessage("Scheduled time is required.")
                .Must(date => date > DateTime.MinValue)
                .WithMessage("Scheduled time must be a valid date.");


            // TakenTime لو موجود لازم يكون بعد أو يساوي ScheduledTime

            RuleFor(x => x.TakenTime)
                .GreaterThanOrEqualTo(x => x.ScheduledTime)
                .When(x => x.TakenTime.HasValue)
                .WithMessage("Taken time cannot be earlier than scheduled time.");


            // Status لازم يكون واحد من القيم المسموح بها (Enum)
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Status must be a valid DoseStatus.");


            RuleFor(x => x.Notes)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.Notes))
                .WithMessage("Notes cannot exceed 500 characters.");
        }
    }
}
