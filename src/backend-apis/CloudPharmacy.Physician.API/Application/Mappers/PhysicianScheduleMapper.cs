using AutoMapper;
using CloudPharmacy.Physician.API.Application.DTO;
using CloudPharmacy.Physician.Application.Model;

namespace CloudPharmacy.Physician.API.Application.Mappers
{
    public class PhysicianScheduleMapper : Profile
    {
        public PhysicianScheduleMapper()
        {
            CreateMap<PhysicianFreeScheduleSlotDTO, PhysicianScheduleSlot>();
            CreateMap<PhysicianScheduleSlotForPatientDTO, PhysicianScheduleSlot>();
            CreateMap<PhysicianScheduleSlot, PhysicianScheduleSlotForPatientDTO>();
            CreateMap<PhysicianScheduleSlot, PhysicianFreeScheduleSlotDTO>();
        }
    }
}
