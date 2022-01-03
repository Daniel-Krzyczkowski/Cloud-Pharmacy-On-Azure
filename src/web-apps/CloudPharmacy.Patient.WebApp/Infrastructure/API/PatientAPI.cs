using CloudPharmacy.Patient.WebApp.Infrastructure.Configuration;
using CloudPharmacy.Patient.WebApp.Application.Model;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CloudPharmacy.Patient.WebApp.Infrastructure.API
{
    internal interface IPatientAPI
    {
        Task<HttpResponseMessage> GetPatientProfileAsync();
        Task<HttpResponseMessage> UpdatePatientProfileAsync(PatientProfileUpdate patientProfileUpdate);
        Task<HttpResponseMessage> IssuePatientVerifiableCredentialAsync();
    }

    internal class PatientAPI : IPatientAPI
    {
        private readonly HttpClient _httpClient;
        protected readonly ITokenAcquisition _tokenAcquisition;
        private readonly IPatientAPIConfiguration _patientAPIConfiguration;

        public PatientAPI(HttpClient httpClient,
                    ITokenAcquisition tokenAcquisition,
                    IPatientAPIConfiguration patientAPIConfiguration)
        {
            _httpClient = httpClient
                          ?? throw new ArgumentNullException(nameof(httpClient));
            _tokenAcquisition = tokenAcquisition
                          ?? throw new ArgumentNullException(nameof(tokenAcquisition));
            _patientAPIConfiguration = patientAPIConfiguration
                          ?? throw new ArgumentNullException(nameof(patientAPIConfiguration));
        }

        public async Task<HttpResponseMessage> GetPatientProfileAsync()
        {
            await GetAndAddApiAccessTokenToAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync($"{_patientAPIConfiguration.Url}/api/patient/profile");
            return response;
        }

        public async Task<HttpResponseMessage> UpdatePatientProfileAsync(PatientProfileUpdate patientProfileUpdate)
        {
            await GetAndAddApiAccessTokenToAuthorizationHeaderAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var response = await _httpClient.PutAsJsonAsync($"{_patientAPIConfiguration.Url}/api/patient/profile",
                                                            patientProfileUpdate, options);
            return response;
        }

        public async Task<HttpResponseMessage> IssuePatientVerifiableCredentialAsync()
        {
            await GetAndAddApiAccessTokenToAuthorizationHeaderAsync();
            var response = await _httpClient
                                 .GetAsync($"{_patientAPIConfiguration.Url}/api/patient/issue/verifiable-credential");
            return response;
        }

        protected async Task GetAndAddApiAccessTokenToAuthorizationHeaderAsync()
        {
            string[] scopes = new[] { _patientAPIConfiguration.Scope };
            string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
