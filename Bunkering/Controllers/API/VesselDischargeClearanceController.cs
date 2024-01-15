using Bunkering.Access.Services;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class VesselDischargeClearanceController : ResponseController
    {
        private readonly VesselDischargeClearanceService _vesselDischargeClearanceService;

        public VesselDischargeClearanceController(VesselDischargeClearanceService vesselDischargeClearanceService)
        {
            _vesselDischargeClearanceService = vesselDischargeClearanceService;
        }


        /// <summary>
        /// This endpoint is used to create vessel discharge clearance 
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/email/create-vessel-discharge-clearance
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("create-vessel-discharge-clearance")]
        [HttpPost]

        public async Task<IActionResult> CreateVesselDischargeClearance(VesselDischargeCleareanceViewModel model) => Response(await _vesselDischargeClearanceService.CreateVesselDischargeClearance(model));



        /// <summary>
        /// This endpoint is used to fetch vessel discharge clearance by id
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/vessel-discharge-clearance/{id}
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("vessel-discharge-clearance/{id}")]
        [HttpGet]

        public async Task<IActionResult> GetAllVesselDischargeClearance() => Response(await _vesselDischargeClearanceService.GetAllVesselDischargeClearance());

    }
}
