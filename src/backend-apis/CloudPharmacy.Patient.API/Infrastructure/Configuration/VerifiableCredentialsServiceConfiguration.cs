using Microsoft.Extensions.Options;

namespace CloudPharmacy.Patient.API.Infrastructure.Configuration
{
    public interface IVerifiableCredentialsServiceConfiguration
    {
        string IssuanceEndpointUrl { get; set; }
        string Scope { get; set; }
        string Authority { get; set; }
    }

    internal class VerifiableCredentialsServiceConfiguration : IVerifiableCredentialsServiceConfiguration
    {
        public string IssuanceEndpointUrl { get; set; }
        public string Scope { get; set; }
        public string Authority { get; set; }
    }

    internal class VerifiableCredentialsServiceConfigurationValidation : IValidateOptions<VerifiableCredentialsServiceConfiguration>
    {
        public ValidateOptionsResult Validate(string name, VerifiableCredentialsServiceConfiguration options)
        {
            if (string.IsNullOrEmpty(options.IssuanceEndpointUrl))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.IssuanceEndpointUrl)} configuration parameter for the Verifiable Credentials Service is required");
            }

            if (string.IsNullOrEmpty(options.Scope))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Scope)} configuration parameter for the Verifiable Credentials Service is required");
            }

            if (string.IsNullOrEmpty(options.Authority))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Authority)} configuration parameter for the Verifiable Credentials Service is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
