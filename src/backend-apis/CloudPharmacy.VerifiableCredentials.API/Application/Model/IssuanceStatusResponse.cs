namespace CloudPharmacy.VerifiableCredentials.API.Application.Model
{
    public class IssuanceStatusResponse
    {
        public string RequestId { get; set; }
        public string Code { get; set; }
        public string State { get; set; }
        public IssuanceError Error { get; set; }
        public string UserId { get; set; }
    }

    public class IssuanceError
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public static class IssuanceStatus
    {
        public const string QrCodeScannedByUser = "request_retrieved";
        public const string VerifiableCredentialSuccessfullyIssued = "issuance_successful";
        public const string IssuanceError = "issuance_error";
    }
}
