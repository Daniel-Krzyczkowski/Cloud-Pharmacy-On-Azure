using Microsoft.Extensions.Options;

namespace CloudPharmacy.Patient.WebApp.Infrastructure.Configuration
{
    public interface IPhysicianAPIConfiguration
    {
        string Url { get; set; }
    }

    public class PhysicianAPIConfiguration : IPhysicianAPIConfiguration
    {
        public string Url { get; set; }
    }

    internal class PhysicianAPIConfigurationValidation : IValidateOptions<PhysicianAPIConfiguration>
    {
        public ValidateOptionsResult Validate(string name, PhysicianAPIConfiguration options)
        {
            if (string.IsNullOrEmpty(options.Url))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Url)} configuration parameter for the Physician API is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
