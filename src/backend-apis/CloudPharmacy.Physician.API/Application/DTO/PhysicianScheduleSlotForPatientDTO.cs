namespace CloudPharmacy.Physician.API.Application.DTO
{
    public class PhysicianScheduleSlotForPatientDTO : PhysicianFreeScheduleSlotDTO
    {
        public PatientProfileDTO Patient { get; set; }
    }
}
