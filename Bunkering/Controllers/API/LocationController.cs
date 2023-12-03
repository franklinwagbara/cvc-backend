using Bunkering.Access.Services;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;

namespace Bunkering.Controllers.API
{
	[AllowAnonymous]
	[ApiController]
	[Route("api/bunkering/[controller]")]
	public class LocationController : ResponseController
	{
		private readonly LocationService _locationService;
		private readonly OfficeService _officeService;


		public LocationController(LocationService locationService, OfficeService officeService)
		{
			_locationService = locationService;
			_officeService = officeService;


		}

		/// <summary>
		/// This endpoint is used to create location
		/// </summary>
		/// <returns>Returns a success message</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// GET: api/location/location
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
		[Route("create-locations")]
		[HttpPost]

		public async Task<IActionResult> CreateLocation(LocationViewModel model) => Response(await _locationService.CreateLocation(model));

		/// <summary>
		/// This endpoint is used to edit location
		/// </summary>
		/// <returns>Returns a success message</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// GET: api/location/location
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
		[Route("edit-locations")]
		[HttpPost]

		public async Task<IActionResult> EditLocation(LocationViewModel model) => Response(await _locationService.EditLocation(model));


		/// <summary>
		/// This endpoint is used to create Office
		/// </summary>
		/// <returns>Returns a success message</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// GET: api/location/location
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
		[Route("create-office")]
		[HttpPost]

		public async Task<IActionResult> CreateOffcie(OfficeViewModel model) => Response(await _officeService.CreateOffice(model));


		/// <summary>
		/// This endpoint is used to edit Office
		/// </summary>
		/// <returns>Returns a success message</returns>
		/// <remarks>
		/// 
		/// Sample Request
		/// GET: api/location/location
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
		[Route("edit-office")]
		[HttpPost]

		public async Task<IActionResult> EditOffice(OfficeViewModel model) => Response(await _officeService.EditOffice(model));
	}
}
