using CloudPharmacy.Physician.Application.Model;

namespace CloudPharmacy.Physician.API.Application.Repositories
{
    public interface IPhysicianRepository
    {
        Task<PhysicianProfile> GetProfileAsync(string physicianId);
        Task CreateProfileAsync(PhysicianProfile physicianProfile);
        Task UpdateProfileAsync(PhysicianProfile physician);
        Task<IList<PhysicianProfile>> GetAllProfilesAsync();
    }
}
