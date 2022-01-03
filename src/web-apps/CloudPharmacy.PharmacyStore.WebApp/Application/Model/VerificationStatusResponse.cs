namespace CloudPharmacy.PharmacyStore.WebApp.Application.Model
{
    internal class VerificationStatusResponse
    {
        public string RequestId { get; set; }
        public string Code { get; set; }
        public string State { get; set; }
        public VerificationError Error { get; set; }
        public string UserId { get; set; }
    }

    internal class VerificationError
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

    internal static class VerificationStatus
    {
        public const string RequestOpenedInAuthenticatorApp = "request_retrieved";
        public const string VerifiableCredentialSuccessfullyPresented = "presentation_verified";
        public const string PresentationError = "presentation_error";
    }
}
