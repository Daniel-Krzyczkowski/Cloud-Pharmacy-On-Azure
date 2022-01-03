using AutoMapper;
using CloudPharmacy.Patient.API.Application.DTO;
using CloudPharmacy.Patient.API.Application.Model;

namespace CloudPharmacy.Patient.API.Application.Mappers
{
    public class PatientProfileMapper : Profile
    {
        public PatientProfileMapper()
        {
            CreateMap<PatientProfileDTO, PatientProfile>();
            CreateMap<PatientProfile, PatientProfileDTO>();
            CreateMap<UpdatePatientProfileDTO, PatientProfile>();
        }
    }
}
