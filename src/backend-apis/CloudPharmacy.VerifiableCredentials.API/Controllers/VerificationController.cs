using CloudPharmacy.VerifiableCredentials.API.Application.DTO;
using CloudPharmacy.VerifiableCredentials.API.Application.Model;
using CloudPharmacy.VerifiableCredentials.API.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CloudPharmacy.VerifiableCredentials.API.Controllers
{
    [Authorize(Policy = "Verifiable-Credentials-Access")]
    [Route("api/[controller]")]
    [ApiController]
    public class VerificationController : ControllerBase
    {
        private readonly ILogger<VerificationController> _logger;
        private readonly IVerifiableCredentialsManagementService _verifiableCredentialsManagementService;
        private IVerifiableCredentialStatusNotificationService _verifiableCredentialStatusNotificationService;

        public VerificationController(ILogger<VerificationController> logger,
                          IVerifiableCredentialsManagementService verifiableCredentialsManagementService,
                          IVerifiableCredentialStatusNotificationService verifiableCredentialStatusNotificationService)
        {
            _logger = logger;
            _verifiableCredentialsManagementService = verifiableCredentialsManagementService;
            _verifiableCredentialStatusNotificationService = verifiableCredentialStatusNotificationService;
        }

        [HttpPost("presentation-request")]
        public async Task<IActionResult> GeneratePresentationRequestAsync([FromBody] PatientProfileDataForVerifiableCredentialDTO patientDTO)
        {
            if (patientDTO == null
                || string.IsNullOrEmpty(patientDTO.PatientId)
                || string.IsNullOrEmpty(patientDTO.FirstNameAndLastName))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Patient data should contain identifier, first name, and last name.");
            }

            var verificationResponse = await _verifiableCredentialsManagementService.VerifyExistingVerifiableCredentialAsync(patientDTO);
            if (verificationResponse != null)
            {
                return new OkObjectResult(verificationResponse);
            }
            return StatusCode((int)HttpStatusCode.InternalServerError, "There is an issue with service. Please contact administrator if problem remains.");
        }
        [AllowAnonymous]
        [HttpPost("presentation-callback")]
        public async Task<ActionResult> PresentationCallback()
        {
            string presentationVerificationStatusResponseAsString = await new StreamReader(Request.Body).ReadToEndAsync();
            var presentationVerificationStatus = await _verifiableCredentialsManagementService.VerifyPresentationStatusAsync(presentationVerificationStatusResponseAsString);

            if (presentationVerificationStatus.Code == VerificationStatus.RequestOpenedInAuthenticatorApp ||
                presentationVerificationStatus.Code == VerificationStatus.VerifiableCredentialSuccessfullyPresented)
            {
                await _verifiableCredentialStatusNotificationService
                                        .SendVerifiableCredentialVerificationStatusUpdateAsync(presentationVerificationStatus);

                return new OkResult();
            }


            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "There is an issue with service. Please contact administrator if problem remains.");
            }
        }
    }
}
