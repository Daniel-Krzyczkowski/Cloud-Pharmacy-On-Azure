using AutoMapper;
using CloudPharmacy.Common.CommonResponse;
using CloudPharmacy.PharmacyStore.API.Application.DTO;
using CloudPharmacy.PharmacyStore.API.Application.Model;
using CloudPharmacy.PharmacyStore.API.Application.Repositories;
using MediatR;

namespace CloudPharmacy.PharmacyStore.API.Application.Queries
{
    internal class GetMedicamentsQuery : IRequest<OperationResponse<IList<MedicamentDTO>>>
    {
    }

    internal class GetMedicamentsQueryHandler : IRequestHandler<GetMedicamentsQuery, OperationResponse<IList<MedicamentDTO>>>
    {
        private readonly IMedicamentRepository _medicamentRepository;
        private readonly IMapper _mapper;

        public GetMedicamentsQueryHandler(IMedicamentRepository medicamentRepository,
                    IMapper mapper)
        {
            _medicamentRepository = medicamentRepository ?? throw new ArgumentNullException(nameof(medicamentRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OperationResponse<IList<MedicamentDTO>>> Handle(GetMedicamentsQuery request, CancellationToken cancellationToken)
        {
            var allMedicaments = await _medicamentRepository.GetAllMedicamentsAsync();
            IList<MedicamentDTO> allMedicamentsDTOs = _mapper.Map<IList<Medicament>,
                                                                      IList<MedicamentDTO>>(allMedicaments);

            return new OperationResponse<IList<MedicamentDTO>>()
            {
                Result = allMedicamentsDTOs
            };
        }
    }
}
