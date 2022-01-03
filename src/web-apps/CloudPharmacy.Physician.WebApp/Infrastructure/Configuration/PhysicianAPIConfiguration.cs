using Microsoft.Extensions.Options;

namespace CloudPharmacy.Physician.WebApp.Infrastructure.Configuration
{
    public interface IPhysicianAPIConfiguration
    {
        string Url { get; set; }
        string Scope { get; set; }
    }

    internal class PhysicianAPIConfiguration : IPhysicianAPIConfiguration
    {
        public string Url { get; set; }
        public string Scope { get; set; }
    }

    internal class PhysicianAPIConfigurationValidation : IValidateOptions<PhysicianAPIConfiguration>
    {
        public ValidateOptionsResult Validate(string name, PhysicianAPIConfiguration options)
        {
            if (string.IsNullOrEmpty(options.Url))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Url)} configuration parameter for the Physician API is required");
            }

            if (string.IsNullOrEmpty(options.Scope))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Scope)} configuration parameter for the Physician API is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
