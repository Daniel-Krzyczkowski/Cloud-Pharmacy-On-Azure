namespace CloudPharmacy.Patient.WebApp.Application.Model
{
    internal class Prescription
    {
        public string Id { get; set; }
        public string PatientName { get; set; }
        public string RecommendedMedicaments { get; set; }
        public string IssuedBy { get; set; }
        public string PhysicianId { get; set; }
        public DateTime IssuanceDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Code { get; set; }
        public string PatientSSN { get; set; }
        public bool WasUsed { get; set; }
    }
}
