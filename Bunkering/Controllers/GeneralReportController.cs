using Bunkering.Access.IContracts;
using Bunkering.Access.Services;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers
{
	//[Authorize]
	[ApiController]
	[Route("api/bunkering/[controller]")]
	public class GeneralReportController : ResponseController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly PaymentService _paymentService;
		private readonly LicenseService _licenseService;
		private readonly AppService _appService;
		public GeneralReportController(IUnitOfWork unitOfWork, PaymentService paymentService, LicenseService licenseService, AppService appService)
		{
			_unitOfWork = unitOfWork;
			_paymentService = paymentService;
			_licenseService = licenseService;
			_appService = appService;

		}



		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		[Produces("application/json")]
		[Route("application_report")]
		[HttpPost]

		public async Task<IActionResult> ApplicationReport(ApplicationReportViewModel model) => Response(await _appService.ApplicationReport(model));



		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		[Produces("application/json")]
		[Route("license_report")]
		[HttpPost]

		public async Task<IActionResult> LicenseReport(LicenseReportViewModel model) => Response(await _licenseService.LicenseReport(model));


		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		[Produces("application/json")]
		[Route("payment_report")]
		[HttpPost]

		public async Task<IActionResult> PaymentReport(PaymentReportViewModel model) => Response(await _paymentService.PaymentReport(model));





	}
}
