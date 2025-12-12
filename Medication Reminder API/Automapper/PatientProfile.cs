using AutoMapper;

namespace Medication_Reminder_API.Automapper
{
    public class PatientProfile : Profile
    {
        public PatientProfile() 
        {
            CreateMap<PatientDto, Patient>();
            CreateMap<Patient, PatientDto>();
        
        }
    }
}
