namespace CloudPharmacy.PharmacyStore.WebApp.Application.Model
{
    public class Medicament
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Producer { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
    }
}
