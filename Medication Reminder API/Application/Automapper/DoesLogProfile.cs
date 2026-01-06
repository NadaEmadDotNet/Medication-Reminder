using AutoMapper;
using Medication_Reminder_API.Domain.Models;
using Medication_Reminder_API.Application.Validators;

namespace Medication_Reminder_API.Application.Automapper
{
    public class DoesLogProfile: Profile
    {
        public DoesLogProfile()
        {
            CreateMap<DoseLog, DoseLogDTO>();
            CreateMap<DoseLogDTO, DoseLog>();
        }
    }
}
