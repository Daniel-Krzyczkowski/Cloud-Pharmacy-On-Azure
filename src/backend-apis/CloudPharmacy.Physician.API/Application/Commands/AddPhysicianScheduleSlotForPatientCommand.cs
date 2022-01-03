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
    internal class AddPhysicianScheduleSlotForPatientCommand : IRequest<OperationResponse>
    {
        public PhysicianScheduleSlotForPatientDTO PhysicianScheduleSlotForPatientDTO { get; set; }
    }

    internal class AddPhysicianScheduleSlotForPatientCommandHandler : IRequestHandler<AddPhysicianScheduleSlotForPatientCommand,
                                                                    OperationResponse>
    {
        private readonly IPhysicianScheduleSlotRepository _physicianScheduleSlotRepository;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public AddPhysicianScheduleSlotForPatientCommandHandler(IPhysicianScheduleSlotRepository physicianScheduleSlotRepository,
                                   IIdentityService identityService,
                                   IMapper mapper)
        {
            _physicianScheduleSlotRepository = physicianScheduleSlotRepository ?? throw new ArgumentNullException(nameof(physicianScheduleSlotRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<OperationResponse> Handle(AddPhysicianScheduleSlotForPatientCommand request, CancellationToken cancellationToken)
        {
            var newScheduleSlotForPatientDTO = request.PhysicianScheduleSlotForPatientDTO;

            if (newScheduleSlotForPatientDTO.SlotDateAndTime > DateTimeOffset.Now)
            {
                return new OperationResponse()
                                .SetAsFailureResponse(OperationErrorDictionary.PhysicianScheduleSlot.WrongSlotTime());
            }

            var physicianId = _identityService.GetUserIdentity();

            var newScheduleSlotForPatient = new PhysicianScheduleSlot()
            {
                Id = Guid.NewGuid().ToString(),
                PhysicianId = physicianId,
                SlotDateAndTime = newScheduleSlotForPatientDTO.SlotDateAndTime
            };

            var patientDTO = newScheduleSlotForPatientDTO.Patient;
            if (newScheduleSlotForPatientDTO.Patient != null)
            {
                newScheduleSlotForPatient.Patient = _mapper.Map<PatientProfile>(patientDTO);
            }

            await _physicianScheduleSlotRepository.AddNewSlotToScheduleAsync(newScheduleSlotForPatient);

            return new OperationResponse();
        }
    }
}
