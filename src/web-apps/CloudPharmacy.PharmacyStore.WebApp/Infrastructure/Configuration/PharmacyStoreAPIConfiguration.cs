using Microsoft.Extensions.Options;

namespace CloudPharmacy.PharmacyStore.WebApp.Infrastructure.Configuration
{
    public interface IPharmacyStoreAPIConfiguration
    {
        string Url { get; set; }
        string Scope { get; set; }
    }

    internal class PharmacyStoreAPIConfiguration : IPharmacyStoreAPIConfiguration
    {
        public string Url { get; set; }
        public string Scope { get; set; }
    }

    internal class PharmacyStoreAPIConfigurationValidation : IValidateOptions<PharmacyStoreAPIConfiguration>
    {
        public ValidateOptionsResult Validate(string name, PharmacyStoreAPIConfiguration options)
        {
            if (string.IsNullOrEmpty(options.Url))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Url)} configuration parameter for the Pharmacy Store API is required");
            }

            if (string.IsNullOrEmpty(options.Scope))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Scope)} configuration parameter for the Pharmacy Store API is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
