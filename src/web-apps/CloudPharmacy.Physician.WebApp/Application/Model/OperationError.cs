namespace CloudPharmacy.Physician.WebApp.Application.Model
{
    internal record OperationError
    {
        public string Details { get; }

        public OperationError(string details) => (Details) = (details);
    }
}
