using AutoMapper;
using CloudPharmacy.Common.CommonResponse;
using CloudPharmacy.Physician.API.Application.DTO;
using CloudPharmacy.Physician.API.Application.Repositories;
using CloudPharmacy.Physician.API.Infrastructure.Services.Identity;
using CloudPharmacy.Physician.API.Infrastructure.Services.Storage;
using CloudPharmacy.Physician.Application.Model;
using MediatR;

namespace CloudPharmacy.Physician.API.Application.Commands
{
    internal class UpdatePhysicianProfileCommand : IRequest<OperationResponse<PhysicianProfileDTO>>
    {
        public UpdatePhysicianProfileDTO UpdatePhysicianProfileDTO { get; set; }
    }

    internal class UploadFileCommandHandler : IRequestHandler<UpdatePhysicianProfileCommand,
                                                                        OperationResponse<PhysicianProfileDTO>>
    {
        private readonly IPhysicianRepository _physicianRepository;
        private readonly IIdentityService _identityService;
        private readonly IStorageService _storageService;
        private readonly IMapper _mapper;

        public UploadFileCommandHandler(IPhysicianRepository physicianRepository,
                                   IIdentityService identityService,
                                   IStorageService storageService,
                                   IMapper mapper)
        {
            _physicianRepository = physicianRepository ?? throw new ArgumentNullException(nameof(physicianRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OperationResponse<PhysicianProfileDTO>> Handle(UpdatePhysicianProfileCommand request, CancellationToken cancellationToken)
        {
            var updatePhysicianProfileDTO = request.UpdatePhysicianProfileDTO;
            var fileToUpload = updatePhysicianProfileDTO.PhotoFile;
            var physicianId = _identityService.GetUserIdentity();

            string photoUrl = string.Empty;
            string photoUrlWithSas = string.Empty;

            if (fileToUpload != null)
            {
                photoUrl = await UploadPhotoFileAsync(fileToUpload, physicianId);
                photoUrlWithSas = await GetPhotoFileUrlWithSasAsync(fileToUpload, physicianId);
            }

            var physicianProfile = _mapper.Map<PhysicianProfile>(updatePhysicianProfileDTO);
            physicianProfile.PhotoUrl = photoUrl;
            physicianProfile.Id = physicianId;
            physicianProfile.FirstNameAndLastName = _identityService.GetUserFirstNameAndLastName();

            var currentPhysicianProfile = await _physicianRepository.GetProfileAsync(physicianId);
            if (currentPhysicianProfile == null)
            {
                await _physicianRepository.CreateProfileAsync(physicianProfile);
            }

            else
            {
                await _physicianRepository.UpdateProfileAsync(physicianProfile);
            }

            var physicianProfileDTO = _mapper.Map<PhysicianProfileDTO>(physicianProfile);

            physicianProfileDTO.PhotoUrl = photoUrlWithSas;

            return new OperationResponse<PhysicianProfileDTO>
            {
                Result = physicianProfileDTO
            };
        }

        private async Task<string> UploadPhotoFileAsync(IFormFile photoFile, string physicianId)
        {
            await _storageService.DeleteBlobIfExistsAsync(photoFile.FileName, physicianId);
            await _storageService.UploadBlobAsync(photoFile.OpenReadStream(), photoFile.FileName, physicianId);
            var fileUrl = await _storageService.GetBlobUrlAsync(photoFile.FileName, physicianId);
            return fileUrl;
        }

        private async Task<string> GetPhotoFileUrlWithSasAsync(IFormFile photoFile, string physicianId)
        {
            var fileUrl = await _storageService.GetBlobUrlAsync(photoFile.FileName, physicianId);
            var sasToken = _storageService.GenerateSasTokenForBlob(physicianId, photoFile.FileName);
            var fileUrlWithSas = $"{fileUrl}?{sasToken}";
            return fileUrlWithSas;
        }
    }
}
