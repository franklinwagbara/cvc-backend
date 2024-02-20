using Bunkering.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bunkering.Access;
using Bunkering.Core.ViewModels;
using Bunkering.Access.Services;
using Bunkering.Core.Utils;
using Microsoft.Extensions.Options;
using System.Net;
using Bunkering.Access.IContracts;

namespace Bunkering.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ResponseController
    {
        private readonly PaymentService _payment;
        private readonly AppSetting _appSetting;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentController(PaymentService payment, IOptions<AppSetting> appSetting, IUnitOfWork unitOfWork)
        {
            _payment = payment;
            _appSetting = appSetting.Value;
            _unitOfWork = unitOfWork;
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
            return Redirect($"{_appSetting.LoginUrl}/company/paymentsum/{id}");
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


        /// <summary>
        /// This endpoint is used to fetch all Debit notes for a NOA/CVC application
        /// </summary>
        /// <returns>Returns a list of Debit notes for an application</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/application/get-debit-notes-by-appid
        /// 
        /// </remarks>
        /// <response code="200">Returns a list of Debit notes for an application </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("get-debit-notes-by-appid")]
        [HttpGet]
        public async Task<IActionResult> GetDebitNotes(int Id) => Response(await _payment.GetDebitNotesByAppId(Id));

        /// <summary>
        /// This endpoint is used to fetch all pending payments for a NOA/CVC application
        /// </summary>
        /// <returns>Returns a list of pending payments for an application</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/application/get-pending-payments-by-coqid
        /// 
        /// </remarks>
        /// <response code="200">Returns a list of pending payments for an application </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("get-pending-payments-by-id")]
        [HttpGet]
        public async Task<IActionResult> GetPendingPaymentsByAppId(int Id) => Response(await _payment.GetPendingPaymentsById(Id));


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

        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [HttpGet]
        [Route("create-debit-note-rrr")]
        public async Task<IActionResult> CreateDebitNoteRRRById(int id)
        {
            var response = await _payment.CreateDebitNoteRRR(id);
            if (response.StatusCode.Equals(HttpStatusCode.OK))
                return Redirect($"{_appSetting.ElpsUrl}/Payment/Pay?rrr={response.Data}");
            else
            {
                var payment = await _unitOfWork.Payment.FirstOrDefaultAsync(p => p.Id.Equals(id));

                var coqRef = await _unitOfWork.CoQReference.FirstOrDefaultAsync(c => c.Id.Equals(payment.COQId));
                if(coqRef != null)
                {
                    if(coqRef.PlantCoQId == null)
                        return Redirect($"{_appSetting.LoginUrl}/company/approvals/{payment.ApplicationId}/debit-notes");
                    else
                        return Redirect($"{_appSetting.LoginUrl}/company/approvals/httpiti/debit-notes");
                }
            }
            return Redirect($"{_appSetting.LoginUrl}/home");
        }
    }
}
