using Azure.Cosmos;
using CloudPharmacy.Patient.API.Application.Repositories;
using CloudPharmacy.Patient.API.Infrastructure.Configuration;
using CloudPharmacy.Patient.API.Infrastructure.Repositories;

namespace CloudPharmacy.Patient.API.Core.DependencyInjection
{
    internal static class CosmosDbServiceCollectionExtensions
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
                   cosmoDbConfiguration.PatientContainerName,
                   cosmoDbConfiguration.PatientContainerPartitionKeyPath, 400)
                   .GetAwaiter()
                   .GetResult();

                database.CreateContainerIfNotExistsAsync(
                    cosmoDbConfiguration.PrescriptionContainerName,
                    cosmoDbConfiguration.PrescriptionContainerPartitionKeyPath, 400)
                    .GetAwaiter()
                    .GetResult();

                return cosmosClient;
            });

            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<IPrescriptionRepository, PrescriptionRepository>();

            return services;
        }
    }
}
