using Microsoft.Extensions.Options;

namespace CloudPharmacy.Logging.Configuration
{
    public interface IApplicationInsightsConfiguration
    {
        string InstrumentationKey { get; set; }
    }

    public class ApplicationInsightsConfiguration : IApplicationInsightsConfiguration
    {
        public string InstrumentationKey { get; set; }
    }

    public class ApplicationInsightsConfigurationValidation : IValidateOptions<ApplicationInsightsConfiguration>
    {
        public ValidateOptionsResult Validate(string name, ApplicationInsightsConfiguration options)
        {
            if (string.IsNullOrEmpty(options.InstrumentationKey))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.InstrumentationKey)} configuration parameter for the Azure Application Insights is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
