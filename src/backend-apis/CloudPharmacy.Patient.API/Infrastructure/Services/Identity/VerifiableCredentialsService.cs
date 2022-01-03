using CloudPharmacy.Patient.API.Application.DTO;
using CloudPharmacy.Patient.API.Infrastructure.Configuration;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CloudPharmacy.Patient.API.Infrastructure.Services.Identity
{
    public interface IVerifiableCredentialsService
    {
        Task IssuePatientCredentialAsync(HttpResponse httpResponse);
    }

    internal class VerifiableCredentialsService : IVerifiableCredentialsService
    {
        private readonly HttpClient _httpClient;
        private readonly IVerifiableCredentialsServiceConfiguration _verifiableCredentialsServiceConfiguration;
        private readonly IIdentityService _identityService;
        private readonly IConfidentialClientApplication _confidentialClient;
        private readonly ILogger<VerifiableCredentialsService> _logger;

        public VerifiableCredentialsService(HttpClient httpClient,
                                           IVerifiableCredentialsServiceConfiguration verifiableCredentialsServiceConfiguration,
                                           IIdentityService identityService,
                                           IConfidentialClientApplication confidentialClient,
                                           ILogger<VerifiableCredentialsService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _verifiableCredentialsServiceConfiguration = verifiableCredentialsServiceConfiguration
                                    ?? throw new ArgumentNullException(nameof(verifiableCredentialsServiceConfiguration));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _confidentialClient = confidentialClient ?? throw new ArgumentNullException(nameof(confidentialClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task IssuePatientCredentialAsync(HttpResponse httpResponse)
        {
            var patientProfileData = new PatientProfileDataForVerifiableCredentialDTO()
            {
                PatientId = _identityService.GetUserIdentity(),
                FirstNameAndLastName = _identityService.GetUserFirstNameAndLastName()
            };

            AuthenticationResult result = await _confidentialClient
                       .AcquireTokenForClient(new string[] { _verifiableCredentialsServiceConfiguration.Scope })
                       .ExecuteAsync();

            var accessToken = result.AccessToken;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var response = await _httpClient.PostAsJsonAsync(_verifiableCredentialsServiceConfiguration.IssuanceEndpointUrl,
                                                             patientProfileData,
                                                             options);
            var responseBody = await response.Content.ReadAsStringAsync();

            var bytes = Encoding.UTF8.GetBytes(responseBody);
            httpResponse.StatusCode = (int)response.StatusCode;
            httpResponse.ContentType = "application/json";
            await httpResponse.Body.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
