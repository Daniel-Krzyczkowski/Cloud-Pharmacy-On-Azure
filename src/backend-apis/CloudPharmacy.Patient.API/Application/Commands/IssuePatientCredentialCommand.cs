using CloudPharmacy.Common.CommonResponse;
using CloudPharmacy.Patient.API.Infrastructure.Services.Identity;
using MediatR;

namespace CloudPharmacy.Patient.API.Application.Commands
{
    public class IssuePatientCredentialCommand : IRequest<OperationResponse>
    {
        public HttpResponse HttpResponse { get; set; }
    }

    internal class VerifyPatientCredentialQueryHandler : IRequestHandler<IssuePatientCredentialCommand, OperationResponse>
    {
        private readonly IVerifiableCredentialsService _verifiableCredentialsService;

        public VerifyPatientCredentialQueryHandler(IVerifiableCredentialsService verifiableCredentialsService)
        {
            _verifiableCredentialsService = verifiableCredentialsService
                                            ?? throw new ArgumentNullException(nameof(verifiableCredentialsService));
        }

        public async Task<OperationResponse> Handle(IssuePatientCredentialCommand request, CancellationToken cancellationToken)
        {
            await _verifiableCredentialsService.IssuePatientCredentialAsync(request.HttpResponse);
            return new OperationResponse();
        }
    }
}
