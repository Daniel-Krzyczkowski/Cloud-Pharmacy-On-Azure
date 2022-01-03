using System.Text.Json.Serialization;

namespace CloudPharmacy.Physician.Application.Model
{
    public class PhysicianScheduleSlot
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("physicianId")]
        public string PhysicianId { get; set; }
        [JsonPropertyName("slotDateAndTime")]
        public DateTimeOffset SlotDateAndTime { get; set; }
        [JsonPropertyName("patient")]
        public PatientProfile Patient { get; set; }
    }
}
