using AutoMapper;
using CloudPharmacy.Common.CommonResponse;
using CloudPharmacy.Physician.API.Application.DTO;
using CloudPharmacy.Physician.API.Application.ErrorHandling;
using CloudPharmacy.Physician.API.Application.Repositories;
using CloudPharmacy.Physician.API.Infrastructure.Services.Identity;
using CloudPharmacy.Physician.Application.Model;
using MediatR;

namespace CloudPharmacy.Physician.API.Application.Commands
{
    internal class AddNewPrescriptionForPatientCommand : IRequest<OperationResponse<PatientPrescriptionDTO>>
    {
        public NewPatientPrescriptionDTO NewPatientPrescriptionDTO { get; set; }
    }

    internal class AddNewPrescriptionForPatientCommandHandler : IRequestHandler<AddNewPrescriptionForPatientCommand,
                                                                        OperationResponse<PatientPrescriptionDTO>>
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public AddNewPrescriptionForPatientCommandHandler(IPrescriptionRepository prescriptionRepository,
                           IIdentityService identityService,
                           IMapper mapper)
        {
            _prescriptionRepository = prescriptionRepository ?? throw new ArgumentNullException(nameof(prescriptionRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OperationResponse<PatientPrescriptionDTO>> Handle(AddNewPrescriptionForPatientCommand request,
                                                                                                    CancellationToken cancellationToken)
        {
            var newPrescriptionDTO = request.NewPatientPrescriptionDTO;

            var physicianId = _identityService.GetUserIdentity();

            if (string.IsNullOrEmpty(newPrescriptionDTO.PatientId))
            {
                return new OperationResponse<PatientPrescriptionDTO>()
                               .SetAsFailureResponse(OperationErrorDictionary.PrescriptionGeneration.PatientIdEmpty());
            }

            if (string.IsNullOrEmpty(newPrescriptionDTO.PatientName))
            {
                return new OperationResponse<PatientPrescriptionDTO>()
                               .SetAsFailureResponse(OperationErrorDictionary.PrescriptionGeneration.PatientNameEmpty());
            }

            var newPrescription = _mapper.Map<Prescription>(newPrescriptionDTO);
            newPrescription.Id = Guid.NewGuid().ToString();
            newPrescription.IssuedBy = _identityService.GetUserFirstNameAndLastName();
            newPrescription.PhysicianId = physicianId;
            int min = 1000;
            int max = 9999;
            Random _rdm = new Random();
            newPrescription.Code = _rdm.Next(min, max).ToString();

            await _prescriptionRepository.AddPrescriptionForPatientAsync(newPrescription);

            var createdPatientPrescriptionDTO = _mapper.Map<PatientPrescriptionDTO>(newPrescription);

            return new OperationResponse<PatientPrescriptionDTO>()
            {
                Result = createdPatientPrescriptionDTO
            };
        }
    }
}
