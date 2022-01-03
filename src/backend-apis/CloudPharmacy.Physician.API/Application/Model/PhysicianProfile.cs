using System.Text.Json.Serialization;

namespace CloudPharmacy.Physician.Application.Model
{
    public class PhysicianProfile
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("firstNameAndLastName")]
        public string FirstNameAndLastName { get; set; }
        [JsonPropertyName("specialization")]
        public string Specialization { get; set; }
        [JsonPropertyName("photoUrl")]
        public string PhotoUrl { get; set; }

    }
}
