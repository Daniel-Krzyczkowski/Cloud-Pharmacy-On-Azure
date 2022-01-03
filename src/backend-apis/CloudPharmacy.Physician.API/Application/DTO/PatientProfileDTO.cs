namespace CloudPharmacy.Physician.API.Application.DTO
{
    public class PatientProfileDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string NationalHealthcareId { get; set; }
    }
}
