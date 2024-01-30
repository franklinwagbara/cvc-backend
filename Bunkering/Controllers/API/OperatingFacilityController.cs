using Bunkering.Access.DAL;
using Bunkering.Access.Services;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OperatingFacilityController : ResponseController
    {
        private readonly OperatingFacilityService _operatingFacility;

        public OperatingFacilityController(OperatingFacilityService operatingFacility)
        {
            _operatingFacility = operatingFacility;
        }


        /// <summary>
        /// This endpoint is used to fetch all operating facilities
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/operatingfacility/all-operating-facilities
        /// 
        /// </remarks>
        /// <response code="200">Returns staff list </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [AllowAnonymous]
        [Route("all-operating-facilities")]
        [HttpGet]
        public async Task<IActionResult> AllOperatingFacilities() => Response(await _operatingFacility.AllOperatingFacilities());


        /// <summary>
        /// This endpoint is used to fetch create operating facility
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/operatingfacility/create
        /// 
        /// </remarks>
        /// <response code="200">Returns staff list </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [AllowAnonymous]
        [Route("create-operating-facility")]
        [HttpPost]
        public async Task<IActionResult> CreateOperatingFacility(OpearatingFacilityViewModel model) => Response(await _operatingFacility.CreateOperatingFacility(model));

        /// <summary>
        /// This endpoint is used to fetch edit operating facility
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/operatingfacility/edit
        /// 
        /// </remarks>
        /// <response code="200">Returns staff list </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [AllowAnonymous]
        [Route("edit-operating-facility")]
        [HttpPost]
        public async Task<IActionResult> EditOperatingFacility(OpearatingFacilityViewModel model) => Response(await _operatingFacility.EditOperatingFacility(model));
    }
}
