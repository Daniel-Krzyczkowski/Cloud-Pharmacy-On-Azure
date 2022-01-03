using System.Text.Json.Serialization;

namespace CloudPharmacy.Physician.API.Application.DTO
{
    public class UpdatePhysicianProfileDTO
    {
        [JsonPropertyName("Specialization")]
        public string Specialization { get; set; }
        public IFormFile? PhotoFile { get; set; }
    }
}
