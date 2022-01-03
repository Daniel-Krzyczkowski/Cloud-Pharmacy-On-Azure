using Microsoft.Extensions.Options;

namespace CloudPharmacy.Physician.API.Infrastructure.Configuration
{
    public interface ICosmosDbConfiguration
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string PhysicianContainerName { get; set; }
        string PhysicianContainerPartitionKeyPath { get; set; }
        string PhysicianScheduleContainerName { get; set; }
        string PhysicianScheduleContainerPartitionKeyPath { get; set; }
        string PrescriptionContainerName { get; set; }
        string PrescriptionContainerPartitionKeyPath { get; set; }
    }
    internal class CosmosDbConfiguration : ICosmosDbConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string PhysicianContainerName { get; set; }
        public string PhysicianContainerPartitionKeyPath { get; set; }
        public string PhysicianScheduleContainerName { get; set; }
        public string PhysicianScheduleContainerPartitionKeyPath { get; set; }
        public string PrescriptionContainerName { get; set; }
        public string PrescriptionContainerPartitionKeyPath { get; set; }
    }

    internal class CosmosDbConfigurationValidation : IValidateOptions<CosmosDbConfiguration>
    {
        public ValidateOptionsResult Validate(string name, CosmosDbConfiguration options)
        {
            if (string.IsNullOrEmpty(options.ConnectionString))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ConnectionString)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrEmpty(options.DatabaseName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.DatabaseName)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrEmpty(options.PhysicianContainerName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.PhysicianContainerName)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrEmpty(options.PhysicianContainerPartitionKeyPath))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.PhysicianContainerPartitionKeyPath)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrEmpty(options.PhysicianScheduleContainerName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.PhysicianScheduleContainerName)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrEmpty(options.PhysicianScheduleContainerPartitionKeyPath))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.PhysicianScheduleContainerPartitionKeyPath)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrEmpty(options.PrescriptionContainerName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.PrescriptionContainerName)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrEmpty(options.PrescriptionContainerPartitionKeyPath))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.PrescriptionContainerPartitionKeyPath)} configuration parameter for the Azure Cosmos DB is required");
            }


            return ValidateOptionsResult.Success;
        }
    }
}
