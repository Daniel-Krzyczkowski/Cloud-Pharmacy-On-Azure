using Microsoft.Extensions.Options;

namespace CloudPharmacy.VerifiableCredentials.API.Infrastructure.Configuration
{
    public interface ILiveNotificationsFuncAppConfiguration
    {
        string VerifiableCredentialIssuanceStatusUpdateEndpointUrl { get; set; }
        string VerifiableCredentialVerificationStatusUpdateEndpointUrl { get; set; }
    }

    internal class LiveNotificationsFuncAppConfiguration : ILiveNotificationsFuncAppConfiguration
    {
        public string VerifiableCredentialIssuanceStatusUpdateEndpointUrl { get; set; }
        public string VerifiableCredentialVerificationStatusUpdateEndpointUrl { get; set; }
    }

    internal class LiveNotificationsFuncAppConfigurationValidation : IValidateOptions<LiveNotificationsFuncAppConfiguration>
    {
        public ValidateOptionsResult Validate(string name, LiveNotificationsFuncAppConfiguration options)
        {
            if (string.IsNullOrEmpty(options.VerifiableCredentialIssuanceStatusUpdateEndpointUrl))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.VerifiableCredentialIssuanceStatusUpdateEndpointUrl)} configuration parameter for the Azure Function App is required");
            }

            if (string.IsNullOrEmpty(options.VerifiableCredentialVerificationStatusUpdateEndpointUrl))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.VerifiableCredentialVerificationStatusUpdateEndpointUrl)} configuration parameter for the Azure Function App is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
