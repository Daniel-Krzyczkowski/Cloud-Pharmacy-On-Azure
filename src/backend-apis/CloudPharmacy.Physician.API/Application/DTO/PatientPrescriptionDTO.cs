namespace CloudPharmacy.Physician.API.Application.DTO
{
    public class PatientPrescriptionDTO : NewPatientPrescriptionDTO
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string PatientName { get; set; }
        public string PatientId { get; set; }
        public string RecommendedMedicaments { get; set; }
        public string IssuedBy { get; set; }
        public DateTime IssuanceDate { get; set; }
        public DateTime ExpirationDate { get; set; }

    }
}
