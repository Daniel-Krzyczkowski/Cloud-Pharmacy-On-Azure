using CloudPharmacy.Physician.WebApp.Application.Model;
using CloudPharmacy.Physician.WebApp.Infrastructure.Configuration;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CloudPharmacy.Physician.WebApp.Infrastructure.API
{
    internal interface IPhysicianAPI
    {
        Task<HttpResponseMessage> GetPhysicianProfileAsync();
        Task<HttpResponseMessage> UpdatePhysicianProfileAsync(PhysicianProfileUpdate physicianProfileUpdate);
        Task<HttpResponseMessage> AddNewScheduleSlotAsync(PhysicianScheduleSlot physicianScheduleSlot);
        Task<HttpResponseMessage> GetFreeSlotsAsync(string physicianId);
    }

    internal class PhysicianAPI : IPhysicianAPI
    {
        private readonly HttpClient _httpClient;
        protected readonly ITokenAcquisition _tokenAcquisition;
        private readonly IPhysicianAPIConfiguration _physicianAPIConfiguration;

        public PhysicianAPI(HttpClient httpClient,
                            ITokenAcquisition tokenAcquisition,
                            IPhysicianAPIConfiguration physicianAPIConfiguration)
        {
            _httpClient = httpClient
                                ?? throw new ArgumentNullException(nameof(httpClient));
            _tokenAcquisition = tokenAcquisition
                    ?? throw new ArgumentNullException(nameof(tokenAcquisition));
            _physicianAPIConfiguration = physicianAPIConfiguration
                                            ?? throw new ArgumentNullException(nameof(physicianAPIConfiguration));
        }

        public async Task<HttpResponseMessage> GetPhysicianProfileAsync()
        {
            await GetAndAddApiAccessTokenToAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync($"{_physicianAPIConfiguration.Url}/api/physician/profile");
            return response;
        }

        public async Task<HttpResponseMessage> UpdatePhysicianProfileAsync(PhysicianProfileUpdate physicianProfileUpdate)
        {
            await GetAndAddApiAccessTokenToAuthorizationHeaderAsync();
            var multipartContent = new MultipartFormDataContent
            {
                { new StringContent(physicianProfileUpdate.Specialization), "specialization" }
            };

            if (physicianProfileUpdate.ProfilePictureFile != null)
            {
                multipartContent.Add(new StreamContent(physicianProfileUpdate.ProfilePictureFile),
                                                       "PhotoFile",
                                                       physicianProfileUpdate.ProfilePictureFileName);
            }
            var response = await _httpClient.PutAsync($"{_physicianAPIConfiguration.Url}/api/physician/profile", multipartContent);
            return response;
        }

        public async Task<HttpResponseMessage> AddNewScheduleSlotAsync(PhysicianScheduleSlot physicianScheduleSlot)
        {
            await GetAndAddApiAccessTokenToAuthorizationHeaderAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var response = await _httpClient.PostAsJsonAsync($"{_physicianAPIConfiguration.Url}/api/physician/free-slot",
                                                            physicianScheduleSlot, options);
            return response;
        }

        public async Task<HttpResponseMessage> GetFreeSlotsAsync(string physicianId)
        {
            var response = await _httpClient.GetAsync($"{_physicianAPIConfiguration.Url}/api/physician/schdule/free-slots/{physicianId}");
            return response;
        }

        protected async Task GetAndAddApiAccessTokenToAuthorizationHeaderAsync()
        {
            string[] scopes = new[] { _physicianAPIConfiguration.Scope };
            string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}
