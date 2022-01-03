namespace CloudPharmacy.VerifiableCredentials.API.Application.Model
{
    public class IssuanceRequest
    {
        public bool IncludeQRCode { get; set; }
        public Callback Callback { get; set; }
        public string Authority { get; set; }
        public Registration Registration { get; set; }
        public Issuance Issuance { get; set; }
    }

    public class Pin
    {
        public string Value { get; set; }
        public int Length { get; set; }
    }

    public class Claims
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }
    }

    public class Issuance
    {
        public string Type { get; set; }
        public string Manifest { get; set; }
        public Pin Pin { get; set; }
        public Claims Claims { get; set; }
    }
}
