using AutoMapper;
using CloudPharmacy.Common.CommonResponse;
using CloudPharmacy.Physician.API.Application.DTO;
using CloudPharmacy.Physician.API.Application.Repositories;
using CloudPharmacy.Physician.Application.Model;
using MediatR;

namespace CloudPharmacy.Physician.API.Application.Queries
{
    public class GetPhysicianFreeScheduleSlotsQuery : IRequest<OperationResponse<IList<PhysicianFreeScheduleSlotDTO>>>
    {
        public string PhysicianId { get; set; }
    }

    internal class GetPhysicianFreeScheduleSlotsQueryHandler : IRequestHandler<GetPhysicianFreeScheduleSlotsQuery, OperationResponse<IList<PhysicianFreeScheduleSlotDTO>>>
    {
        private readonly IPhysicianScheduleSlotRepository _physicianScheduleSlotRepository;
        private readonly IMapper _mapper;

        public GetPhysicianFreeScheduleSlotsQueryHandler(IPhysicianScheduleSlotRepository physicianScheduleSlotRepository,
                                                        IMapper mapper)
        {
            _physicianScheduleSlotRepository = physicianScheduleSlotRepository
                                               ?? throw new ArgumentNullException(nameof(physicianScheduleSlotRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OperationResponse<IList<PhysicianFreeScheduleSlotDTO>>> Handle(GetPhysicianFreeScheduleSlotsQuery request, CancellationToken cancellationToken)
        {
            var physicianId = request.PhysicianId;

            IList<PhysicianScheduleSlot> scheduleSlots = await _physicianScheduleSlotRepository.GetFreeSlotsFromScheduleAsync(physicianId);

            var scheduleSlotsDTOs = _mapper.Map<List<PhysicianFreeScheduleSlotDTO>>(scheduleSlots);

            return new OperationResponse<IList<PhysicianFreeScheduleSlotDTO>>()
            {
                Result = scheduleSlotsDTOs
            };
        }
    }
}
