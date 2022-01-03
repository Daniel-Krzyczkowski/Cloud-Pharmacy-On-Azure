using System.Text.Json.Serialization;

namespace CloudPharmacy.Physician.Application.Model
{
    public class PatientProfile
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        [JsonPropertyName("birthDate")]
        public DateTime BirthDate { get; set; }
        [JsonPropertyName("nationalHealthcareId")]
        public string NationalHealthcareId { get; set; }
    }
}
