using AutoMapper;
using CloudPharmacy.Common.CommonResponse;
using CloudPharmacy.Physician.API.Application.DTO;
using CloudPharmacy.Physician.API.Application.Repositories;
using CloudPharmacy.Physician.API.Infrastructure.Services.Identity;
using CloudPharmacy.Physician.Application.Model;
using MediatR;

namespace CloudPharmacy.Physician.API.Application.Queries
{
    public class GetPhysicianReservedScheduleSlotsQuery : IRequest<OperationResponse<IList<PhysicianScheduleSlotForPatientDTO>>>
    {
    }

    internal class GetPhysicianReservedScheduleSlotsQueryHandler : IRequestHandler<GetPhysicianReservedScheduleSlotsQuery,
                                                                            OperationResponse<IList<PhysicianScheduleSlotForPatientDTO>>>
    {
        private readonly IPhysicianScheduleSlotRepository _physicianScheduleSlotRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetPhysicianReservedScheduleSlotsQueryHandler(IPhysicianScheduleSlotRepository physicianScheduleSlotRepository,
                           IIdentityService identityService,
                            IMapper mapper)
        {
            _physicianScheduleSlotRepository = physicianScheduleSlotRepository
                                               ?? throw new ArgumentNullException(nameof(physicianScheduleSlotRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OperationResponse<IList<PhysicianScheduleSlotForPatientDTO>>> Handle(GetPhysicianReservedScheduleSlotsQuery request,
                                                                                     CancellationToken cancellationToken)
        {
            var physicianId = _identityService.GetUserIdentity();
            IList<PhysicianScheduleSlot> scheduleSlots = await _physicianScheduleSlotRepository.GetReservedSlotsFromScheduleAsync(physicianId);


            var scheduleSlotsDTOs = _mapper.Map<List<PhysicianScheduleSlotForPatientDTO>>(scheduleSlots);

            return new OperationResponse<IList<PhysicianScheduleSlotForPatientDTO>>()
            {
                Result = scheduleSlotsDTOs
            };
        }
    }
}
