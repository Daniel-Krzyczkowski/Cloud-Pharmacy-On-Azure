namespace CloudPharmacy.Physician.WebApp.Application.Model
{
    internal class PhysicianProfileUpdate
    {
        public string Specialization { get; set; }
        public Stream ProfilePictureFile { get; set; }
        public string ProfilePictureFileName { get; set; }
    }
}
