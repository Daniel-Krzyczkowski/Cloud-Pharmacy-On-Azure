using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CloudPharmacy.LiveNotifications.FuncApp
{
    public class VerifiableCredentialsNotificationFuncApps
    {
        private readonly ILogger<VerifiableCredentialsNotificationFuncApps> _logger;

        public VerifiableCredentialsNotificationFuncApps(ILogger<VerifiableCredentialsNotificationFuncApps> log)
        {
            _logger = log;
        }

        [FunctionName("negotiate")]
        public static SignalRConnectionInfo GetSignalRInfo(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST")] HttpRequest req,
        [SignalRConnectionInfo(HubName = "vcnotifications", UserId = "{headers.x-ms-client-principal-id}")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        [FunctionName("send-vc-issuance-status-notification")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task SendVcIssuanceStatusNotificationAsync(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = null)] HttpRequest notificationRequest,
            [SignalR(HubName = "vcnotifications")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            _logger.LogInformation("send-vc-issuance-status-notification function app was triggered");
            var userId = notificationRequest.Headers["x-ms-client-principal-id"].ToString();
            StreamReader reader = new StreamReader(notificationRequest.Body);
            var notificationAsJson = await reader.ReadToEndAsync();

            await signalRMessages.AddAsync(
                                        new SignalRMessage
                                        {
                                            UserId = userId,
                                            Target = "vc-issuance-status-update",
                                            Arguments = new[] { notificationAsJson }
                                        });
        }


        [FunctionName("send-vc-verification-status-notification")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
        public async Task SendVcPresentationStatusNotificationAsync(
            [HttpTrigger(AuthorizationLevel.Function, "POST", Route = null)] HttpRequest notificationRequest,
            [SignalR(HubName = "vcnotifications")] IAsyncCollector<SignalRMessage> signalRMessages)
        {
            _logger.LogInformation("send-vc-verification-status-notification");
            var userId = notificationRequest.Headers["x-ms-client-principal-id"].ToString();
            StreamReader reader = new StreamReader(notificationRequest.Body);
            var notificationAsJson = await reader.ReadToEndAsync();

            await signalRMessages.AddAsync(
              new SignalRMessage
              {
                  UserId = userId,
                  Target = "vc-verification-status-update",
                  Arguments = new[] { notificationAsJson }
              });
        }
    }
}

