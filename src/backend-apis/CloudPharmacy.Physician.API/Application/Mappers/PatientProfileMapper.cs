using AutoMapper;
using CloudPharmacy.Physician.API.Application.DTO;
using CloudPharmacy.Physician.Application.Model;

namespace CloudPharmacy.Physician.API.Application.Mappers
{
    public class PatientProfileMapper : Profile
    {
        public PatientProfileMapper()
        {
            CreateMap<PatientProfileDTO, PatientProfile>();
            CreateMap<PatientProfile, PatientProfileDTO>();
        }
    }
}
