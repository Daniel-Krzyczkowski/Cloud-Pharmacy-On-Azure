using CloudPharmacy.Common.CommonResponse;

namespace CloudPharmacy.Patient.API.Application.ErrorHandling
{
    public static class OperationErrorDictionary
    {
        public static class PatientProfile
        {
            public static OperationError ProfileNotFound() =>
               new OperationError("There is no profile found.");
        }
    }
}
