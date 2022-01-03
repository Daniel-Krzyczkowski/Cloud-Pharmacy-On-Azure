using Azure.Storage.Blobs;
using CloudPharmacy.Physician.API.Infrastructure.Configuration;
using CloudPharmacy.Physician.API.Infrastructure.Services.Storage;

namespace CloudPharmacy.Physician.API.Core.DependecyInjection
{
    public static class BlobStorageServiceCollectionExtensions
    {
        public static IServiceCollection AddStorageServices(this IServiceCollection services)
        {
            services.AddSingleton(implementationFactory =>
            {
                var storageConfiguration = implementationFactory.GetRequiredService<IBlobStorageConfiguration>();
                return new BlobServiceClient(storageConfiguration.ConnectionString);
            });

            services.AddTransient<IStorageService, StorageService>();

            return services;
        }
    }
}
