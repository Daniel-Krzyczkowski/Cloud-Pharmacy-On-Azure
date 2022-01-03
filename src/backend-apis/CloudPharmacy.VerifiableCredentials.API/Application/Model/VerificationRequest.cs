namespace CloudPharmacy.VerifiableCredentials.API.Application.Model
{
    public class VerificationRequest
    {
        public bool IncludeQRCode { get; set; }
        public Callback Callback { get; set; }
        public string Authority { get; set; }
        public Registration Registration { get; set; }
        public Presentation Presentation { get; set; }
    }

    public class RequestedCredential
    {
        public string Type { get; set; }
        public string Purpose { get; set; }
        public List<string> AcceptedIssuers { get; set; }
    }

    public class Presentation
    {
        public bool IncludeReceipt { get; set; }
        public List<RequestedCredential> RequestedCredentials { get; set; }
    }
}
