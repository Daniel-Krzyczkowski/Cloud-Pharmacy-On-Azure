using CloudPharmacy.PharmacyStore.WebApp.Infrastructure.Configuration;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;

namespace CloudPharmacy.PharmacyStore.WebApp.Infrastructure.API
{
    public interface IPharmacyStoreAPI
    {
        Task<HttpResponseMessage> VerifyPatientVerifiableCredentialAsync();
        Task<HttpResponseMessage> GetMedicamentsAsync();
    }
    internal class PharmacyStoreAPI : IPharmacyStoreAPI
    {
        private readonly HttpClient _httpClient;
        protected readonly ITokenAcquisition _tokenAcquisition;
        private readonly IPharmacyStoreAPIConfiguration _pharmacyStoreAPIConfiguration;

        public PharmacyStoreAPI(HttpClient httpClient,
                    ITokenAcquisition tokenAcquisition,
                    IPharmacyStoreAPIConfiguration pharmacyStoreAPIConfiguration)
        {
            _httpClient = httpClient
                          ?? throw new ArgumentNullException(nameof(httpClient));
            _tokenAcquisition = tokenAcquisition
                          ?? throw new ArgumentNullException(nameof(tokenAcquisition));
            _pharmacyStoreAPIConfiguration = pharmacyStoreAPIConfiguration
                          ?? throw new ArgumentNullException(nameof(pharmacyStoreAPIConfiguration));
        }

        public async Task<HttpResponseMessage> VerifyPatientVerifiableCredentialAsync()
        {
            await GetAndAddApiAccessTokenToAuthorizationHeaderAsync();
            var response = await _httpClient
                                 .GetAsync($"{_pharmacyStoreAPIConfiguration.Url}/api/medicament/verify/patient/verifiable-credential");
            return response;
        }

        public async Task<HttpResponseMessage> GetMedicamentsAsync()
        {
            await GetAndAddApiAccessTokenToAuthorizationHeaderAsync();
            var response = await _httpClient
                     .GetAsync($"{_pharmacyStoreAPIConfiguration.Url}/api/medicament/all");
            return response;
        }

        protected async Task GetAndAddApiAccessTokenToAuthorizationHeaderAsync()
        {
            string[] scopes = new[] { _pharmacyStoreAPIConfiguration.Scope };
            string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
