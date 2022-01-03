using System.Text.Json.Serialization;

namespace CloudPharmacy.VerifiableCredentials.API.Application.Model
{
    public class Headers
    {
        [JsonPropertyName("api-key")]
        public string ApiKey { get; set; }
    }

    public class Callback
    {
        public string Url { get; set; }
        public string State { get; set; }
        public Headers Headers { get; set; }
    }

    public class Registration
    {
        public string ClientName { get; set; }
        public string Purpose { get; set; }
    }
}
