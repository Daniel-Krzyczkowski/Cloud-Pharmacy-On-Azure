using CloudPharmacy.PharmacyStore.WebApp.Infrastructure.Configuration;
using Microsoft.AspNetCore.SignalR.Client;

namespace CloudPharmacy.PharmacyStore.WebApp.Infrastructure.Notifications
{
    internal class VerifiableCredentialsNotificationService
    {
        private readonly IVerifiableCredentialsNotificationServiceConfiguration _verifiableCredentialsNotificationServiceConfiguration;

        private HubConnection _hub;

        public VerifiableCredentialsNotificationService(IVerifiableCredentialsNotificationServiceConfiguration
                                                                            verifiableCredentialsNotificationServiceConfiguration)
        {
            _verifiableCredentialsNotificationServiceConfiguration = verifiableCredentialsNotificationServiceConfiguration
                                                                     ?? throw new ArgumentNullException(nameof(verifiableCredentialsNotificationServiceConfiguration));
        }

        /// <summary>
        /// Initialize connection with hub.
        /// </summary>
        /// <param name="connectionUrl"></param>
        /// <returns></returns>
        public async Task Initialize(string userId)
        {
            var connectionUrl = _verifiableCredentialsNotificationServiceConfiguration.Url;

            if (_hub == null)
            {
                _hub = new HubConnectionBuilder()
                    .WithUrl(connectionUrl, configureHttpConnection =>
                    {
                        configureHttpConnection.Headers.Add("x-ms-client-principal-id", userId);
                    })
                    .Build();
            }
        }

        /// <summary>
        /// Open connection to the server
        /// </summary>
        public async Task OpenConnectionAsync()
        {
            if (_hub.State == HubConnectionState.Disconnected)
            {
                await _hub.StartAsync();
            }
        }

        /// <summary>
        /// Close connection to the server
        /// </summary>
        public async Task CloseConnectionAsync()
        {
            if (_hub.State == HubConnectionState.Connected)
            {
                await _hub.DisposeAsync();
                _hub = null;
            }
        }

        /// <summary>
        /// Check if connection to hub is active
        /// </summary>
        /// 
        public bool IsConnectionOpened
        {
            get
            {
                return _hub != null && _hub.State == HubConnectionState.Connected;
            }
        }


        /// <summary>
        /// Subscribe to receive updates from the hub.
        /// </summary>
        /// <param name="methodName"></param>
        public void SubscribeHubMethod(string methodName)
        {
            _hub.On<string>(methodName, (notification) =>
            {
                OnMessageReceived?.Invoke(notification);
            });
        }

        public event Action<string> OnMessageReceived;
    }
}
