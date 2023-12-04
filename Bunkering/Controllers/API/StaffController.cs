using Bunkering.Access.Services;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
	//[Authorize]
	[Route("api/bunkering/[controller]")]
	[ApiController]
	public class StaffController : ResponseController
	{
		private readonly StaffService _staffService;

		public StaffController(StaffService staffService)
		{
			_staffService = staffService;
		}

		/// <summary>
		/// This endpoint is used to fetch the satff dashbaord summary
		/// </summary>
		/// <returns>Returns a success message or otherwise</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// GET: api/application/get-dashboard
		/// 
		/// </remarks>
		/// <response code="200">Returns the staff dashboard </response>
		/// <response code="404">Returns not found </response>
		/// <response code="401">Unauthorized user </response>
		/// <response code="400">Internal server error - bad request </response>
		[Route("get-dashboard")]
		[HttpGet]
		public async Task<IActionResult> Dashboard() => Response(await _staffService.Dashboard());

		/// <summary>
		/// This endpoint is used to fetch all staff
		/// </summary>
		/// <returns>Returns a success message or otherwise</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// GET: api/staff/all-users
		/// 
		/// </remarks>
		/// <response code="200">Returns staff list </response>
		/// <response code="404">Returns not found </response>
		/// <response code="401">Unauthorized user </response>
		/// <response code="400">Internal server error - bad request </response>
		[AllowAnonymous]
		[Route("all-users")]
		[HttpGet]
		public async Task<IActionResult> AllUsers() => Response(await _staffService.AllUsers());

		/// <summary>
		/// This endpoint is used to add new user
		/// </summary>
		/// <returns>Returns a success message or otherwise</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// POST: api/staff/add-user
		/// 
		/// </remarks>
		/// <response code="200">Returns success message </response>
		/// <response code="404">Returns not found </response>
		/// <response code="401">Unauthorized user </response>
		/// <response code="400">Internal server error - bad request </response>
		/// 


		[Route("get-staffby-elpsId")]
		[HttpGet]

		public async Task<IActionResult> GetAllStaff() => Response(await _staffService.AllElpsStaff());
		/// <summary>
		/// This endpoint is used to add new user
		/// </summary>
		/// <returns>Returns a success message or otherwise</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// POST: api/staff/add-user
		/// 
		/// </remarks>
		/// <response code="200">Returns success message </response>
		/// <response code="404">Returns not found </response>
		/// <response code="401">Unauthorized user </response>
		/// <response code="400">Internal server error - bad request </response>
		/// 
		[Route("add-user")]
		[HttpPost]
		public async Task<IActionResult> AddUsers(UserViewModel model) => Response(await _staffService.Create(model));


		/// <summary>
		/// This endpoint is used to edit user
		/// </summary>
		/// <returns>Returns a success message or otherwise</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// POST: api/staff/edit-user
		/// 
		/// </remarks>
		/// <response code="200">Returns success message </response>
		/// <response code="404">Returns not found </response>
		/// <response code="401">Unauthorized user </response>
		/// <response code="400">Internal server error - bad request </response>
		[Route("edit-user")]
		[HttpPost]
		public async Task<IActionResult> Edit(UserViewModel model) => Response(await _staffService.Edit(model));


		/// <summary>
		/// This endpoint is used to Deactivate a user
		/// </summary>
		/// <returns>Returns a success message or otherwise</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// POST: api/staff/deactivate-user
		/// 
		/// </remarks>
		/// <response code="200">Returns success message </response>
		/// <response code="404">Returns not found </response>
		/// <response code="401">Unauthorized user </response>
		/// <response code="400">Internal server error - bad request </response>
		[Route("Delete-User")]
		[HttpDelete]

		public async Task<IActionResult> DeleteStaff(string id) => Response(await _staffService.DeleteUser(id));
	}
}
