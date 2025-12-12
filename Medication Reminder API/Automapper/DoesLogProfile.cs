using AutoMapper;
using Medication_Reminder_API.DTOS;
using Medication_Reminder_API.Models;

namespace Medication_Reminder_API.Automapper
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
