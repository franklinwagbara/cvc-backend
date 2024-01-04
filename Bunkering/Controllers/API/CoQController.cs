﻿using Bunkering.Access;
using Bunkering.Access.IContracts;
using Bunkering.Access.Services;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using System.Net;

namespace Bunkering.Controllers.API
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class CoQController : ResponseController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly CoQService _coqService;

		public CoQController(IUnitOfWork unitOfWork, CoQService coQService)
		{
			_unitOfWork = unitOfWork;
			_coqService = coQService;
		}


		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		[Produces("application/json")]
		[Route("all_coqs")]
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			try
			{
                var coqs = await _unitOfWork.CoQ.GetAll("Application.User.Company,Depot");

                return Ok(new ApiResponse
                {
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = coqs.Select(c => new
                    {
						AppId = c.AppId,
                        ImportName = c.Application?.User?.Company?.Name,
						VesselName = c.Application?.VesselName,
						DepotName = c.Depot?.Name,
						DepotId = c.DepotId,
                        DateOfVesselArrival = c.DateOfVesselArrival.ToShortDateString(),
                        DateOfVesselUllage = c.DateOfVesselUllage.ToShortDateString(),
                        DateOfSTAfterDischarge = c.DateOfSTAfterDischarge.ToShortDateString(),
						MT_VAC = c.MT_VAC,
						MT_AIR = c.MT_AIR,
						GOV = c.GOV,
						GSV = c.GSV,
						DepotPrice = c.DepotPrice,
						CreatedBy = c.CreatedBy,
						Id = c.Id
                    }).ToList()
                });
            }
			catch (Exception e)
			{
                return Response(new ApiResponse
                {
                    Message = e.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = true,
                    Data = null
                });
            }
		}

		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		[Produces("application/json")]
		[Route("coq_by_id/{id}")]
		[HttpGet]
		public async Task<IActionResult> GetById(int id)
		{
            try
            {
                var coq = await _unitOfWork.CoQ.FirstOrDefaultAsync(x => x.Id == id, "Application.User.Company,Depot");

                return Ok(new ApiResponse
                {
                    Message = "Success",
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = new {
                        AppId = coq.AppId,
                        ImportName = coq.Application?.User?.Company?.Name,
                        VesselName = coq.Application?.VesselName,
                        DepotName = coq.Depot?.Name,
                        DepotId = coq.DepotId,
                        DateOfVesselArrival = coq.DateOfVesselArrival.ToShortDateString(),
                        DateOfVesselUllage = coq.DateOfVesselUllage.ToShortDateString(),
                        DateOfSTAfterDischarge = coq.DateOfSTAfterDischarge.ToShortDateString(),
                        DateOfVesselArrivalISO = coq.DateOfVesselArrival.ToString("MM/dd/yyyy"),
                        DateOfVesselUllageISO = coq.DateOfVesselUllage.ToString("MM/dd/yyyy"),
                        DateOfSTAfterDischargeISO = coq.DateOfSTAfterDischarge.ToString("MM/dd/yyyy"),
                        MT_VAC = coq.MT_VAC,
                        MT_AIR = coq.MT_AIR,
                        GOV = coq.GOV,
                        GSV = coq.GSV,
                        DepotPrice = coq.DepotPrice,
                        CreatedBy = coq.CreatedBy,
                        Id = coq.Id,
                        SubmittedDate = coq.SubmittedDate
                    }
                });
            }
            catch (Exception e)
            {
                return Response(new ApiResponse
                {
                    Message = e.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = true,
                    Data = null
                });
            }
        }

        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("createCOQ")]
        [HttpPost]
        public async Task<IActionResult> CreateCoQ([FromBody] CreateCoQViewModel Model) => Response(await _coqService.CreateCoQ(Model));

        [AllowAnonymous]
		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		[Produces("application/json")]
		[Route("coq_by_appId/{appId}")]
		[HttpGet]
		public async Task<IActionResult> GetCoqs(int appId) => Response(await _coqService.GetCoQsByAppId(appId));

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

        /// <summary>
        /// This endpoint is used to fetch documents required for a coq application
        /// </summary>
        /// <returns>Returns a model of required documents </returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/application/get-dcouments/xxxx
        /// 
        /// </remarks>
        /// <param name="id">The application id used to fetch documenst for the application type</param>
        /// <response code="200">Returns an object of fees </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("get-documents")]
        [HttpGet]
        public async Task<IActionResult> DocumentUpload(int id) => Response(await _coqService.DocumentUpload(id));

        /// <summary>
        /// This endpoint is used to add coq documents required for an application
        /// </summary>
        /// <returns>Returns a message after upload </returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/application/add-dcouments/xxxx
        /// 
        /// </remarks>
        /// <param name="id">The application id used to fetch documenst for the application type</param>
        /// <response code="200">Returns an object of fees </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("add-documents")]
        [HttpPost]
        public async Task<IActionResult> AddDocuments(int id) => Response(await _coqService.AddDocuments(id));

        /// <summary>
        /// This endpoint is used to submit a COQ Application
        /// </summary>
        /// <returns>Returns a message after submission </returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/coq/submit/xxxx
        /// 
        /// </remarks>
        /// <param name="id">The coq id used to fetch coq application</param>
        /// <response code="200">Returns success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("submit")]
        [HttpPost]
        public async Task<IActionResult> Submit(int id) => Response(await _coqService.Submit(id));

        /// <summary>
        /// This endpoint is used to process a COQ Application
        /// </summary>
        /// <returns>Returns a message after submission </returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/coq/submit/xxxx
        /// 
        /// </remarks>
        /// <param name="id">The coq id used to fetch coq application</param>
        /// <response code="200">Returns success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("process")]
        [HttpPost]
        public async Task<IActionResult> Process(int id, string act, string comment) => Response(await _coqService.Process(id, act, comment));

        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("debit_note/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DebitNote(int id)
        {
            var note = (await _coqService.GetDebitNote(id));
            var data = note.Data as DebitNoteDTO;
            if (data is not null)
            {
                var viewAsPdf = new ViewAsPdf
                {
                    Model = data,
                    PageHeight = 327,
                    PageMargins = new Rotativa.AspNetCore.Options.Margins(10, 10, 10, 10),

                    ViewName = "DebitNote"
                };
                var pdf = await viewAsPdf.BuildFile(ControllerContext);
                return File(new MemoryStream(pdf), "application/pdf");
            }
            return BadRequest(note);

        }

        /// <summary>
        /// This endpoint is used to add coq tanks
        /// </summary>
        /// <returns>Returns a message after adding </returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/coq/add-coq-tank/xxxx
        /// 
        /// </remarks>
        /// <param name="model">model for adding tank to coq</param>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("add-coq-tank")]
        [HttpPost]
        public async Task<IActionResult> AddCoqTank(COQCrudeTankDTO model) => Response(await _coqService.AddCoqTank(model));

        /// <summary>
        /// This endpoint is used to add coq gas tanks
        /// </summary>
        /// <returns>Returns a message after adding </returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/coq/add-coq-tank/xxxx
        /// 
        /// </remarks>
        /// <param name="model">model for adding tank to coq</param>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("add-coq-gas-tank")]
        [HttpPost]
        public async Task<IActionResult> AddCoqTank(CreateGasProductCoQDto model) => Response(await _coqService.AddCoqTank(model));
    }
}
