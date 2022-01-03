using Microsoft.Extensions.Options;

namespace CloudPharmacy.Patient.API.Infrastructure.Configuration
{
    public interface ICosmosDbConfiguration
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string PatientContainerName { get; set; }
        string PatientContainerPartitionKeyPath { get; set; }
        string PrescriptionContainerName { get; set; }
        string PrescriptionContainerPartitionKeyPath { get; set; }
    }

    public class CosmosDbConfiguration : ICosmosDbConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string PatientContainerName { get; set; }
        public string PatientContainerPartitionKeyPath { get; set; }
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

            if (string.IsNullOrEmpty(options.PatientContainerName))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.PatientContainerName)} configuration parameter for the Azure Cosmos DB is required");
            }

            if (string.IsNullOrEmpty(options.PatientContainerPartitionKeyPath))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.PatientContainerPartitionKeyPath)} configuration parameter for the Azure Cosmos DB is required");
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

    public enum CosmosDbContainer
    {
        PatientContainer,
        PrescriptionContainer
    }
}
