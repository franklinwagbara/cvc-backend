using Bunkering.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bunkering.Access;
using Bunkering.Core.ViewModels;
using Bunkering.Access.Services;
using Bunkering.Core.Utils;
using Microsoft.Extensions.Options;

namespace Bunkering.Controllers.API
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ResponseController
	{
		private readonly PaymentService _payment;
		private readonly AppSetting _appSetting;

		public PaymentController(PaymentService payment, IOptions<AppSetting> appSetting)
		{
			_payment = payment;
			_appSetting = appSetting.Value;
		}

		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		[Produces("application/json")]
		[HttpPost]
		[Route("create-payment")]
		public async Task<IActionResult> CreatePayment(int id) => Response(await _payment.CreatePayment(id).ConfigureAwait(false));

		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		[Produces("application/json")]
		[HttpGet]
		[Route("pay-online")]
		public IActionResult PayOnline(string rrr) =>
			Redirect($"{_appSetting.ElpsUrl}/Payment/Pay?rrr={rrr}");

		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		[Produces("application/json")]
		[HttpPost]
		[Route("remita")]
		public async Task<IActionResult> Remita(int id, [FromForm] string status, [FromForm] string statuscode, [FromForm] string orderId, [FromForm] string RRR)
		{
			var payment = await _payment.ConfirmPayment(id, orderId);
			return Redirect($"{_appSetting.LoginUrl}/paymentsum/{id}");
		}


		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		[Produces("application/json")]
		[HttpGet]
		[Route("confirm-payment")]

		public async Task<IActionResult> ConfirmPayment(int Id, string OrderId) => Response(await _payment.ConfirmPayment(Id, OrderId));

	}
}
