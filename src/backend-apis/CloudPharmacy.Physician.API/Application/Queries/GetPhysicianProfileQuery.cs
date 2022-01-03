using AutoMapper;
using CloudPharmacy.Common.CommonResponse;
using CloudPharmacy.Physician.API.Application.DTO;
using CloudPharmacy.Physician.API.Application.ErrorHandling;
using CloudPharmacy.Physician.API.Application.Repositories;
using CloudPharmacy.Physician.API.Infrastructure.Services.Identity;
using CloudPharmacy.Physician.API.Infrastructure.Services.Storage;
using MediatR;

namespace CloudPharmacy.Physician.API.Application.Queries
{
    internal class GetPhysicianProfileQuery : IRequest<OperationResponse<PhysicianProfileDTO>>
    {
    }

    internal class GetPhysicianProfileQueryHandler : IRequestHandler<GetPhysicianProfileQuery, OperationResponse<PhysicianProfileDTO>>
    {
        private readonly IPhysicianRepository _physicianRepository;
        private readonly IIdentityService _identityService;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;

        public GetPhysicianProfileQueryHandler(IPhysicianRepository physicianRepository,
                                   IIdentityService identityService,
                                   IStorageService storageService,
                                    IMapper mapper)
        {
            _physicianRepository = physicianRepository ?? throw new ArgumentNullException(nameof(physicianRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OperationResponse<PhysicianProfileDTO>> Handle(GetPhysicianProfileQuery request, CancellationToken cancellationToken)
        {
            var physicianId = _identityService.GetUserIdentity();
            var physicianProfile = await _physicianRepository.GetProfileAsync(physicianId);
            PhysicianProfileDTO physicianProfileDTO = new();
            if (physicianProfile == null)
            {
                physicianProfileDTO.FirstNameAndLastName = _identityService.GetUserFirstNameAndLastName();
            }

            else
            {
                physicianProfileDTO = _mapper.Map<PhysicianProfileDTO>(physicianProfile);
                if (!string.IsNullOrEmpty(physicianProfile.PhotoUrl))
                {
                    string filename = Path.GetFileName(new Uri(physicianProfile.PhotoUrl).AbsolutePath);
                    var sasToken = _storageService.GenerateSasTokenForBlob(physicianId, filename);
                    var fileUrlWithSas = $"{physicianProfile.PhotoUrl}?{sasToken}";
                    physicianProfileDTO.PhotoUrl = fileUrlWithSas;
                }
            }

            return new OperationResponse<PhysicianProfileDTO>()
            {
                Result = physicianProfileDTO
            };
        }
    }
}
