using Bunkering.Access;
using Bunkering.Access.DAL;
using Bunkering.Access.IContracts;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using System.Drawing.Printing;
using System.Net;
using System.Security.Claims;

namespace Bunkering.Controllers.API
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class LicensesController : ResponseController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<ApplicationUser> _userManager;

        public LicensesController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
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
				Data = permits.Count() > 0 ? permits.OrderByDescending(d => d.IssuedDate).Select(x => new
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

		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		[Produces("application/json")]
		[Route("all_company_permits")]
		[HttpGet]
		public async Task<IActionResult> CompanyPermits()
		{
			var userId = User.Claims.First(x => x.Type == ClaimTypes.PrimarySid).Value;

            var permits = await _unitOfWork.Permit.Find(c => c.Application.UserId == userId, "Application.User.Company,Application.Facility.VesselType");

			return Ok(new ApiResponse
			{
				//using if statements here ?: to check conditions for the permit
				Message = permits.Count() > 0 ? "Success, Permit Found" : "Permit Not Found",
				StatusCode = permits.Count() > 0 ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
				Success = permits.Count() > 0 ? true : false,
				Data = permits.Count() > 0 ? permits.OrderByDescending(d => d.IssuedDate).Select(x => new
				{
					x.Id,
					x.ApplicationId,
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
				//var qrcode = Utils.GenerateQrCode($"{Request.Scheme}://{Request.Host}/License/ValidateQrCode/{license.ApplicationId}");
				//license.QRCode = Convert.ToBase64String(qrcode, 0, qrcode.Length);
				var depots = await _unitOfWork.ApplicationDepot.Find(x => x.AppId.Equals(license.ApplicationId), "Application.Histories,Depot,Product");
                NominatedSurveyor nominatedSurveyor  = null;
				if (depots.FirstOrDefault()?.Application.SurveyorId != null)
				{
					var surveyorId = depots.FirstOrDefault()?.Application.SurveyorId;

                    nominatedSurveyor = await _unitOfWork.NominatedSurveyor.FirstOrDefaultAsync(x => x.Id.Equals(surveyorId));
				}
				var  user = await _userManager.FindByIdAsync(depots?.FirstOrDefault()?
					.Application.Histories?
					.OrderByDescending(a => a.Date)
					.LastOrDefault()?.TargetedTo ?? string.Empty);

				string jetty = null;

                if (license.Application?.Jetty > 0)
				{
                    jetty = _unitOfWork.Jetty.Query().FirstOrDefault(x => x.Id == license.Application.Jetty)?.Name;
                }
              
                var viewAsPdf = new ViewAsPdf 
				{
					Model = new CertificareDTO
					{
						ETA = license.Application.ETA.Value,
						LoadPort = license.Application.LoadingPort,
						PermitNo = license.PermitNo,
						QRCode = license.QRCode,
						Vessel = license.Application.VesselName,
						Destinations = depots.Select(y => new DepotDTO
						{
							Name = $"{y.Depot.Name} ({y.Depot.State})",
							Product = y.Product.Name,
							Volume = y.Volume,
							DischargeId = y.DischargeId,
						}).ToList(),
						Jetty = jetty,
						Surveyor = nominatedSurveyor is not null ? nominatedSurveyor.Name : "N/A",
						Signature = user?.Signature ?? string.Empty,
						DateIssued = license.IssuedDate
                    },
					PageHeight = 327,
                    ViewName = "ViewLicense",
					PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
				};
				var pdf = await viewAsPdf.BuildFile(ControllerContext);
				return File(new MemoryStream(pdf), "application/pdf");
			}
			return BadRequest();
		}

        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("ValidateQrCode")]
        [HttpGet]
        public async Task<IActionResult> ValidateQrCode(int id) 
		{
            var license = await _unitOfWork.Permit.FirstOrDefaultAsync(x => x.ApplicationId.Equals(id), "Application.User.Company,Application.Facility.VesselType,Application.Payments");
            if (license != null)
            {
                var depots = await _unitOfWork.ApplicationDepot.Find(x => x.AppId.Equals(license.ApplicationId), "Application.Histories,Depot,Product");
                NominatedSurveyor nominatedSurveyor = null;
                if (depots.FirstOrDefault()?.Application.SurveyorId != null)
                {
                    var surveyorId = depots.FirstOrDefault()?.Application.SurveyorId;

                    nominatedSurveyor = await _unitOfWork.NominatedSurveyor.FirstOrDefaultAsync(x => x.Id.Equals(surveyorId));
                }
                var user = await _userManager.FindByIdAsync(depots?.FirstOrDefault()?
                    .Application.Histories?
                    .OrderByDescending(a => a.Date)
                    .LastOrDefault()?.TargetedTo ?? string.Empty);

                string jetty = null;

                if (license.Application?.Jetty > 0)
                {
                    jetty = _unitOfWork.Jetty.Query().FirstOrDefault(x => x.Id == license.Application.Jetty)?.Name;
                }

                var viewAsPdf = new ViewAsPdf
                {
                    Model = new CertificareDTO
                    {
                        ETA = license.Application.ETA.Value,
                        LoadPort = license.Application.LoadingPort,
                        PermitNo = license.PermitNo,
                        QRCode = license.QRCode,
                        Vessel = license.Application.VesselName,
                        Destinations = depots.Select(y => new DepotDTO
                        {
                            Name = y.Depot.Name,
                            Product = y.Product.Name,
                            Volume = y.Volume,
                            DischargeId = y.DischargeId,
                        }).ToList(),
                        Jetty = jetty,
                        Surveyor = nominatedSurveyor is not null ? nominatedSurveyor.Name : "N/A",
                        Signature = user?.Signature ?? string.Empty,
                        DateIssued = license.IssuedDate
                    },
                    PageHeight = 327,
                    PageMargins = new Rotativa.AspNetCore.Options.Margins(10, 10, 10, 10),
                    ViewName = "ViewLicense"
                };
                var pdf = await viewAsPdf.BuildFile(ControllerContext);
                return File(new MemoryStream(pdf), "application/pdf");
            }
            return BadRequest();
        }
    }
}
