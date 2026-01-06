using AutoMapper;
using Medication_Reminder_API.Domain.Models;

namespace Medication_Reminder_API.Application.Automapper
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
