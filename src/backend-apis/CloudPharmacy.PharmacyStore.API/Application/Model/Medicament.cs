using System.Text.Json.Serialization;

namespace CloudPharmacy.PharmacyStore.API.Application.Model
{
    public class Medicament
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("producer")]
        public string Producer { get; set; }
        [JsonPropertyName("pictureUrl")]
        public string PictureUrl { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
}
