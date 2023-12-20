using Bunkering.Access;
using Bunkering.Access.IContracts;
using Bunkering.Access.Services;
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

		[ProducesResponseType(typeof(ApiResponse), 200)]
		[ProducesResponseType(typeof(ApiResponse), 404)]
		[ProducesResponseType(typeof(ApiResponse), 405)]
		[ProducesResponseType(typeof(ApiResponse), 500)]
		[Produces("application/json")]
		[Route("coq_by_id/{id}")]
		[HttpGet]
		public async Task<IActionResult> GetById(int id)
		{
			var permits = (await _unitOfWork.Permit
				.Find(c => c.Id == id,"Application.User.Company,Application.Facility.VesselType"))
				.FirstOrDefault();

			return Ok(new ApiResponse
			{
				//using if statements here ?: to check conditions for the permit
				Message = permits is not null ? "Success, Permit Found" : "Permit Not Found",
				StatusCode = permits is not null ? HttpStatusCode.OK : HttpStatusCode.BadRequest,
				Success = permits is null,
				Data = permits is null ? null! : new
				{
					permits.Id,
					CompanyName = permits.Application?.User?.Company?.Name,
					LicenseNo = permits.PermitNo,
					IssuedDate = permits.IssuedDate.ToString("MMM dd, yyyy HH:mm:ss"),
					EpermitspiryDate = permits.ExpireDate.ToString("MMM dd, yyyy HH:mm:ss"),
					permits.Application?.User?.Email,
					VesselTypeType = permits.Application?.Facility?.VesselType?.Name,
					VesselName = permits.Application?.Facility?.Name,
				}
			});

		}

        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("createCOQ")]
        [HttpPost]
        public async Task<IActionResult> CreateCoQ([FromBody] CoQViewModel Model) => Response(await _coqService.CreateCoQ(Model));

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

    }
}
