using Azure;
using Azure.Cosmos;
using CloudPharmacy.PharmacyStore.API.Application.Model;
using CloudPharmacy.PharmacyStore.API.Application.Repositories;
using CloudPharmacy.PharmacyStore.API.Infrastructure.Configuration;
using System.Net;

namespace CloudPharmacy.PharmacyStore.API.Infrastructure.Repositories
{
    public class MedicamentRepository : IMedicamentRepository
    {
        private readonly ILogger<MedicamentRepository> _logger;
        private readonly ICosmosDbConfiguration _cosmosDbConfiguration;
        private readonly CosmosClient _client;

        public MedicamentRepository(ILogger<MedicamentRepository> logger,
                      ICosmosDbConfiguration cosmosDbConfiguration,
                      CosmosClient client)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cosmosDbConfiguration = cosmosDbConfiguration
                    ?? throw new ArgumentNullException(nameof(cosmosDbConfiguration));

            _client = client
                    ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task AddMedicamentAsync(Medicament medicament)
        {
            try
            {
                CosmosContainer container = GetContainer();
                ItemResponse<Medicament> createResponse = await container.CreateItemAsync(medicament,
                                                                                           new PartitionKey(medicament.Id));
            }
            catch (CosmosException ex)
            {
                _logger.LogError($"New entity with ID: {medicament.Id} was not added successfully - error details: {ex.Message}");

                if (ex.Status != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }
            }
        }

        public async Task<IList<Medicament>> GetAllMedicamentsAsync()
        {
            try
            {
                CosmosContainer container = GetContainer();
                AsyncPageable<Medicament> queryResultSetIterator = container.GetItemQueryIterator<Medicament>();
                List<Medicament> entities = new List<Medicament>();

                await foreach (var entity in queryResultSetIterator)
                {
                    entities.Add(entity);
                }

                return entities;

            }
            catch (CosmosException ex)
            {
                _logger.LogError($"Entities were not retrieved successfully - error details: {ex.Message}");

                if (ex.Status != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }

                return null;
            }

            catch (Exception eee)
            {
                return null;
            }
        }

        protected CosmosContainer GetContainer()
        {
            var database = _client.GetDatabase(_cosmosDbConfiguration.DatabaseName);
            CosmosContainer container = database.GetContainer(_cosmosDbConfiguration.MedicamentContainerName);

            return container;
        }
    }
}
