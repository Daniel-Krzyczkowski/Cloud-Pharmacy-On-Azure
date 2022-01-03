using AutoMapper;
using CloudPharmacy.Common.CommonResponse;
using CloudPharmacy.Patient.API.Application.DTO;
using CloudPharmacy.Patient.API.Application.Model;
using CloudPharmacy.Patient.API.Application.Repositories;
using CloudPharmacy.Patient.API.Infrastructure.Services.Identity;
using MediatR;

namespace CloudPharmacy.Patient.API.Application.Commands
{
    internal class UpdatePatientProfileCommand : IRequest<OperationResponse<PatientProfileDTO>>
    {
        public UpdatePatientProfileDTO UpdatePatientProfileDTO { get; set; }
    }

    internal class UpdatePatientProfileCommandHandler : IRequestHandler<UpdatePatientProfileCommand,
                                                                        OperationResponse<PatientProfileDTO>>
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public UpdatePatientProfileCommandHandler(IPatientRepository patientRepository,
                                   IIdentityService identityService,
                                   IMapper mapper)
        {
            _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OperationResponse<PatientProfileDTO>> Handle(UpdatePatientProfileCommand request, CancellationToken cancellationToken)
        {
            var updatePatientProfileDTO = request.UpdatePatientProfileDTO;
            var patientId = _identityService.GetUserIdentity();
            var patientName = _identityService.GetUserFirstNameAndLastName();
            var patientProfile = _mapper.Map<PatientProfile>(updatePatientProfileDTO);
            patientProfile.Id = patientId;
            patientProfile.FirstNameAndLastName = patientName;

            var currentPatientProfile = await _patientRepository.GetProfileAsync(patientId);
            if (currentPatientProfile == null)
            {
                await _patientRepository.CreateProfileAsync(patientProfile);
            }

            else
            {
                await _patientRepository.UpdateProfileAsync(patientProfile);
            }

            var patientProfileDTO = _mapper.Map<PatientProfileDTO>(patientProfile);

            return new OperationResponse<PatientProfileDTO>
            {
                Result = patientProfileDTO
            };
        }
    }
}
