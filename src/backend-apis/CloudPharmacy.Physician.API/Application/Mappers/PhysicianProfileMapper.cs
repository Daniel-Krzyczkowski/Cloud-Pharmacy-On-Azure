using AutoMapper;
using CloudPharmacy.Physician.API.Application.DTO;
using CloudPharmacy.Physician.Application.Model;

namespace CloudPharmacy.Physician.API.Application.Mappers
{
    public class PhysicianProfileMapper : Profile
    {
        public PhysicianProfileMapper()
        {
            CreateMap<PhysicianProfileDTO, PhysicianProfile>();
            CreateMap<PhysicianProfile, PhysicianProfileDTO>();
            CreateMap<UpdatePhysicianProfileDTO, PhysicianProfile>()
                     .ForSourceMember(x => x.PhotoFile, opt => opt.DoNotValidate());
        }
    }
}
