using Azure;
using Azure.Cosmos;
using CloudPharmacy.Physician.API.Application.Repositories;
using CloudPharmacy.Physician.API.Infrastructure.Configuration;
using CloudPharmacy.Physician.Application.Model;
using System.Net;

namespace CloudPharmacy.Physician.API.Infrastructure.Repositories
{
    public class PhysicianRepository : IPhysicianRepository
    {
        private readonly ILogger<PhysicianRepository> _logger;
        private readonly ICosmosDbConfiguration _cosmosDbConfiguration;
        private readonly CosmosClient _client;

        public PhysicianRepository(ILogger<PhysicianRepository> logger,
                              ICosmosDbConfiguration cosmosDbConfiguration,
                              CosmosClient client)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cosmosDbConfiguration = cosmosDbConfiguration
                    ?? throw new ArgumentNullException(nameof(cosmosDbConfiguration));

            _client = client
                    ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<PhysicianProfile> GetProfileAsync(string physicianId)
        {
            try
            {
                CosmosContainer container = GetContainer();

                ItemResponse<PhysicianProfile> entityResult = await container.ReadItemAsync<PhysicianProfile>(physicianId,
                                                                                                    new PartitionKey(physicianId));
                return entityResult.Value;
            }
            catch (CosmosException ex)
            {
                _logger.LogError($"Entity with ID: {physicianId} was not retrieved successfully - error details: {ex.Message}");

                if (ex.Status != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }

                return null;
            }
        }

        public async Task CreateProfileAsync(PhysicianProfile physicianProfile)
        {
            try
            {
                CosmosContainer container = GetContainer();
                ItemResponse<PhysicianProfile> createResponse = await container.CreateItemAsync(physicianProfile,
                                                                                           new PartitionKey(physicianProfile.Id));
            }
            catch (CosmosException ex)
            {
                _logger.LogError($"New entity with ID: {physicianProfile.Id} was not added successfully - error details: {ex.Message}");

                if (ex.Status != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }
            }
        }

        public async Task UpdateProfileAsync(PhysicianProfile physician)
        {
            try
            {
                CosmosContainer container = GetContainer();

                ItemResponse<PhysicianProfile> entityResult = await container
                                                           .ReadItemAsync<PhysicianProfile>(physician.Id, new PartitionKey(physician.Id));

                if (entityResult != null)
                {
                    await container
                          .ReplaceItemAsync(physician, physician.Id, new PartitionKey(physician.Id));
                }
            }
            catch (CosmosException ex)
            {
                _logger.LogError($"Entity with ID: {physician.Id} was not updated successfully - error details: {ex.Message}");

                if (ex.Status != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }
            }
        }

        public async Task<IList<PhysicianProfile>> GetAllProfilesAsync()
        {
            try
            {
                CosmosContainer container = GetContainer();
                QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM c");
                AsyncPageable<PhysicianProfile> queryResultSetIterator = container.GetItemQueryIterator<PhysicianProfile>(queryDefinition);
                List<PhysicianProfile> entities = new List<PhysicianProfile>();

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
        }

        protected CosmosContainer GetContainer()
        {
            var database = _client.GetDatabase(_cosmosDbConfiguration.DatabaseName);
            CosmosContainer container = database.GetContainer(_cosmosDbConfiguration.PhysicianContainerName);

            return container;
        }
    }
}
