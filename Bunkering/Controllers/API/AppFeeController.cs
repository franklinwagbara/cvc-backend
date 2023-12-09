using Bunkering.Access.Services;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
    
	//[Authorize]
	[Route("api/[controller]")]
    [ApiController]
    public class AppFeeController : ResponseController
    {
        private readonly AppFeeService _appFeeService;
        public AppFeeController(AppFeeService appFeeService)
        {
            _appFeeService = appFeeService;
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
		[AllowAnonymous]
        [Route("get-all-fees")]
        [HttpGet]
        public async Task<IActionResult> GetAllFees() => Response(await _appFeeService.GetAllFees());

        /// <summary>
        /// This endpoint is used to add new fee
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/AppFee/add-fee
        /// 
        /// </remarks>
        /// <response code="200">Returns success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        /// 
        [Route("add-fee")]
        [HttpPost]
        public async Task<IActionResult> AddFee(AppFee model, ApplicationUser user) => Response(await _appFeeService.CreateFee(model, user));


        /// <summary>
        /// This endpoint is used to Edit new fee
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/AppFee/edit-fee
        /// 
        /// </remarks>
        /// <response code="200">Returns success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        /// 
        [Route("edit-fee")]
        [HttpPost]
        public async Task<IActionResult> EditFee(AppFee model, ApplicationUser user) => Response(await _appFeeService.EditFee(model, user));


        /// <summary>
        /// This endpoint is used to Get fee by Id
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/AppFee/get-fee-byId
        /// 
        /// </remarks>
        /// <response code="200">Returns success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        /// 
        [Route("get-fee-byId")]
        [HttpPost]
        public async Task<IActionResult> GetFeeById(int id) => Response(await _appFeeService.GetFeeByID(id));


        /// <summary>
        /// This endpoint is used to Delete fee by Id
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/AppFee/delete-fee
        /// 
        /// </remarks>
        /// <response code="200">Returns success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        /// 
        [Route("delete-fee")]
        [HttpPost]
        public async Task<IActionResult> DeleteFee(int id) => Response(await _appFeeService.DeleteFee(id));

    }
}
