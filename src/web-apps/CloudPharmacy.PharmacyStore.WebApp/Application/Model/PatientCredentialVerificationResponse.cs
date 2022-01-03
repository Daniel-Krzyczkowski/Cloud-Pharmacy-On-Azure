namespace CloudPharmacy.PharmacyStore.WebApp.Application.Model
{
    internal class PatientCredentialVerificationResponse
    {
        public string Id { get; set; }
        public string RequestId { get; set; }
        public string Url { get; set; }
        public int Expiry { get; set; }
        public string QrCode { get; set; }
    }
}
