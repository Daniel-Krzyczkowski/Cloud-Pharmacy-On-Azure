using CloudPharmacy.Patient.WebApp.Infrastructure.Configuration;

namespace CloudPharmacy.Patient.WebApp.Infrastructure.API
{
    internal interface IPhysicianAPI
    {
        Task<HttpResponseMessage> GetAllPhysiciansAsync();
        Task<HttpResponseMessage> GetFreeSlotsForPhysicianAsync(string physicianId);
    }

    internal class PhysicianAPI : IPhysicianAPI
    {
        private readonly HttpClient _httpClient;
        private readonly IPhysicianAPIConfiguration _physicianAPIConfiguration;
        public PhysicianAPI(HttpClient httpClient,
            IPhysicianAPIConfiguration physicianAPIConfiguration)
        {
            _httpClient = httpClient
                          ?? throw new ArgumentNullException(nameof(httpClient));
            _physicianAPIConfiguration = physicianAPIConfiguration
                          ?? throw new ArgumentNullException(nameof(physicianAPIConfiguration));
        }

        public async Task<HttpResponseMessage> GetAllPhysiciansAsync()
        {
            var response = await _httpClient
                                 .GetAsync($"{_physicianAPIConfiguration.Url}/api/physician/profiles");
            return response;
        }

        public async Task<HttpResponseMessage> GetFreeSlotsForPhysicianAsync(string physicianId)
        {
            var response = await _httpClient
                                 .GetAsync($"{_physicianAPIConfiguration.Url}/api/Physician/schdule/free-slots/{physicianId}");
            return response;
        }
    }
}
