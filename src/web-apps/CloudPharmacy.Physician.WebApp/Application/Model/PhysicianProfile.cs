namespace CloudPharmacy.Physician.WebApp.Application.Model
{
    internal class PhysicianProfile
    {
        public string FirstNameAndLastName { get; set; }
        public string Specialization { get; set; }
        public bool HasProfilePublished { get; set; }
        public string PhotoUrl { get; set; }

    }
}
