using CloudPharmacy.Patient.API.Application.Model;

namespace CloudPharmacy.Patient.API.Application.Repositories
{
    public interface IPrescriptionRepository
    {
        Task AddPrescriptionForPatientAsync(Prescription prescription);
        Task<Prescription> GetPrescriptionForPatientAsync(string prescriptionId, string patientId);
        Task<IList<Prescription>> GetAllPrescriptionsForPatientAsync(string patientId);
    }
}
