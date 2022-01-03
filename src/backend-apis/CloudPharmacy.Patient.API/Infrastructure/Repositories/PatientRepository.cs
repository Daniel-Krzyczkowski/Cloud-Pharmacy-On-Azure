using Azure.Cosmos;
using CloudPharmacy.Patient.API.Application.Model;
using CloudPharmacy.Patient.API.Application.Repositories;
using CloudPharmacy.Patient.API.Infrastructure.Configuration;
using System.Net;

namespace CloudPharmacy.Patient.API.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ILogger<PatientRepository> _logger;
        private readonly ICosmosDbConfiguration _cosmosDbConfiguration;
        private readonly CosmosClient _client;

        public PatientRepository(ILogger<PatientRepository> logger,
                      ICosmosDbConfiguration cosmosDbConfiguration,
                      CosmosClient client)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cosmosDbConfiguration = cosmosDbConfiguration
                    ?? throw new ArgumentNullException(nameof(cosmosDbConfiguration));

            _client = client
                    ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<PatientProfile> GetProfileAsync(string patientId)
        {
            try
            {
                CosmosContainer container = GetContainer();

                ItemResponse<PatientProfile> entityResult = await container.ReadItemAsync<PatientProfile>(patientId,
                                                                                                    new PartitionKey(patientId));
                return entityResult.Value;
            }
            catch (CosmosException ex)
            {
                _logger.LogError($"Entity with ID: {patientId} was not retrieved successfully - error details: {ex.Message}");

                if (ex.Status != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }

                return null;
            }
        }

        public async Task CreateProfileAsync(PatientProfile patient)
        {
            try
            {
                CosmosContainer container = GetContainer();
                ItemResponse<PatientProfile> createResponse = await container.CreateItemAsync(patient,
                                                                                           new PartitionKey(patient.Id));
            }
            catch (CosmosException ex)
            {
                _logger.LogError($"New entity with ID: {patient.Id} was not added successfully - error details: {ex.Message}");

                if (ex.Status != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }
            }
        }

        public async Task UpdateProfileAsync(PatientProfile patient)
        {
            try
            {
                CosmosContainer container = GetContainer();

                await container
                      .ReplaceItemAsync(patient, patient.Id, new PartitionKey(patient.Id));
            }
            catch (CosmosException ex)
            {
                _logger.LogError($"Entity with ID: {patient.Id} was not updated successfully - error details: {ex.Message}");

                if (ex.Status != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }
            }
        }

        protected CosmosContainer GetContainer()
        {
            var database = _client.GetDatabase(_cosmosDbConfiguration.DatabaseName);
            CosmosContainer container = database.GetContainer(_cosmosDbConfiguration.PatientContainerName);

            return container;
        }
    }
}
