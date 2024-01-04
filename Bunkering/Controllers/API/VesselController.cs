using Bunkering.Access.Services;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VesselController : ResponseController
    {
        private readonly VesselService _vesselService;
        public VesselController(VesselService vesselService)
        {
            _vesselService = vesselService;
        }

        /// <summary>
        /// This endpoint is used to fetch  all the Fees summary
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/application/get-all-fees
        /// 
        /// </remarks>
        /// <response code="200">Returns the fees  </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        /// 

        [Route("edit-vessel-IMO/{name, imo}")]
        [HttpPut]
        public async Task<IActionResult> EditIMO(string name, string newImo) => Response(await _vesselService.EditIMONoByName(name, newImo));

    }
}
