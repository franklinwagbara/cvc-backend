using Bunkering.Access.Services;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bunkering.Controllers.API
{
	[Authorize]
	[ApiController]
	[Route("api/bunkering/[controller]")]
	public class SchedulesController : ResponseController
	{
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly ScheduleService _schedules;

		public SchedulesController(
			IHttpContextAccessor httpContextAccessor,
			ScheduleService schedules)
		{
			_contextAccessor = httpContextAccessor;
			_schedules = schedules;
		}

		/// <summary>
		/// This endpoint is used to schedule an inspection for an application
		/// </summary>
		/// <returns>Returns a success message or otherwise</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// POST: api/application/add-schedule
		/// 
		/// </remarks>
		/// <param name="model">This is the schedule object for an application </param>
		/// <response code="200">Returns an application info </response>
		/// <response code="404">Returns not found </response>
		/// <response code="401">Unauthorized user </response>
		/// <response code="400">Internal server error - bad request </response>
		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		//[Produces("application/json")]
		[Route("add-schedule")]
		[HttpPost]
		public async Task<IActionResult> ScheduleInspection(ScheduleViewModel model) =>

			Response(await _schedules.ScheduleInspection(model));

		/// <summary>
		/// This endpoint is used to fetch a schedule for an application
		/// </summary>
		/// <returns>Returns a success message or rotherwise</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// GET: api/application/get-schedule
		/// 
		/// </remarks>
		/// <param name="id">This is the id of the schedule</param>
		/// <response code="200">Returns an application info </response>
		/// <response code="404">Returns not found </response>
		/// <response code="401">Unauthorized user </response>
		/// <response code="400">Internal server error - bad request </response>
		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		[Produces("application/json")]
		[Route("get-schedule")]
		[HttpGet]
		public async Task<IActionResult> GetSchedule(int id) => Response(await _schedules.GetSchedule(id));

		/// <summary>
		/// This endpoint is used to approve or rejct a schedule for an application
		/// </summary>
		/// <returns>Returns a success message or rotherwise</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// POST: api/application/approve-schedule
		/// 
		/// </remarks>
		/// <param name="model">This is the object used to approve or reject the schedule</param>
		/// <response code="200">Returns an application info </response>
		/// <response code="404">Returns not found </response>
		/// <response code="401">Unauthorized user </response>
		/// <response code="400">Internal server error - bad request </response>
		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		[Produces("application/json")]
		[Route("approve-schedule")]
		[HttpPost]
		public async Task<IActionResult> ApproveSchedule(ScheduleViewModel model)
		{
			if (ModelState.IsValid)
				return Response(await _schedules.ApproveSchedule(model));
			else
				return BadRequest(new
				{
					Message = "The approval object model is invalid",
					StatusCode = HttpStatusCode.BadRequest,
					Success = false
				});
		}

		/// <summary>
		/// This endpoint is used to fetch all schedules per logged-in user
		/// </summary>
		/// <returns>Returns a success message or rotherwise</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// GET: api/application/all-schedules
		/// 
		/// </remarks>
		/// <response code="200">Returns a list of schedules </response>
		/// <response code="404">Returns not found </response>
		/// <response code="401">Unauthorized user </response>
		/// <response code="400">Internal server error - bad request </response>
		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		[Produces("application/json")]
		[Route("all-schedules")]
		[HttpGet]
		public async Task<IActionResult> Schedules() => Response(await _schedules.Schedules());

		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		[Produces("application/json")]
		[Route("accept-schedule")]
		[HttpPost]

		public async Task<IActionResult> AcceptSchedule(ScheduleViewModel model) => Response(await _schedules.AcceptSchedule(model));
	}
}
