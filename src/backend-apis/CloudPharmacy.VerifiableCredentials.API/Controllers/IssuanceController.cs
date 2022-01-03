using CloudPharmacy.VerifiableCredentials.API.Application.DTO;
using CloudPharmacy.VerifiableCredentials.API.Application.Model;
using CloudPharmacy.VerifiableCredentials.API.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CloudPharmacy.VerifiableCredentials.API.Controllers
{
    [Authorize(Policy = "Verifiable-Credentials-Access")]
    [ApiController]
    [Route("api/[controller]")]
    public class IssuanceController : ControllerBase
    {
        private readonly ILogger<IssuanceController> _logger;
        private readonly IVerifiableCredentialsManagementService _verifiableCredentialsManagementService;
        private IVerifiableCredentialStatusNotificationService _verifiableCredentialStatusNotificationService;

        public IssuanceController(ILogger<IssuanceController> logger,
                                  IVerifiableCredentialsManagementService verifiableCredentialsManagementService,
                                  IVerifiableCredentialStatusNotificationService verifiableCredentialStatusNotificationService)
        {
            _logger = logger;
            _verifiableCredentialsManagementService = verifiableCredentialsManagementService;
            _verifiableCredentialStatusNotificationService = verifiableCredentialStatusNotificationService;
        }

        [HttpPost("issuance-request")]
        public async Task<IActionResult> GenerateIssuanceRequestAsync([FromBody] PatientProfileDataForVerifiableCredentialDTO patientDTO)
        {
            if (patientDTO == null
              || string.IsNullOrEmpty(patientDTO.PatientId)
              || string.IsNullOrEmpty(patientDTO.FirstNameAndLastName))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Patient data should contain identifier, first name, and last name.");
            }

            var issuanceResponse = await _verifiableCredentialsManagementService.IssueNewVerifiableCredentialAsync(patientDTO);
            if (issuanceResponse != null)
            {
                return new OkObjectResult(issuanceResponse);
            }
            return StatusCode((int)HttpStatusCode.InternalServerError, "There is an issue with service. Please contact administrator if problem remains.");
        }
        [AllowAnonymous]
        [HttpPost("issuance-callback")]
        public async Task<ActionResult> IssuanceCallback()
        {
            string issuanceStatusResponseAsString = await new StreamReader(Request.Body).ReadToEndAsync();
            var issuanceStatus = await _verifiableCredentialsManagementService.VerifyIssuanceStatusAsync(issuanceStatusResponseAsString);

            if (issuanceStatus.Code == IssuanceStatus.QrCodeScannedByUser ||
                issuanceStatus.Code == IssuanceStatus.VerifiableCredentialSuccessfullyIssued)
            {
                await _verifiableCredentialStatusNotificationService
                                        .SendVerifiableCredentialIssuanceStatusUpdateAsync(issuanceStatus);
                return new OkResult();
            }


            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "There is an issue with service. Please contact administrator if problem remains.");
            }
        }
    }
}