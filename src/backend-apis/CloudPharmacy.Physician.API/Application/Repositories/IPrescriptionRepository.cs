using CloudPharmacy.Physician.Application.Model;

namespace CloudPharmacy.Physician.API.Application.Repositories
{
    public interface IPrescriptionRepository
    {
        Task AddPrescriptionForPatientAsync(Prescription prescription);
        Task<Prescription> GetPrescriptionForPatientAsync(string prescriptionId, string patientId);
    }
}
