using CloudPharmacy.PharmacyStore.API.Application.Model;

namespace CloudPharmacy.PharmacyStore.API.Application.Repositories
{
    public interface IMedicamentRepository
    {
        Task AddMedicamentAsync(Medicament medicament);
        Task<IList<Medicament>> GetAllMedicamentsAsync();
    }
}
