using CloudPharmacy.VerifiableCredentials.API.Application.DTO;
using CloudPharmacy.VerifiableCredentials.API.Application.Model;
using CloudPharmacy.VerifiableCredentials.API.Infrastructure.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CloudPharmacy.VerifiableCredentials.API.Infrastructure.Services
{

    public interface IVerifiableCredentialsManagementService
    {
        Task<IssuanceResponse> IssueNewVerifiableCredentialAsync(PatientProfileDataForVerifiableCredentialDTO patientDTO);
        Task<VerificationResponse> VerifyExistingVerifiableCredentialAsync(PatientProfileDataForVerifiableCredentialDTO patientDTO);
        Task<IssuanceStatusResponse> VerifyIssuanceStatusAsync(string issuanceStatusResponseAsString);
        Task<VerificationStatusResponse> VerifyPresentationStatusAsync(string presentationVerificationStatusResponseAsString);
    }

    internal class VerifiableCredentialsManagementService : IVerifiableCredentialsManagementService
    {
        private readonly HttpClient _httpClient;
        private readonly IVerifiableCredentialsConfiguration _verifiableCredentialsConfiguration;
        private IMemoryCache _cache;
        private readonly ILogger<VerifiableCredentialsManagementService> _logger;

        public VerifiableCredentialsManagementService(HttpClient httpClient,
                                                      IVerifiableCredentialsConfiguration verifiableCredentialsConfiguration,
                                                      IMemoryCache cache,
                                                      ILogger<VerifiableCredentialsManagementService> logger)
        {
            _httpClient = httpClient
                    ?? throw new ArgumentNullException(nameof(httpClient));
            _verifiableCredentialsConfiguration = verifiableCredentialsConfiguration
                    ?? throw new ArgumentNullException(nameof(verifiableCredentialsConfiguration));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger
                    ?? throw new ArgumentNullException(nameof(logger));
        }

        private async Task<string> GetAccessTokenAsync()
        {
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
                    .Create(_verifiableCredentialsConfiguration.ClientId)
                    .WithClientSecret(_verifiableCredentialsConfiguration.ClientSecret)
                    .WithAuthority(new Uri(_verifiableCredentialsConfiguration.TokenAuthority))
                    .Build();

            //X509Certificate2 certificate = AppSettings.ReadCertificate(AppSettings.CertificateName);
            //app = ConfidentialClientApplicationBuilder.Create(AppSettings.ClientId)
            //    .WithCertificate(certificate)
            //    .WithAuthority(new Uri(AppSettings.Authority))
            //    .Build();

            app.AddDistributedTokenCache(services =>
            {
                services.AddDistributedMemoryCache();
                services.AddLogging(configure => configure.AddConsole())
                .Configure<LoggerFilterOptions>(options => options.MinLevel = Microsoft.Extensions.Logging.LogLevel.Debug);
            });

            string[] scopes = new string[] { _verifiableCredentialsConfiguration.VerifiableCredentialsServiceScope };

            AuthenticationResult result = null;
            try
            {
                result = await app.AcquireTokenForClient(scopes)
                                  .ExecuteAsync();
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                // Invalid scope. The scope has to be of the form "https://resourceurl/.default"
                _logger.LogError("Scope provided is not supported");
            }
            catch (MsalServiceException ex)
            {
                _logger.LogError("Something went wrong getting an access token for the Verifiable Credentials client API:" + ex.Message);
            }

            if (result == null)
            {
                return string.Empty;
            }

            else
            {
                return result.AccessToken;
            }
        }

        public async Task<IssuanceResponse> IssueNewVerifiableCredentialAsync(PatientProfileDataForVerifiableCredentialDTO patientDTO)
        {
            int min = 1000;
            int max = 9999;
            Random _rdm = new Random();
            var pinValue = _rdm.Next(min, max).ToString();
            string state = Guid.NewGuid().ToString();

            IssuanceRequest issuanceRequest = new IssuanceRequest();
            issuanceRequest.Authority = _verifiableCredentialsConfiguration.IssuerAuthority;
            issuanceRequest.IncludeQRCode = true;
            issuanceRequest.Registration = new Registration()
            {
                ClientName = _verifiableCredentialsConfiguration.IssuanceRegistrationClientName,
                Purpose = _verifiableCredentialsConfiguration.IssuanceRegistrationPurpose
            };
            issuanceRequest.Issuance = new Issuance()
            {
                Type = _verifiableCredentialsConfiguration.IssuanceType,
                Manifest = _verifiableCredentialsConfiguration.CredentialManifest,
                Pin = new Pin()
                {
                    Value = pinValue,
                    Length = 4
                }
            };
            issuanceRequest.Callback = new Callback()
            {
                Url = _verifiableCredentialsConfiguration.IssuanceCallbackUrl,
                State = state,
                Headers = new Headers()
                {
                    ApiKey = _verifiableCredentialsConfiguration.IssuanceCallbackApiKey
                }
            };
            issuanceRequest.Issuance.Claims = new Claims()
            {
                PatientId = patientDTO.PatientId,
                PatientName = patientDTO.FirstNameAndLastName
            };

            var accessToken = await GetAccessTokenAsync();

            if (!string.IsNullOrEmpty(accessToken))
            {
                var defaultRequestHeaders = _httpClient.DefaultRequestHeaders;
                defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpStatusCode statusCode = HttpStatusCode.OK;
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                var issuanceRequestAsString = JsonSerializer.Serialize(issuanceRequest, options);

                HttpResponseMessage response = await _httpClient
                                                    .PostAsync(_verifiableCredentialsConfiguration.ApiEndpoint,
                                                               new StringContent(issuanceRequestAsString, Encoding.UTF8, "application/json"));
                statusCode = response.StatusCode;
                string issuanceResponseAsString = await response.Content.ReadAsStringAsync();

                if (statusCode == HttpStatusCode.Created)
                {
                    var issuanceResponse = JsonSerializer.Deserialize<IssuanceResponse>(issuanceResponseAsString, options);
                    issuanceResponse.Id = state;
                    issuanceResponse.Pin = pinValue;


                    var cacheData = new
                    {
                        status = "notscanned",
                        message = "Request ready, please scan with Authenticator",
                        expiry = issuanceResponse.Expiry,
                        userId = patientDTO.PatientId
                    };
                    _cache.Set(state, JsonSerializer.Serialize(cacheData));

                    return issuanceResponse;
                }

                else
                {
                    _logger.LogError("Unsuccesfully called Verifiable Credentials Issuance Request API");
                    _logger.LogError($"Verifiable Credentials Request API error: {issuanceResponseAsString}");
                }
            }

            else
            {
                _logger.LogError("Cannot create Verifiable Credentials issuance request because access token is empty");
            }

            return null;
        }

        public async Task<VerificationResponse> VerifyExistingVerifiableCredentialAsync(PatientProfileDataForVerifiableCredentialDTO patientDTO)
        {
            string state = Guid.NewGuid().ToString();
            VerificationRequest verificationRequest = new VerificationRequest();
            verificationRequest.IncludeQRCode = true;
            verificationRequest.Callback = new Callback()
            {
                Url = _verifiableCredentialsConfiguration.PresentationCallbackUrl,
                State = state,
                Headers = new Headers()
                {
                    ApiKey = _verifiableCredentialsConfiguration.PresentationCallbackApiKey
                }
            };
            verificationRequest.Authority = _verifiableCredentialsConfiguration.IssuerAuthority;
            verificationRequest.Registration = new Registration()
            {
                ClientName = _verifiableCredentialsConfiguration.IssuanceRegistrationClientName,
                Purpose = _verifiableCredentialsConfiguration.IssuanceRegistrationPurpose
            };
            verificationRequest.Presentation = new Presentation()
            {
                IncludeReceipt = false,
                RequestedCredentials = new List<RequestedCredential>
                {
                    new RequestedCredential
                    {
                        AcceptedIssuers = new List<string>
                        {
                            _verifiableCredentialsConfiguration.IssuerAuthority
                        },
                        Purpose = _verifiableCredentialsConfiguration.IssuanceRegistrationPurpose,
                        Type = _verifiableCredentialsConfiguration.IssuanceType
                    }
                }
            };

            var accessToken = await GetAccessTokenAsync();

            if (!string.IsNullOrEmpty(accessToken))
            {
                var defaultRequestHeaders = _httpClient.DefaultRequestHeaders;
                defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpStatusCode statusCode = HttpStatusCode.OK;
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                var verificationRequestAsString = JsonSerializer.Serialize(verificationRequest, options);

                HttpResponseMessage response = await _httpClient
                                                    .PostAsync(_verifiableCredentialsConfiguration.ApiEndpoint,
                                                               new StringContent(verificationRequestAsString, Encoding.UTF8, "application/json"));
                statusCode = response.StatusCode;
                string verificationResponseAsString = await response.Content.ReadAsStringAsync();

                if (statusCode == HttpStatusCode.Created)
                {
                    var verificationResponse = JsonSerializer.Deserialize<VerificationResponse>(verificationResponseAsString, options);
                    verificationResponse.Id = state;
                    var cacheData = new
                    {
                        status = "notscanned",
                        message = "Verification code generated, please scan with Authenticator",
                        userId = patientDTO.PatientId
                    };
                    _cache.Set(state, JsonSerializer.Serialize(cacheData));

                    return verificationResponse;
                }

                else
                {
                    _logger.LogError("Unsuccesfully called Verifiable Credentials Verification Request API");
                    _logger.LogError($"Verifiable Credentials Request API error: {verificationResponseAsString}");
                }
            }

            else
            {
                _logger.LogError("Cannot create Verifiable Credentials verification request because access token is empty");
            }

            return null;
        }

        public async Task<IssuanceStatusResponse> VerifyIssuanceStatusAsync(string issuanceStatusResponseAsString)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var issuanceStatusResponse = JsonSerializer.Deserialize<IssuanceStatusResponse>(issuanceStatusResponseAsString, options);

            var issuanceCode = issuanceStatusResponse.Code;
            var cacheDataAsJson = _cache.Get<string>(issuanceStatusResponse.State);
            var data = JsonObject.Parse(cacheDataAsJson);
            issuanceStatusResponse.UserId = data["userId"].ToString();

            if (issuanceCode == IssuanceStatus.QrCodeScannedByUser)
            {
                var cacheData = new
                {
                    status = "request_retrieved",
                    message = "QR Code is scanned. Waiting for issuance...",
                    userId = data["userId"].ToString()
                };
                _cache.Set(issuanceStatusResponse.State, JsonSerializer.Serialize(cacheData));
            }


            if (issuanceCode == IssuanceStatus.VerifiableCredentialSuccessfullyIssued)
            {
                var cacheData = new
                {
                    status = "issuance_successful",
                    message = "Credential successfully issued",
                    userId = data["userId"].ToString()
                };
                _cache.Set(issuanceStatusResponse.State, JsonSerializer.Serialize(cacheData));
            }

            if (issuanceCode == IssuanceStatus.IssuanceError)
            {
                var cacheData = new
                {
                    status = "issuance_error",
                    payload = issuanceStatusResponse.Error.Code,
                    message = issuanceStatusResponse.Error.Message,
                    userId = data["userId"].ToString()

                };
                _cache.Set(issuanceStatusResponse.State, JsonSerializer.Serialize(cacheData));
            }

            return issuanceStatusResponse;
        }

        public async Task<VerificationStatusResponse> VerifyPresentationStatusAsync(string presentationVerificationStatusResponseAsString)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var verificationStatusResponse = JsonSerializer.Deserialize<VerificationStatusResponse>(presentationVerificationStatusResponseAsString, options);

            var issuanceCode = verificationStatusResponse.Code;
            var cacheDataAsJson = _cache.Get<string>(verificationStatusResponse.State);
            var data = JsonObject.Parse(cacheDataAsJson);
            verificationStatusResponse.UserId = data["userId"].ToString();

            if (issuanceCode == VerificationStatus.RequestOpenedInAuthenticatorApp)
            {
                var cacheData = new
                {
                    status = "request_retrieved",
                    message = "QR Code is scanned. Waiting for issuance...",
                    userId = data["userId"].ToString()
                };
                _cache.Set(verificationStatusResponse.State, JsonSerializer.Serialize(cacheData));
            }


            if (issuanceCode == VerificationStatus.VerifiableCredentialSuccessfullyPresented)
            {
                var cacheData = new
                {
                    status = "presentation_verified",
                    message = "Credential successfully presented",
                    userId = data["userId"].ToString()
                };
                _cache.Set(verificationStatusResponse.State, JsonSerializer.Serialize(cacheData));
            }

            if (issuanceCode == VerificationStatus.PresentationError)
            {
                var cacheData = new
                {
                    status = "presentation_error",
                    payload = verificationStatusResponse.Error.Code,
                    message = verificationStatusResponse.Error.Message,
                    userId = data["userId"].ToString()

                };
                _cache.Set(verificationStatusResponse.State, JsonSerializer.Serialize(cacheData));
            }

            return verificationStatusResponse;
        }
    }
}
