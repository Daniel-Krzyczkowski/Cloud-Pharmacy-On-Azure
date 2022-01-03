using Microsoft.Extensions.Options;

namespace CloudPharmacy.Patient.WebApp.Infrastructure.Configuration
{
    public interface IPatientAPIConfiguration
    {
        string Url { get; set; }
        string Scope { get; set; }
    }

    internal class PatientAPIConfiguration : IPatientAPIConfiguration
    {
        public string Url { get; set; }
        public string Scope { get; set; }
    }

    internal class PatientAPIConfigurationValidation : IValidateOptions<PatientAPIConfiguration>
    {
        public ValidateOptionsResult Validate(string name, PatientAPIConfiguration options)
        {
            if (string.IsNullOrEmpty(options.Url))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Url)} configuration parameter for the Patient API is required");
            }

            if (string.IsNullOrEmpty(options.Scope))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.Scope)} configuration parameter for the Patient API is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
