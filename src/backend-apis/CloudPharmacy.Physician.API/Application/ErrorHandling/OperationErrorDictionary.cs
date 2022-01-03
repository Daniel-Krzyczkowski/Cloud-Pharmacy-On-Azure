using CloudPharmacy.Common.CommonResponse;

namespace CloudPharmacy.Physician.API.Application.ErrorHandling
{
    public static class OperationErrorDictionary
    {
        public static class PhysicianScheduleSlot
        {
            public static OperationError WrongSlotTime() =>
             new OperationError("Please provide correct date and time for the new schedule slot.");
        }

        public static class PrescriptionGeneration
        {
            public static OperationError PatientIdEmpty() =>
             new OperationError("Please provide correct ID of the patient");

            public static OperationError PatientNameEmpty() =>
             new OperationError("Please provide patient name");
        }
    }
}
