using CloudPharmacy.Common.CommonResponse;
using CloudPharmacy.Physician.API.Application.Repositories;
using CloudPharmacy.Physician.API.Infrastructure.Services.Identity;
using MediatR;

namespace CloudPharmacy.Physician.API.Application.Commands
{
    internal class RemoveScheduleSlotCommand : IRequest<OperationResponse>
    {
        public string ScheduleSlotId { get; set; }
    }

    internal class RemoveScheduleSlotCommandHandler : IRequestHandler<RemoveScheduleSlotCommand,
                                                                        OperationResponse>
    {
        private readonly IPhysicianScheduleSlotRepository _physicianScheduleSlotRepository;
        private readonly IIdentityService _identityService;

        public RemoveScheduleSlotCommandHandler(IPhysicianScheduleSlotRepository physicianScheduleSlotRepository,
                           IIdentityService identityService)
        {
            _physicianScheduleSlotRepository = physicianScheduleSlotRepository ?? throw new ArgumentNullException(nameof(physicianScheduleSlotRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        public async Task<OperationResponse> Handle(RemoveScheduleSlotCommand request, CancellationToken cancellationToken)
        {
            var physicianId = _identityService.GetUserIdentity();
            var scheduleSlotId = request.ScheduleSlotId;

            await _physicianScheduleSlotRepository.DeleteScheduleSlotAsync(scheduleSlotId, physicianId);

            return new OperationResponse();
        }
    }

}
