namespace CloudPharmacy.Patient.WebApp.Application.Model
{
    internal class PhysicianScheduleSlot
    {
        public string PhysicianId { get; set; }
        public DateTimeOffset SlotDateAndTime { get; set; }
    }
}
