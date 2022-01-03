using Microsoft.Extensions.Options;
using System.Globalization;

namespace CloudPharmacy.VerifiableCredentials.API.Infrastructure.Configuration
{
    public interface IVerifiableCredentialsConfiguration
    {
        string Endpoint { get; set; }
        string ApiEndpoint { get; }
        string VerifiableCredentialsServiceScope { get; set; }
        string Instance { get; set; }
        string IssuanceType { get; set; }
        string TokenAuthority { get; }
        string TenantId { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string IssuerAuthority { get; set; }
        string VerifierAuthority { get; set; }
        string CredentialManifest { get; set; }
        string IssuanceCallbackUrl { get; set; }
        string IssuanceCallbackApiKey { get; set; }
        string IssuanceRegistrationClientName { get; set; }
        string IssuanceRegistrationPurpose { get; set; }
        string PresentationCallbackUrl { get; set; }
        string PresentationCallbackApiKey { get; set; }
    }

    internal class VerifiableCredentialsConfiguration : IVerifiableCredentialsConfiguration
    {
        public string Endpoint { get; set; }
        public string ApiEndpoint
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, Endpoint, TenantId);
            }
        }

        public string VerifiableCredentialsServiceScope { get; set; }
        public string Instance { get; set; }
        public string IssuanceType { get; set; }
        public string TokenAuthority
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, Instance, TenantId);
            }
        }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string IssuerAuthority { get; set; }
        public string VerifierAuthority { get; set; }
        public string CredentialManifest { get; set; }
        public string IssuanceCallbackUrl { get; set; }
        public string IssuanceCallbackApiKey { get; set; }
        public string IssuanceRegistrationClientName { get; set; }
        public string IssuanceRegistrationPurpose { get; set; }
        public string PresentationCallbackUrl { get; set; }
        public string PresentationCallbackApiKey { get; set; }
    }

    internal class VerifiableCredentialsConfigurationValidation : IValidateOptions<VerifiableCredentialsConfiguration>
    {
        public ValidateOptionsResult Validate(string name, VerifiableCredentialsConfiguration options)
        {
            if (string.IsNullOrEmpty(options.Endpoint))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Endpoint)} configuration parameter for the Verifiable Credentials Service is required");
            }

            if (string.IsNullOrEmpty(options.VerifiableCredentialsServiceScope))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.VerifiableCredentialsServiceScope)} configuration parameter for the Verifiable Credentials Service is required");
            }

            if (string.IsNullOrEmpty(options.Instance))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Instance)} configuration parameter for the Verifiable Credentials Service is required");
            }

            if (string.IsNullOrEmpty(options.IssuanceType))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.IssuanceType)} configuration parameter for the Verifiable Credentials Service is required");
            }

            if (string.IsNullOrEmpty(options.TenantId))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.TenantId)} configuration parameter for the Verifiable Credentials Service is required");
            }

            if (string.IsNullOrEmpty(options.ClientId))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ClientId)} configuration parameter for the Verifiable Credentials Service is required");
            }


            if (string.IsNullOrEmpty(options.ClientSecret))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ClientSecret)} configuration parameter for the Verifiable Credentials Service is required");
            }

            if (string.IsNullOrEmpty(options.IssuerAuthority))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.IssuerAuthority)} configuration parameter for the Verifiable Credentials Service is required");
            }

            if (string.IsNullOrEmpty(options.VerifierAuthority))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.VerifierAuthority)} configuration parameter for the Verifiable Credentials Service is required");
            }

            if (string.IsNullOrEmpty(options.CredentialManifest))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.CredentialManifest)} configuration parameter for the Verifiable Credentials Service is required");
            }

            if (string.IsNullOrEmpty(options.IssuanceCallbackUrl))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.IssuanceCallbackUrl)} configuration parameter for the Verifiable Credentials Service is required");
            }

            if (string.IsNullOrEmpty(options.IssuanceCallbackApiKey))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.IssuanceCallbackApiKey)} configuration parameter for the Verifiable Credentials Service is required");
            }

            if (string.IsNullOrEmpty(options.IssuanceRegistrationClientName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.IssuanceRegistrationClientName)} configuration parameter for the Verifiable Credentials Service is required");
            }

            if (string.IsNullOrEmpty(options.IssuanceRegistrationPurpose))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.IssuanceRegistrationPurpose)} configuration parameter for the Verifiable Credentials Service is required");
            }

            if (string.IsNullOrEmpty(options.PresentationCallbackUrl))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.PresentationCallbackUrl)} configuration parameter for the Verifiable Credentials Service is required");
            }

            if (string.IsNullOrEmpty(options.PresentationCallbackApiKey))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.PresentationCallbackApiKey)} configuration parameter for the Verifiable Credentials Service is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
