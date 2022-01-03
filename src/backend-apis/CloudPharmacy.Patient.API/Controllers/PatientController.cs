using CloudPharmacy.Patient.API.Application.Commands;
using CloudPharmacy.Patient.API.Application.DTO;
using CloudPharmacy.Patient.API.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudPharmacy.Patient.API.Controllers
{
    [Authorize(Policy = "Patient")]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PatientController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Gets patient's profile
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfileAsync()
        {
            var response = await _mediator.Send(new GetPatientProfileQuery());

            if (response.CompletedWithSuccess)
            {
                return Ok(response.Result);
            }

            else
            {
                return BadRequest(response.OperationError);
            }
        }

        /// <summary>
        /// Updates patient profile
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfileAsync([FromBody] UpdatePatientProfileDTO updatePatientProfileDTO)
        {
            var response = await _mediator.Send(new UpdatePatientProfileCommand()
            {
                UpdatePatientProfileDTO = updatePatientProfileDTO
            });

            if (response.CompletedWithSuccess)
            {
                return Ok(response.Result);
            }

            else
            {
                return BadRequest(response.OperationError);
            }
        }

        /// <summary>
        /// Get patient's prescriptions
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("prescriptions")]
        public async Task<IActionResult> GetPrescriptionsAsync()
        {
            var response = await _mediator.Send(new GetPatientPrescriptionsQuery());

            if (response.CompletedWithSuccess)
            {
                return Ok(response.Result);
            }

            else
            {
                return BadRequest(response.OperationError);
            }
        }

        /// <summary>
        /// Issue Verifiable Credential for patient
        /// </summary>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("issue/verifiable-credential")]
        public async Task VerifyPatientCredentialAsync()
        {
            await _mediator.Send(new IssuePatientCredentialCommand()
            {
                HttpResponse = Response
            });
        }
    }
}
