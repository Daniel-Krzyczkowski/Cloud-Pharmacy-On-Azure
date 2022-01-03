using System.Text.Json.Serialization;

namespace CloudPharmacy.Patient.API.Application.Model
{
    public class Prescription
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("patientName")]
        public string PatientName { get; set; }
        [JsonPropertyName("recommendedMedicaments")]
        public string RecommendedMedicaments { get; set; }
        [JsonPropertyName("issuedBy")]
        public string IssuedBy { get; set; }
        [JsonPropertyName("physicianId")]
        public string PhysicianId { get; set; }
        [JsonPropertyName("patientId")]
        public string PatientId { get; set; }
        [JsonPropertyName("issuanceDate")]
        public DateTime IssuanceDate { get; set; }
        [JsonPropertyName("expirationDate")]
        public DateTime ExpirationDate { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}
