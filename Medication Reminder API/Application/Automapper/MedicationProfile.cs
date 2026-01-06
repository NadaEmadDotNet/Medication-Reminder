using AutoMapper;
using Medication_Reminder_API.Application.DTOS;
using Medication_Reminder_API.Domain.Models;

namespace Medication_Reminder_API.Application.Automapper
{
    public class MedicationProfile : Profile
    {
        public MedicationProfile()
        {
            CreateMap<MedicationDTO, Medication>();
            CreateMap<Medication, MedicationDTO>();

        }
    }
}
