using Azure;
using Azure.Cosmos;
using CloudPharmacy.Physician.API.Application.Repositories;
using CloudPharmacy.Physician.API.Infrastructure.Configuration;
using CloudPharmacy.Physician.Application.Model;
using System.Net;

namespace CloudPharmacy.Physician.API.Infrastructure.Repositories
{
    public class PhysicianScheduleSlotRepository : IPhysicianScheduleSlotRepository
    {
        private readonly ILogger<PhysicianScheduleSlotRepository> _logger;
        private readonly ICosmosDbConfiguration _cosmosDbConfiguration;
        private readonly CosmosClient _client;

        public PhysicianScheduleSlotRepository(ILogger<PhysicianScheduleSlotRepository> logger,
                      ICosmosDbConfiguration cosmosDbConfiguration,
                      CosmosClient client)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cosmosDbConfiguration = cosmosDbConfiguration
                    ?? throw new ArgumentNullException(nameof(cosmosDbConfiguration));

            _client = client
                    ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task AddNewSlotToScheduleAsync(PhysicianScheduleSlot physicianScheduleSlot)
        {
            try
            {
                CosmosContainer container = GetContainer();
                ItemResponse<PhysicianScheduleSlot> createResponse = await container.CreateItemAsync(physicianScheduleSlot,
                                                                                           new PartitionKey(physicianScheduleSlot.PhysicianId));
            }
            catch (CosmosException ex)
            {
                _logger.LogError($"New entity with ID: {physicianScheduleSlot.Id} was not added successfully - error details: {ex.Message}");

                if (ex.Status != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }
            }
        }

        public async Task<IList<PhysicianScheduleSlot>> GetFreeSlotsFromScheduleAsync(string physicianId)
        {
            IList<PhysicianScheduleSlot> entities = new List<PhysicianScheduleSlot>();
            try
            {
                CosmosContainer container = GetContainer();
                QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM c where c.physicianId = @physicianId AND IS_NULL(c.patient)")
                                                      .WithParameter("@physicianId", physicianId);
                AsyncPageable<PhysicianScheduleSlot> queryResultSetIterator = container.GetItemQueryIterator<PhysicianScheduleSlot>(queryDefinition);

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

                return entities;
            }
        }

        public async Task<IList<PhysicianScheduleSlot>> GetReservedSlotsFromScheduleAsync(string physicianId)
        {
            IList<PhysicianScheduleSlot> entities = new List<PhysicianScheduleSlot>();
            try
            {
                CosmosContainer container = GetContainer();
                QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM c where c.physicianId = @physicianId AND c.patient != null")
                                                      .WithParameter("@physicianId", physicianId);
                AsyncPageable<PhysicianScheduleSlot> queryResultSetIterator = container.GetItemQueryIterator<PhysicianScheduleSlot>(queryDefinition);

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

                return entities;
            }
        }

        public async Task DeleteScheduleSlotAsync(string scheduleSlotId, string physicianId)
        {
            try
            {
                CosmosContainer container = GetContainer();

                await container.DeleteItemAsync<PhysicianScheduleSlot>(scheduleSlotId, new PartitionKey(physicianId));
            }
            catch (CosmosException ex)
            {
                _logger.LogError($"Entity with ID: {scheduleSlotId} was not removed successfully - error details: {ex.Message}");

                if (ex.Status != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }
            }
        }

        public async Task<IList<PhysicianScheduleSlot>> GetAllScheduleSlotsAsync(string physicianId)
        {
            IList<PhysicianScheduleSlot> entities = new List<PhysicianScheduleSlot>();
            try
            {
                CosmosContainer container = GetContainer();
                QueryDefinition queryDefinition = new QueryDefinition($"SELECT * FROM c where c.physicianId = @physicianId")
                                                      .WithParameter("@physicianId", physicianId);
                AsyncPageable<PhysicianScheduleSlot> queryResultSetIterator = container.GetItemQueryIterator<PhysicianScheduleSlot>(queryDefinition);

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

                return entities;
            }
        }

        protected CosmosContainer GetContainer()
        {
            var database = _client.GetDatabase(_cosmosDbConfiguration.DatabaseName);
            CosmosContainer container = database.GetContainer(_cosmosDbConfiguration.PhysicianScheduleContainerName);

            return container;
        }
    }
}
