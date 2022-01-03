using AutoMapper;
using CloudPharmacy.PharmacyStore.API.Application.DTO;
using CloudPharmacy.PharmacyStore.API.Application.Model;

namespace CloudPharmacy.PharmacyStore.API.Application.Mappers
{
    public class MedicamentMapper : Profile
    {
        public MedicamentMapper()
        {
            CreateMap<MedicamentDTO, Medicament>();
            CreateMap<Medicament, MedicamentDTO>();
        }
    }
}
