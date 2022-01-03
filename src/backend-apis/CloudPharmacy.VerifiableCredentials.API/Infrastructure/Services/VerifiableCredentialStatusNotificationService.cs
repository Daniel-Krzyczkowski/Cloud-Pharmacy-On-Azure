using CloudPharmacy.VerifiableCredentials.API.Application.Model;
using CloudPharmacy.VerifiableCredentials.API.Infrastructure.Configuration;

namespace CloudPharmacy.VerifiableCredentials.API.Infrastructure.Services
{
    public interface IVerifiableCredentialStatusNotificationService
    {
        Task SendVerifiableCredentialIssuanceStatusUpdateAsync(IssuanceStatusResponse issuanceStatusResponse);
        Task SendVerifiableCredentialVerificationStatusUpdateAsync(VerificationStatusResponse verificationStatusResponse);
    }

    internal class VerifiableCredentialStatusNotificationService : IVerifiableCredentialStatusNotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILiveNotificationsFuncAppConfiguration _liveNotificationsFuncAppConfiguration;

        public VerifiableCredentialStatusNotificationService(HttpClient httpClient,
            ILiveNotificationsFuncAppConfiguration liveNotificationsFuncAppConfiguration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _liveNotificationsFuncAppConfiguration = liveNotificationsFuncAppConfiguration
                                                        ?? throw new ArgumentNullException(nameof(liveNotificationsFuncAppConfiguration));
        }

        public async Task SendVerifiableCredentialIssuanceStatusUpdateAsync(IssuanceStatusResponse issuanceStatusResponse)
        {
            _httpClient.DefaultRequestHeaders.Remove("x-ms-client-principal-id");
            _httpClient.DefaultRequestHeaders.Add("x-ms-client-principal-id", issuanceStatusResponse.UserId);
            await _httpClient.PostAsJsonAsync(_liveNotificationsFuncAppConfiguration.VerifiableCredentialIssuanceStatusUpdateEndpointUrl,
                                              issuanceStatusResponse);
        }

        public async Task SendVerifiableCredentialVerificationStatusUpdateAsync(VerificationStatusResponse verificationStatusResponse)
        {
            _httpClient.DefaultRequestHeaders.Remove("x-ms-client-principal-id");
            _httpClient.DefaultRequestHeaders.Add("x-ms-client-principal-id", verificationStatusResponse.UserId);
            await _httpClient.PostAsJsonAsync(_liveNotificationsFuncAppConfiguration.VerifiableCredentialVerificationStatusUpdateEndpointUrl,
                                              verificationStatusResponse);
        }
    }
}
