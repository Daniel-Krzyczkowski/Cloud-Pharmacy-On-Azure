using CloudPharmacy.Physician.API.Application.Commands;
using CloudPharmacy.Physician.API.Application.DTO;
using CloudPharmacy.Physician.API.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudPharmacy.Physician.API.Controllers
{
    [Authorize(Policy = "Physician")]
    [Route("api/[controller]")]
    [ApiController]
    public class PhysicianController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PhysicianController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Get all available physicians
        /// </summary>
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("profiles")]
        public async Task<IActionResult> GetAllPhysiciansAsync()
        {
            var response = await _mediator.Send(new GetAllPhysiciansQuery());

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
        /// Gets physician's profile
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfileAsync()
        {
            var response = await _mediator.Send(new GetPhysicianProfileQuery());

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
        /// Updates physician profile
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfileAsync([FromForm] UpdatePhysicianProfileDTO updatePhysicianProfileDTO)
        {
            var response = await _mediator.Send(new UpdatePhysicianProfileCommand()
            {
                UpdatePhysicianProfileDTO = updatePhysicianProfileDTO
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
        /// Gets physician's free slots from schedule
        /// </summary>
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("schdule/free-slots/{physicianId}")]
        public async Task<IActionResult> GetAllFreeSlotsFromScheduleAsync([FromRoute] string physicianId)
        {
            var response = await _mediator.Send(new GetPhysicianFreeScheduleSlotsQuery()
            {
                PhysicianId = physicianId
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
        /// Gets physician's reserved slots from schedule
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("schdule/reserved-slots")]
        public async Task<IActionResult> GetAllReservedSlotsFromScheduleAsync()
        {
            var response = await _mediator.Send(new GetPhysicianReservedScheduleSlotsQuery());

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
        /// Adds new free slot to physician's schedule
        /// </summary>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("free-slot")]
        public async Task<IActionResult> AddFreeSlotToScheduleAsync([FromBody] PhysicianFreeScheduleSlotDTO physicianFreeScheduleSlotDTO)
        {
            var response = await _mediator.Send(new AddPhysicianNewScheduleSlotCommand()
            {
                PhysicianFreeScheduleSlotDTO = physicianFreeScheduleSlotDTO
            });

            if (response.CompletedWithSuccess)
            {
                return Ok();
            }

            else
            {
                return BadRequest(response.OperationError);
            }
        }

        /// <summary>
        /// Adds slot to physician's schedule for visit with patient
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("visit-slot")]
        public async Task<IActionResult> AddSlotToScheduleForVisitWithPatientAsync([FromBody] PhysicianScheduleSlotForPatientDTO
                                                                                                    physicianScheduleSlotForPatientDTO)
        {
            var response = await _mediator.Send(new AddPhysicianScheduleSlotForPatientCommand()
            {
                PhysicianScheduleSlotForPatientDTO = physicianScheduleSlotForPatientDTO
            });

            if (response.CompletedWithSuccess)
            {
                return Ok();
            }

            else
            {
                return BadRequest(response.OperationError);
            }
        }

        /// <summary>
        /// Deletes slot from physician's schedule
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("slot/{scheduleSlotId}")]
        public async Task<IActionResult> DeleteSlotFromScheduleAsync([FromRoute] string scheduleSlotId)
        {
            var response = await _mediator.Send(new RemoveScheduleSlotCommand()
            {
                ScheduleSlotId = scheduleSlotId
            });

            if (response.CompletedWithSuccess)
            {
                return Ok();
            }

            else
            {
                return BadRequest(response.OperationError);
            }
        }

        /// <summary>
        /// Generates new prescription for patient
        /// </summary>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("new-prescription")]
        public async Task<IActionResult> GenerateNewPrescriptionForPatientAsync([FromBody] NewPatientPrescriptionDTO newPatientPrescriptionDTO)
        {
            var response = await _mediator.Send(new AddNewPrescriptionForPatientCommand()
            {
                NewPatientPrescriptionDTO = newPatientPrescriptionDTO
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
    }
}
