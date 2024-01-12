using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bunkering.Controllers
{
	public class ResponseController : Controller
	{
		protected new IActionResult Response(ApiResponse response)
		{
			if (response?.StatusCode == HttpStatusCode.OK)
				return Ok(response);

			if (response.StatusCode == HttpStatusCode.BadRequest)
				return BadRequest(response);

			if (response.StatusCode == HttpStatusCode.NotFound)
				return NotFound(response);

			if (response.StatusCode == HttpStatusCode.Conflict)
				return Conflict(response);

			return NotFound(response);
		}
	}
}
