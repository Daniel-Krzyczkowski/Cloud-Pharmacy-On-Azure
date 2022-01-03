using CloudPharmacy.PharmacyStore.API.Infrastructure.Services.Identity;
using Microsoft.Identity.Client;

namespace CloudPharmacy.PharmacyStore.API.Core.DependecyInjection
{
    internal static class VerifiableCredentialsServiceCollectionExtensions
    {
        public static IServiceCollection AddVerifiableCredentialsServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped(implementationFactory =>
            {
                var confidentialClient = ConfidentialClientApplicationBuilder.Create(config["AzureAdB2CConfiguration:ClientId"])
                    .WithClientSecret(config["AzureAdB2CConfiguration:ClientSecret"])
                    .WithAuthority(config["VerifiableCredentialsServiceConfiguration:Authority"])
                    .Build();

                return confidentialClient;
            });

            services.AddHttpClient<IVerifiableCredentialsService, VerifiableCredentialsService>();
            return services;
        }
    }
}
