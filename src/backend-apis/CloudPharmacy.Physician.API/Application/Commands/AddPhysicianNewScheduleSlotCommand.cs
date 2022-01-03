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
    internal class AddPhysicianNewScheduleSlotCommand : IRequest<OperationResponse>
    {
        public PhysicianFreeScheduleSlotDTO PhysicianFreeScheduleSlotDTO { get; set; }
    }

    internal class AddPhysicianNewScheduleSlotCommandHandler : IRequestHandler<AddPhysicianNewScheduleSlotCommand,
                                                                        OperationResponse>
    {
        private readonly IPhysicianScheduleSlotRepository _physicianScheduleSlotRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public AddPhysicianNewScheduleSlotCommandHandler(IPhysicianScheduleSlotRepository physicianScheduleSlotRepository,
                                   IIdentityService identityService,
                                   IMapper mapper)
        {
            _physicianScheduleSlotRepository = physicianScheduleSlotRepository ?? throw new ArgumentNullException(nameof(physicianScheduleSlotRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<OperationResponse> Handle(AddPhysicianNewScheduleSlotCommand request, CancellationToken cancellationToken)
        {
            var newScheduleSlotDTO = request.PhysicianFreeScheduleSlotDTO;

            if (newScheduleSlotDTO.SlotDateAndTime <= DateTimeOffset.Now)
            {
                return new OperationResponse()
                                .SetAsFailureResponse(OperationErrorDictionary.PhysicianScheduleSlot.WrongSlotTime());
            }

            var physicianId = _identityService.GetUserIdentity();

            var newScheduleSlot = new PhysicianScheduleSlot()
            {
                Id = Guid.NewGuid().ToString(),
                PhysicianId = physicianId,
                SlotDateAndTime = newScheduleSlotDTO.SlotDateAndTime
            };

            await _physicianScheduleSlotRepository.AddNewSlotToScheduleAsync(newScheduleSlot);

            return new OperationResponse();
        }
    }
}
