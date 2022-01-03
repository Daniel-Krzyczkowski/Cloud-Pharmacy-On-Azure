using Azure.Cosmos;
using CloudPharmacy.Physician.API.Application.Repositories;
using CloudPharmacy.Physician.API.Infrastructure.Configuration;
using CloudPharmacy.Physician.API.Infrastructure.Repositories;

namespace CloudPharmacy.Physician.API.Core.DependecyInjection
{
    public static class CosmosDbServiceCollectionExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {

            services.AddSingleton(implementationFactory =>
            {
                var cosmoDbConfiguration = implementationFactory.GetRequiredService<ICosmosDbConfiguration>();
                CosmosClient cosmosClient = new CosmosClient(cosmoDbConfiguration.ConnectionString);
                CosmosDatabase database = cosmosClient.CreateDatabaseIfNotExistsAsync(cosmoDbConfiguration.DatabaseName)
                                            .GetAwaiter()
                                            .GetResult();

                database.CreateContainerIfNotExistsAsync(
                   cosmoDbConfiguration.PhysicianContainerName,
                   cosmoDbConfiguration.PhysicianContainerPartitionKeyPath, 400)
                   .GetAwaiter()
                   .GetResult();

                database.CreateContainerIfNotExistsAsync(
                    cosmoDbConfiguration.PhysicianScheduleContainerName,
                    cosmoDbConfiguration.PhysicianScheduleContainerPartitionKeyPath, 400)
                    .GetAwaiter()
                    .GetResult();

                database.CreateContainerIfNotExistsAsync(
                    cosmoDbConfiguration.PrescriptionContainerName,
                    cosmoDbConfiguration.PrescriptionContainerPartitionKeyPath, 400)
                    .GetAwaiter()
                    .GetResult();

                return cosmosClient;
            });

            services.AddTransient<IPhysicianRepository, PhysicianRepository>();
            services.AddTransient<IPhysicianScheduleSlotRepository, PhysicianScheduleSlotRepository>();
            services.AddTransient<IPrescriptionRepository, PrescriptionRepository>();

            return services;
        }
    }
}
