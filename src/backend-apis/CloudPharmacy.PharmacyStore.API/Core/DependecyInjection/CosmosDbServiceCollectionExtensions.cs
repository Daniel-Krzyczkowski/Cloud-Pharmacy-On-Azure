using Azure.Cosmos;
using CloudPharmacy.PharmacyStore.API.Application.Model;
using CloudPharmacy.PharmacyStore.API.Application.Repositories;
using CloudPharmacy.PharmacyStore.API.Infrastructure.Configuration;
using CloudPharmacy.PharmacyStore.API.Infrastructure.Repositories;

namespace CloudPharmacy.PharmacyStore.API.Core.DependecyInjection
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
                   cosmoDbConfiguration.MedicamentContainerName,
                   cosmoDbConfiguration.MedicamentContainerPartitionKeyPath, 400)
                   .GetAwaiter()
                   .GetResult();

                return cosmosClient;
            });

            services.AddTransient<IMedicamentRepository, MedicamentRepository>();

            return services;
        }

        // Only for seed:
        private static void SeedData(IServiceCollection services)
        {
            var medicamentsRepository = services.BuildServiceProvider().GetRequiredService<IMedicamentRepository>();
            var medicamentsList = new List<Medicament>()
            {
                new Medicament()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Aspirin",
                    PictureUrl = "https://stcloudpharmacydev.blob.core.windows.net/medicaments-photos/aspirin-image.png",
                    Price = 15,
                    Producer = "Bayer"
                },
                new Medicament()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Dasen",
                    PictureUrl = "https://stcloudpharmacydev.blob.core.windows.net/medicaments-photos/dasen-image.png",
                    Price = 26.50M,
                    Producer = "Synthetic"
                },
                new Medicament()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Nabucox",
                    PictureUrl = "https://stcloudpharmacydev.blob.core.windows.net/medicaments-photos/nabucox-image.png",
                    Price = 45.60M,
                    Producer = "Expanscience"
                },
                new Medicament()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Efferalgan",
                    PictureUrl = "https://stcloudpharmacydev.blob.core.windows.net/medicaments-photos/efferalgan-image.png",
                    Price = 35,
                    Producer = "UPSA"
                },
                new Medicament()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "norethistrone",
                    PictureUrl = "https://stcloudpharmacydev.blob.core.windows.net/medicaments-photos/norethistrone-image.png",
                    Price = 11,
                    Producer = "Wockhardt"
                }
            };

            foreach (var medicament in medicamentsList)
            {
                medicamentsRepository.AddMedicamentAsync(medicament)
                                           .GetAwaiter()
                                           .GetResult();
            }
        }
    }
}
