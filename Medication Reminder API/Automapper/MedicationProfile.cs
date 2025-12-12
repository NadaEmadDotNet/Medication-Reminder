using AutoMapper;
using Medication_Reminder_API.Models;
namespace Medication_Reminder_API.Automapper
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
