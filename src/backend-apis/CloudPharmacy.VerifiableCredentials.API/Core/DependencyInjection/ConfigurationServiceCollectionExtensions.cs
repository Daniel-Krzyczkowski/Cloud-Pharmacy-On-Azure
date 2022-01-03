using CloudPharmacy.VerifiableCredentials.API.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace CloudPharmacy.VerifiableCredentials.API.Core.DependencyInjection
{
    internal static class ConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<VerifiableCredentialsConfiguration>(config.GetSection("VerifiableCredentialsConfiguration"));
            services.AddSingleton<IValidateOptions<VerifiableCredentialsConfiguration>, VerifiableCredentialsConfigurationValidation>();
            var verifiableCredentialsConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<VerifiableCredentialsConfiguration>>().Value;
            services.AddSingleton<IVerifiableCredentialsConfiguration>(verifiableCredentialsConfiguration);

            services.Configure<LiveNotificationsFuncAppConfiguration>(config.GetSection("LiveNotificationsFuncAppConfiguration"));
            services.AddSingleton<IValidateOptions<LiveNotificationsFuncAppConfiguration>, LiveNotificationsFuncAppConfigurationValidation>();
            var liveNotificationsFuncAppConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<LiveNotificationsFuncAppConfiguration>>().Value;
            services.AddSingleton<ILiveNotificationsFuncAppConfiguration>(liveNotificationsFuncAppConfiguration);

            return services;
        }
    }
}
