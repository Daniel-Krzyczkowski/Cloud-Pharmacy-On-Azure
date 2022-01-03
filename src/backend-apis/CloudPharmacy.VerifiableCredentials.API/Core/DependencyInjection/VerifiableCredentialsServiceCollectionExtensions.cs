using CloudPharmacy.VerifiableCredentials.API.Infrastructure.Services;

namespace CloudPharmacy.VerifiableCredentials.API.Core.DependencyInjection
{
    internal static class VerifiableCredentialsServiceCollectionExtensions
    {
        public static IServiceCollection AddVerifiableCredentialsServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<IVerifiableCredentialsManagementService, VerifiableCredentialsManagementService>();
            services.AddScoped<IVerifiableCredentialStatusNotificationService, VerifiableCredentialStatusNotificationService>();

            return services;
        }
    }
}
