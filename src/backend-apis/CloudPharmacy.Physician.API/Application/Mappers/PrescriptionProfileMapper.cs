using AutoMapper;
using CloudPharmacy.Physician.API.Application.DTO;
using CloudPharmacy.Physician.Application.Model;

namespace CloudPharmacy.Physician.API.Application.Mappers
{
    public class PrescriptionProfileMapper : Profile
    {
        public PrescriptionProfileMapper()
        {
            CreateMap<PatientPrescriptionDTO, Prescription>();
            CreateMap<NewPatientPrescriptionDTO, Prescription>();
            CreateMap<Prescription, PatientPrescriptionDTO>();
        }
    }
}
