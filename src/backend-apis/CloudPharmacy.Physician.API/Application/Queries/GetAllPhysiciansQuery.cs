using AutoMapper;
using CloudPharmacy.Common.CommonResponse;
using CloudPharmacy.Physician.API.Application.DTO;
using CloudPharmacy.Physician.API.Application.Repositories;
using CloudPharmacy.Physician.API.Infrastructure.Services.Identity;
using CloudPharmacy.Physician.API.Infrastructure.Services.Storage;
using CloudPharmacy.Physician.Application.Model;
using MediatR;

namespace CloudPharmacy.Physician.API.Application.Queries
{
    internal class GetAllPhysiciansQuery : IRequest<OperationResponse<IList<PhysicianProfileDTO>>>
    {
    }

    internal class GetAllPhysiciansQueryHandler : IRequestHandler<GetAllPhysiciansQuery, OperationResponse<IList<PhysicianProfileDTO>>>
    {
        private readonly IPhysicianRepository _physicianRepository;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;

        public GetAllPhysiciansQueryHandler(IPhysicianRepository physicianRepository,
                           IIdentityService identityService,
                           IStorageService storageService,
                            IMapper mapper)
        {
            _physicianRepository = physicianRepository ?? throw new ArgumentNullException(nameof(physicianRepository));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OperationResponse<IList<PhysicianProfileDTO>>> Handle(GetAllPhysiciansQuery request, CancellationToken cancellationToken)
        {
            var allPhysicians = await _physicianRepository.GetAllProfilesAsync();
            IList<PhysicianProfileDTO> allPhysiciansDTOs = _mapper.Map<IList<PhysicianProfile>,
                                                                      IList<PhysicianProfileDTO>>(allPhysicians);

            foreach (var physicianProfileDTO in allPhysiciansDTOs)
            {
                if (!string.IsNullOrEmpty(physicianProfileDTO.PhotoUrl))
                {
                    string filename = Path.GetFileName(new Uri(physicianProfileDTO.PhotoUrl).AbsolutePath);
                    var sasToken = _storageService.GenerateSasTokenForBlob(physicianProfileDTO.Id, filename);
                    var fileUrlWithSas = $"{physicianProfileDTO.PhotoUrl}?{sasToken}";
                    physicianProfileDTO.PhotoUrl = fileUrlWithSas;
                }
            }


            return new OperationResponse<IList<PhysicianProfileDTO>>()
            {
                Result = allPhysiciansDTOs
            };
        }
    }
}
