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

        /// <summary>
        /// This endpoint is used to generate Debit note RRR for Depot's COQ 
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// post: api/application/generate-debit-note
        /// 
        /// </remarks>
        /// <param name="id">refers to the COQ Id generated for a specific Depot</param>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [HttpPost]
        [Route("generate-debit-note")]
        public async Task<IActionResult> GenerateDebitNote(int id) => Response(await _payment.GenerateDebitNote(id).ConfigureAwait(false));

        /// <summary>
        /// This endpoint is used to generate Demand notice RRR for a Depot defaulter
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// post: api/application/generate-demand-notice
        /// 
        /// </remarks>
        /// <param name="id">refers to the COQ Id generated for a specific Depot</param>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [HttpPost]
        [Route("generate-demand-notice")]
        public async Task<IActionResult> GenerateDemandNotice(int id) => Response(await _payment.GenerateDemandNotice(id).ConfigureAwait(false));

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

        /// <summary>
        /// This endpoint is used to update the status of any payment
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// post: api/application/update-payment-status
        /// 
        /// </remarks>
        /// <param name="orderId">refers to the payment reference used to generate the RRR</param>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [HttpPost]
        [Route("update-payment-status")]
        public async Task<IActionResult> UpdatePaymentStatus([FromForm] string orderId)
        {
            var payment = await _payment.ConfirmOtherPayment(orderId);
            return Redirect($"{_appSetting.LoginUrl}/paymentsum/{payment.Data}");
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


        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [HttpGet]
        [Route("All-payment")]

        public async Task<IActionResult> GetAllPayments() => Response(await _payment.GetAllPayments());



        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [HttpGet]
        [Route("PaymentById")]

        public async Task<IActionResult> GetPaymentsById(int id) => Response(await _payment.GetPaymentById(id));

    }
}
