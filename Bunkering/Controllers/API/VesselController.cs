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
       private readonly AppService _appService;


       public  VesselController(AppService appService)
        {
            _appService = appService;
        }

        /// <summary>
        /// This endpoint is used to fetch verified IMO numbers  
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// post: api/vessel/verifyIMONumbers
        /// 
        /// </remarks>
        /// <param name="model">Model for applying for a bunker application</param>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("verify-IMO-numbers")]
        [HttpGet]

        public async Task<IActionResult> IMONumberVerification(string imoNumber) => Response(await _appService.IMONumberVerification(imoNumber));
    }
}
