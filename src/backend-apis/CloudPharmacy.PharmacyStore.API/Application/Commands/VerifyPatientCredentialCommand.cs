using CloudPharmacy.Common.CommonResponse;
using CloudPharmacy.PharmacyStore.API.Infrastructure.Services.Identity;
using MediatR;

namespace CloudPharmacy.PharmacyStore.API.Application.Commands
{
    public class VerifyPatientCredentialCommand : IRequest<OperationResponse>
    {
        public HttpResponse HttpResponse { get; set; }
    }

    internal class VerifyPatientCredentialQueryHandler : IRequestHandler<VerifyPatientCredentialCommand, OperationResponse>
    {
        private readonly IVerifiableCredentialsService _verifiableCredentialsService;

        public VerifyPatientCredentialQueryHandler(IVerifiableCredentialsService verifiableCredentialsService)
        {
            _verifiableCredentialsService = verifiableCredentialsService
                                            ?? throw new ArgumentNullException(nameof(verifiableCredentialsService));
        }

        public async Task<OperationResponse> Handle(VerifyPatientCredentialCommand request, CancellationToken cancellationToken)
        {
            await _verifiableCredentialsService.VerifyPatientCredentialAsync(request.HttpResponse);
            return new OperationResponse();
        }
    }
}
