namespace CloudPharmacy.VerifiableCredentials.API.Application.Model
{
    public class VerificationResponse
    {
        public string Id { get; set; }
        public string RequestId { get; set; }
        public string Url { get; set; }
        public int Expiry { get; set; }
        public string QrCode { get; set; }
    }
}
