using AutoMapper;
using CloudPharmacy.Common.CommonResponse;
using CloudPharmacy.Patient.API.Application.DTO;
using CloudPharmacy.Patient.API.Application.ErrorHandling;
using CloudPharmacy.Patient.API.Application.Repositories;
using CloudPharmacy.Patient.API.Infrastructure.Services.Identity;
using MediatR;

namespace CloudPharmacy.Patient.API.Application.Queries
{
    internal class GetPatientProfileQuery : IRequest<OperationResponse<PatientProfileDTO>>
    {
    }

    internal class GetPatientProfileQueryHandler : IRequestHandler<GetPatientProfileQuery, OperationResponse<PatientProfileDTO>>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetPatientProfileQueryHandler(IPatientRepository patientRepository,
                                   IIdentityService identityService,
                                    IMapper mapper)
        {
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OperationResponse<PatientProfileDTO>> Handle(GetPatientProfileQuery request, CancellationToken cancellationToken)
        {
            var patientId = _identityService.GetUserIdentity();
            var patientProfile = await _patientRepository.GetProfileAsync(patientId);
            PatientProfileDTO patientProfileDTO = new();
            if (patientProfile == null)
            {
                patientProfileDTO.FirstNameAndLastName = _identityService.GetUserFirstNameAndLastName();
            }
            else
            {
                patientProfileDTO = _mapper.Map<PatientProfileDTO>(patientProfile);
            }

            return new OperationResponse<PatientProfileDTO>()
            {
                Result = patientProfileDTO
            };
        }
    }
}
