using Microsoft.Extensions.Options;

namespace CloudPharmacy.Patient.WebApp.Infrastructure.Configuration
{
    public interface IVerifiableCredentialsNotificationServiceConfiguration
    {
        string Url { get; set; }
    }

    internal class VerifiableCredentialsNotificationServiceConfiguration : IVerifiableCredentialsNotificationServiceConfiguration
    {
        public string Url { get; set; }
    }

    internal class VerifiableCredentialsNotificationServiceConfigurationValidation : IValidateOptions<VerifiableCredentialsNotificationServiceConfiguration>
    {
        public ValidateOptionsResult Validate(string name, VerifiableCredentialsNotificationServiceConfiguration options)
        {
            if (string.IsNullOrEmpty(options.Url))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Url)} configuration parameter for the Verifiable Credentials Notification Service is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
