using CloudPharmacy.Logging.Configuration;
using CloudPharmacy.Physician.API.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace CloudPharmacy.Physician.API.Core.DependecyInjection
{
    internal static class ConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<ApplicationInsightsConfiguration>(config.GetSection("ApplicationInsightsConfiguration"));
            services.AddSingleton<IValidateOptions<ApplicationInsightsConfiguration>, ApplicationInsightsConfigurationValidation>();
            var applicationInsightsConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<ApplicationInsightsConfiguration>>().Value;
            services.AddSingleton<IApplicationInsightsConfiguration>(applicationInsightsConfiguration);

            services.Configure<BlobStorageConfiguration>(config.GetSection("BlobStorageConfiguration"));
            services.AddSingleton<IValidateOptions<BlobStorageConfiguration>, StorageConfigurationValidation>();
            var blobStorageConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<BlobStorageConfiguration>>().Value;
            services.AddSingleton<IBlobStorageConfiguration>(blobStorageConfiguration);

            services.Configure<CosmosDbConfiguration>(config.GetSection("CosmosDbConfiguration"));
            services.AddSingleton<IValidateOptions<CosmosDbConfiguration>, CosmosDbConfigurationValidation>();
            var cosmosDbConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<CosmosDbConfiguration>>().Value;
            services.AddSingleton<ICosmosDbConfiguration>(cosmosDbConfiguration);

            return services;
        }
    }
}
