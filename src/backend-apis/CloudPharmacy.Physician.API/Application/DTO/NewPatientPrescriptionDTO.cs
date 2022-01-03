namespace CloudPharmacy.Physician.API.Application.DTO
{
    public class NewPatientPrescriptionDTO
    {
        public string PatientName { get; set; }
        public string PatientId { get; set; }
        public string RecommendedMedicaments { get; set; }
        public DateTime IssuanceDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
