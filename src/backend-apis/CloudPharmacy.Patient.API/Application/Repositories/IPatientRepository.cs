using CloudPharmacy.Patient.API.Application.Model;

namespace CloudPharmacy.Patient.API.Application.Repositories
{
    public interface IPatientRepository
    {
        Task<PatientProfile> GetProfileAsync(string patientId);
        Task CreateProfileAsync(PatientProfile patient);
        Task UpdateProfileAsync(PatientProfile patientProfile);
    }
}
