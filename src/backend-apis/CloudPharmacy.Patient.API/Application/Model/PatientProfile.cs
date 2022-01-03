using System.Text.Json.Serialization;

namespace CloudPharmacy.Patient.API.Application.Model
{
    public class PatientProfile
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("firstNameAndLastName")]
        public string FirstNameAndLastName { get; set; }
        [JsonPropertyName("nationalHealthcareId")]
        public string NationalHealthcareId { get; set; }
    }
}
