namespace CloudPharmacy.VerifiableCredentials.API.Application.Model
{
    public class VerificationStatusResponse
    {
        public string RequestId { get; set; }
        public string Code { get; set; }
        public string State { get; set; }
        public VerificationError Error { get; set; }
        public string UserId { get; set; }
    }

    public class VerificationError
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public static class VerificationStatus
    {
        public const string RequestOpenedInAuthenticatorApp = "request_retrieved";
        public const string VerifiableCredentialSuccessfullyPresented = "presentation_verified";
        public const string PresentationError = "presentation_error";
    }
}
