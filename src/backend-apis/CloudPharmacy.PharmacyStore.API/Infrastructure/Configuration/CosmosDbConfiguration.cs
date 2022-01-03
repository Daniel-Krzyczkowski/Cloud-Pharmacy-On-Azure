using Microsoft.Extensions.Options;

namespace CloudPharmacy.PharmacyStore.API.Infrastructure.Configuration
{
    public interface ICosmosDbConfiguration
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        public string MedicamentContainerName { get; set; }
        public string MedicamentContainerPartitionKeyPath { get; set; }
    }

    public class CosmosDbConfiguration : ICosmosDbConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string MedicamentContainerName { get; set; }
        public string MedicamentContainerPartitionKeyPath { get; set; }
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

            if (string.IsNullOrEmpty(options.MedicamentContainerName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.MedicamentContainerName)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrEmpty(options.MedicamentContainerPartitionKeyPath))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.MedicamentContainerPartitionKeyPath)} configuration parameter for the Azure Cosmos DB is required");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
