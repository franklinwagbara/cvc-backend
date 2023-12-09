using Bunkering.Access.Services;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class ApplicationProcessesController : ResponseController
	{
		private readonly AppProvessesService _appProcService;

		public ApplicationProcessesController(AppProvessesService appProcService)
		{
			_appProcService = appProcService;
		}

		/// <summary>
		/// This endpoint is used to fetch all configured processes
		/// </summary>
		/// <returns>Returns a success message or otherwise</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// GET: api/application/get-processes
		/// 
		/// </remarks>
		/// <response code="200">Returns an application info </response>
		/// <response code="404">Returns not found </response>
		/// <response code="401">Unauthorized user </response>
		/// <response code="400">Internal server error - bad request </response>
		[Route("Get-Processes")]
		[HttpGet]
		public async Task<IActionResult> Index() => Response(await _appProcService.Index());

		/// <summary>
		/// This endpoint is used to add new flow to the system
		/// </summary>
		/// <returns>Returns a success message or otherwise</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// POST: api/application/add-flow
		/// 
		/// </remarks>
		/// <param name="model">This is the model for adding a new flow</param>
		/// <response code="200">Returns a success message </response>
		/// <response code="404">Returns not found </response>
		/// <response code="401">Unauthorized user </response>
		/// <response code="400">Internal server error - bad request </response>
		[Route("Add-Flow")]
		[HttpPost]
		public async Task<IActionResult> AddFlow(WorkFlow model) => Response(await _appProcService.AddFlow(model));

		/// <summary>
		/// This endpoint is used to edit an exisiting flow
		/// </summary>
		/// <returns>Returns a success message or otherwise</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// POST: api/application/edit-flow
		/// 
		/// </remarks>
		/// <param name="model">This is the model for editing a new flow</param>
		/// <response code="200">Returns a success message </response>
		/// <response code="404">Returns not found </response>
		/// <response code="401">Unauthorized user </response>
		/// <response code="400">Internal server error - bad request </response>
		[Route("Edit-Flow")]
		[HttpPost]
		public async Task<IActionResult> EditFlow(WorkFlow model) => Response(await _appProcService.EditFlow(model));

		/// <summary>
		/// This endpoint is used to clone an exisiting flow
		/// </summary>
		/// <returns>Returns a success message or otherwise</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// GET: api/application/clone-flow
		/// 
		/// </remarks>
		/// <param name="id">This is the id o fthe processes to be cloned</param>
		/// <response code="200">Returns a success message </response>
		/// <response code="404">Returns not found </response>
		/// <response code="401">Unauthorized user </response>
		/// <response code="400">Internal server error - bad request </response>
		[Route("Clone-Flow")]
		[HttpGet]
		public async Task<IActionResult> CloneFlow(int id) => Response(await _appProcService.CloneFlow(id));

		/// <summary>
		/// This endpoint is used to Archive an exisiting flow
		/// </summary>
		/// <returns>Returns a success message or otherwise</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// GET: api/application/Archive-flow
		/// 
		/// </remarks>
		/// <param name="id">This is the id o fthe processes to be cloned</param>
		/// <response code="200">Returns a success message </response>
		/// <response code="404">Returns not found </response>
		/// <response code="401">Unauthorized user </response>
		/// <response code="400">Internal server error - bad request </response>
		[Route("Archive-Flow")]
		[HttpDelete]

		public async Task<IActionResult> ArchiveFlow(int id) => Response(await _appProcService.ArchiveFlow(id));
	}
}
