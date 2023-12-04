using Bunkering.Access;
using Bunkering.Access.IContracts;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using System.Net;

namespace Bunkering.Controllers.API
{
	[Authorize]
	[Route("api/bunkering/[controller]")]
	[ApiController]
	public class LicensesController : ResponseController
	{
		private readonly IUnitOfWork _unitOfWork;

		public LicensesController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}


		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		[Produces("application/json")]
		[Route("all_permits")]
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var permits = await _unitOfWork.Permit.GetAll("Application.User.Company,Application.Facility.VesselType");

			return Ok(new ApiResponse
			{
				//using if statements here ?: to check conditions for the permit
				Message = permits.Count() > 0 ? "Success, Permit Found" : "Permit Not Found",
				StatusCode = permits.Count() > 0 ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
				Success = permits.Count() > 0 ? true : false,
				Data = permits.Count() > 0 ? permits.Select(x => new
				{
					x.Id,
					CompanyName = x.Application.User.Company.Name,
					LicenseNo = x.PermitNo,
					IssuedDate = x.IssuedDate.ToString("MMM dd, yyyy HH:mm:ss"),
					ExpiryDate = x.ExpireDate.ToString("MMM dd, yyyy HH:mm:ss"),
					x.Application.User.Email,
					VesselTypeType = x.Application.Facility.VesselType.Name,
					VesselName = x.Application.Facility.Name,
				}) : new { }
			});

		}

		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		[Produces("application/json")]
		[Route("view_license")]
		[HttpGet]
		public async Task<IActionResult> ViewLicense(int id)
		{
			var license = await _unitOfWork.Permit.FirstOrDefaultAsync(x => x.Id.Equals(id), "Application.User.Company,Application.Facility.VesselType,Application.Payments");
			if (license != null)
			{
				var qrcode = Utils.GenerateQrCode($"{Request.Scheme}://{Request.Host}/License/ValidateQrCode/{license.ApplicationId}");
				license.QRCode = Convert.ToBase64String(qrcode, 0, qrcode.Length);
				var viewAsPdf = new ViewAsPdf
				{
					Model = license,
					PageHeight = 327,
					ViewName = "ViewLicense"
				};
				var pdf = await viewAsPdf.BuildFile(ControllerContext);
				return File(new MemoryStream(pdf), "application/pdf");
			}
			return BadRequest();
		}
	}
}
