using Azure.Cosmos;
using CloudPharmacy.Physician.API.Application.Repositories;
using CloudPharmacy.Physician.API.Infrastructure.Configuration;
using CloudPharmacy.Physician.Application.Model;
using System.Net;

namespace CloudPharmacy.Physician.API.Infrastructure.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly ILogger<PrescriptionRepository> _logger;
        private readonly ICosmosDbConfiguration _cosmosDbConfiguration;
        private readonly CosmosClient _client;

        public PrescriptionRepository(ILogger<PrescriptionRepository> logger,
                              ICosmosDbConfiguration cosmosDbConfiguration,
                              CosmosClient client)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cosmosDbConfiguration = cosmosDbConfiguration
                    ?? throw new ArgumentNullException(nameof(cosmosDbConfiguration));

            _client = client
                    ?? throw new ArgumentNullException(nameof(client));
        }
        public async Task AddPrescriptionForPatientAsync(Prescription prescription)
        {
            try
            {
                CosmosContainer container = GetContainer();
                ItemResponse<Prescription> createResponse = await container.CreateItemAsync(prescription,
                                                                                           new PartitionKey(prescription.PatientId));
            }
            catch (CosmosException ex)
            {
                _logger.LogError($"New entity with ID: {prescription.Id} was not added successfully - error details: {ex.Message}");

                if (ex.Status != (int)HttpStatusCode.NotFound)
                {
                    throw;
                }
            }
        }

        public async Task<Prescription> GetPrescriptionForPatientAsync(string prescriptionId, string patientId)
        {
            try
            {
                CosmosContainer container = GetContainer();

                ItemResponse<Prescription> entityResult = await container.ReadItemAsync<Prescription>(prescriptionId,
                                                                                                    new PartitionKey(patientId));
                return entityResult.Value;
            }
            catch (CosmosException ex)
            {
                _logger.LogError($"Entity with ID: {prescriptionId} was not retrieved successfully - error details: {ex.Message}");

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
            CosmosContainer container = database.GetContainer(_cosmosDbConfiguration.PrescriptionContainerName);

            return container;
        }
    }
}
