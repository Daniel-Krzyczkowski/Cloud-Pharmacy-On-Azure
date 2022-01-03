using CloudPharmacy.PharmacyStore.API.Application.Commands;
using CloudPharmacy.PharmacyStore.API.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudPharmacy.PharmacyStore.API.Controllers
{
    [Authorize(Policy = "Customer")]
    [Route("api/[controller]")]
    [ApiController]
    public class MedicamentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MedicamentController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Get all available medicaments
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAvailableMedicamentsAsync()
        {
            var response = await _mediator.Send(new GetMedicamentsQuery());

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
        /// Verify patient's profile with Verifiable Credential
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("verify/patient/verifiable-credential")]
        public async Task VerifyPatientCredentialAsync()
        {
            await _mediator.Send(new VerifyPatientCredentialCommand()
            {
                HttpResponse = Response
            });
        }
    }
}
