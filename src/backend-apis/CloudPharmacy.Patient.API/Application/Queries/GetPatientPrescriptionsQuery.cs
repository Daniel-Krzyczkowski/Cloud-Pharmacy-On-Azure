using AutoMapper;
using CloudPharmacy.Common.CommonResponse;
using CloudPharmacy.Patient.API.Application.DTO;
using CloudPharmacy.Patient.API.Application.Model;
using CloudPharmacy.Patient.API.Application.Repositories;
using CloudPharmacy.Patient.API.Infrastructure.Services.Identity;
using MediatR;

namespace CloudPharmacy.Patient.API.Application.Queries
{
    internal class GetPatientPrescriptionsQuery : IRequest<OperationResponse<IList<PatientPrescriptionDTO>>>
    {
    }

    internal class GetPatientPrescriptionsQueryHandler : IRequestHandler<GetPatientPrescriptionsQuery,
                                                                OperationResponse<IList<PatientPrescriptionDTO>>>
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetPatientPrescriptionsQueryHandler(IPrescriptionRepository prescriptionRepository,
                           IIdentityService identityService,
                           IMapper mapper)
        {
            _prescriptionRepository = prescriptionRepository ?? throw new ArgumentNullException(nameof(prescriptionRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OperationResponse<IList<PatientPrescriptionDTO>>> Handle(GetPatientPrescriptionsQuery request, CancellationToken cancellationToken)
        {
            var patientId = _identityService.GetUserIdentity();

            IList<Prescription> patientPrescriptions = await _prescriptionRepository.GetAllPrescriptionsForPatientAsync(patientId);
            var patientPrescriptionsDTOs = _mapper.Map<List<PatientPrescriptionDTO>>(patientPrescriptions);


            return new OperationResponse<IList<PatientPrescriptionDTO>>()
            {
                Result = patientPrescriptionsDTOs
            };
        }
    }
}
