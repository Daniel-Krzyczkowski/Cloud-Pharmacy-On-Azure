using CloudPharmacy.Physician.Application.Model;

namespace CloudPharmacy.Physician.API.Application.Repositories
{
    public interface IPhysicianScheduleSlotRepository
    {
        Task AddNewSlotToScheduleAsync(PhysicianScheduleSlot physicianScheduleSlot);
        Task<IList<PhysicianScheduleSlot>> GetAllScheduleSlotsAsync(string physicianId);
        Task<IList<PhysicianScheduleSlot>> GetFreeSlotsFromScheduleAsync(string physicianId);
        Task<IList<PhysicianScheduleSlot>> GetReservedSlotsFromScheduleAsync(string physicianId);
        Task DeleteScheduleSlotAsync(string scheduleSlotId, string physicianId);
    }
}
