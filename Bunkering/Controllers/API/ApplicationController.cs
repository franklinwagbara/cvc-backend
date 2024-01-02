using AutoMapper;
using Bunkering.Access;
using Bunkering.Access.IContracts;
using Bunkering.Access.Services;
using Bunkering.Core.Data;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bunkering.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ResponseController
    {
        private readonly AppService _appService;

        public ApplicationController(AppService appService)
        {
            _appService = appService;
        }

        /// <summary>
        /// This endpoint is used to initiate an application 
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// post: api/application/apply
        /// 
        /// </remarks>
        /// <param name="model">Model for applying for a bunker application</param>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("apply")]
        [HttpPost]
        public async Task<IActionResult> Apply(ApplictionViewModel model) => Response(await _appService.Apply(model));

        /// <summary>
        /// This endpoint returns a model of tanks for the application using the application id
        /// </summary>
        /// <returns>Returns a model of tanks which can be mepty</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/application/add-tank
        /// 
        /// </remarks>
        /// <param name="id">The inspector/Reviewer email to fetch inspection for</param>
        /// <response code="200">Returns a list of tanks </response>
        /// <response code="404">Returns not found tanks </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("get-tanks-by-appid")]
        [HttpGet]
        public async Task<IActionResult> AddTanks(int id) => Response(await _appService.GetTanksByAppId(id));

        /// <summary>
        /// This endpoint returns a model of depots for the application using the application id
        /// </summary>
        /// <returns>Returns a model of depots which can be mepty</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/application/get-depots/1
        /// 
        /// </remarks>
        /// <param name="id">NOA application Id</param>
        /// <response code="200">Returns a list of depots </response>
        /// <response code="404">Returns not found depots </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("get-depots-by-appid")]
        [HttpGet]
        public async Task<IActionResult> GetDepots(int id) => Response(await _appService.GetDepotsByAppId(id));

        /// <summary>
        /// This endpoint returns a model of tanks for the application using the application id
        /// </summary>
        /// <returns>Returns a model of tanks which can be mepty</returns>
        /// <remarks>
        /// x
        /// Sample Request
        /// POST: api/application/add-tank
        /// 
        /// </remarks>
        /// <param name="id">The inspector/Reviewer email to fetch inspection for</param>
        /// <response code="200">Returns a list of tanks </response>
        /// <response code="404">Returns not found tanks </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("add-tanks")]
        [HttpPost]
        public async Task<IActionResult> AddTanks(List<TankViewModel> model) => Response(await _appService.AddTanks(model));

        /// <summary>
        /// This endpoint adds payment to the application
        /// </summary>
        /// <returns>Returns a model of fees per facility type</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/application/payment/xxx
        /// 
        /// </remarks>
        /// <param name="id">The application id used to generate payment model</param>
        /// <response code="200">Returns an object of fees </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("payment")]
        [HttpGet]
        public async Task<IActionResult> Payment(int id) => Response(await _appService.Payment(id));

        /// <summary>
        /// This endpoint is used to fetch documents required for an application
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
        public async Task<IActionResult> DocumentUpload(int id) => Response(await _appService.DocumentUpload(id));

        /// <summary>
        /// This endpoint is used to add documents required for an application
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
        public async Task<IActionResult> AddDocuments(int id) => Response(await _appService.AddDocuments(id));

        /// <summary>
        /// This endpoint is used to verify depot license
        /// </summary>
        /// <returns>Returns a message after verification </returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/application/verify-license/xxxx?license=xxx
        /// 
        /// </remarks>
        /// <param name="license">The license field name is used to validate a depot license</param>
        /// <response code="200">Returns an object of fees </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("verify-license")]
        [HttpGet]
        public async Task<IActionResult> CheckLicense(string license) => Response(await _appService.CheckLicense(license));

        /// <summary>
        /// This endpoint is used to fetch all applications
        /// </summary>
        /// <returns>Returns a list of applications</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/application/all-applications
        /// 
        /// </remarks>
        /// <response code="200">Returns an object of applications </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("all-applications")]
        [HttpGet]
        public async Task<IActionResult> All() => Response(await _appService.AllApps());


        /// <summary>
        /// This endpoint is used to fetch LGAs by StateID
        /// </summary>
        /// <returns>Returns a list of lgas</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/application/get-lga-by-stateid
        /// 
        /// </remarks>
        /// <response code="200">Returns an object of lgas </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("get-lga-by-stateid")]
        [HttpGet]
        public async Task<IActionResult> GetLGAbyStateId(int id) => Response(await _appService.GetLGAbyStateId(id));

        /// <summary>
        /// This endpoint is used to fetch applications on a staff desk
        /// </summary>
        /// <returns>Returns a list of applications</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/application/my-desk
        /// 
        /// </remarks>
        /// <response code="200">Returns an object of apps </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("my-desk")]
        [HttpGet]
        public async Task<IActionResult> MyDesk() => Response(await _appService.MyDesk());

        /// <summary>
        /// This endpoint is used to fetch details of an application
        /// </summary>
        /// <returns>Returns an application info</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/application/view-application
        /// 
        /// </remarks>
        /// <response code="200">Returns an application info </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("view-application")]
        [HttpGet]
        public async Task<IActionResult> ViewApplication(int id) => Response(await _appService.ViewApplication(id));

        /// <summary>
        /// This endpoint is used to approve or reject an application
        /// </summary>
        /// <returns>Returns a success message or rotherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/application/process
        /// 
        /// </remarks>
        /// <param name="id">This is the id of the application to be processed</param>
        /// <param name="act">This is the action taken on the aplication </param>
        /// <param name="comment">This is the message attaced to the processing of the application </param>
        /// <response code="200">Returns an application info </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("process")]
        [HttpPost]
        public async Task<IActionResult> Process(int id, string act, string comment) => Response(await _appService.Process(id, act, comment));

        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("view-application-By-Depot")]
        [HttpGet]
        public async Task<IActionResult> ViewApplicationByDepotID(int id) => Response(await _appService.AllApplicationsByDepot(id));

        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("view-application-By-Depot-Officer")]
        [HttpGet]
        public async Task<IActionResult> ViewApplicationByDepotOfficer() => Response(await _appService.AllApplicationsInDepotByUserID());

        /// <summary>
        /// This endpoint is used to fetch details of an NOA Vessel
        /// </summary>
        /// <returns>Returns an application info</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/application/get-app-vessel-info
        /// 
        /// </remarks>
        /// <response code="200">Returns an application / vessel info </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("get-app-vessel-info")]
        [HttpGet]
        public async Task<IActionResult> GetAppVesselInfo(int Id, int DepotId) => Response(await _appService.GetAppVesselInfo(Id, DepotId));


    }
}
