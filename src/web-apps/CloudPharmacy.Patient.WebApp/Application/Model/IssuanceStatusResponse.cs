namespace CloudPharmacy.Patient.WebApp.Application.Model
{
    internal class IssuanceStatusResponse
    {
        public string RequestId { get; set; }
        public string Code { get; set; }
        public string State { get; set; }
        public IssuanceError Error { get; set; }
        public string UserId { get; set; }
    }

    internal class IssuanceError
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

    internal static class IssuanceStatus
    {
        public const string QrCodeScannedByUser = "request_retrieved";
        public const string VerifiableCredentialSuccessfullyIssued = "issuance_successful";
        public const string IssuanceError = "issuance_error";
    }
}
