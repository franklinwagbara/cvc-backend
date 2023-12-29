using Bunkering.Access.Services;
using Bunkering.Core.Utils;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Identity.Client;
using System.Runtime.CompilerServices;

namespace Bunkering.Controllers.API
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]

    public class LibraryController : ResponseController
    {
        private readonly LibraryService libraryService;
        private readonly JettyService _jettyService;
        private readonly DepotService _depotservice;
        private readonly NominatedSurveyorService _nominatedSurveyorService;
        private readonly AppStageDocService _appStageDocService;


        public LibraryController(LibraryService libraryService_, AppStageDocService appStageDocService, JettyService jettyService, DepotService depotService, NominatedSurveyorService nominatedSurveyorService)
        {
            this.libraryService = libraryService_;

            _appStageDocService = appStageDocService;
            _jettyService = jettyService;
            _depotservice = depotService;
            _nominatedSurveyorService = nominatedSurveyorService;

        }


        /// <summary>
        /// This endpoint is used to get all states
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/states
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("States")]
        [HttpGet]
        public async Task<IActionResult> GetStates() => Response(await libraryService.GetAllStates());


        /// <summary>
        /// This endpoint is used to get all states
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/states
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("StatesInNigeria")]
        [HttpGet]
        public async Task<IActionResult> GetStatesInNigeria() => Response(await libraryService.GetAllStatesInNigeria());

        //public async Task<IActionResult> GetStates()
        //{
        //    var states = await locationService.GetAllStates();
        //   return Response(states);
        //}

        /// <summary>
        /// This endpoint is used to get all Local Governments
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/local Government 
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("Lga")]
        [HttpGet]
        public async Task<IActionResult> GetAllLocalGov() => Response(await libraryService.GetLocalGov());



        /// <summary>
        /// This endpoint is used to get all Local Government by StateId
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/local Government 
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("LgaByStateId")]
        [HttpGet]
        public async Task<IActionResult> LGAByStateId(int stateId) => Response(await libraryService.LGA_StateID(stateId));



        /// <summary>
        /// This endpoint is used to get all states by CountryId
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/local Government 
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("StatebyCountryId/{id:int}")]
        [HttpGet]
        public async Task<IActionResult> StateByCountID(int id) => Response(await libraryService.State_CountryID(id));



        /// <summary>
        /// This endpoint is used to get all Countries
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/local Government 
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("Countries")]
        [HttpGet]
        public async Task<IActionResult> all_countires() => Response(await libraryService.GetCountries());

        /// <summary>
        /// This endpoint is used to get all AppStatus
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/local Government 
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("GetAppStatus")]
        [HttpGet]

        public async Task<IActionResult> GetAppStatus() => Ok(EnumExtension.GetNames<AppStatus>());



        /// <summary>
        /// This endpoint is used to get all App Actions
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/local Government 
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("GetAllAppActions")]
        [HttpGet]

        public async Task<IActionResult> getAppActions() => Ok(EnumExtension.GetNames<AppActions>());



        /// <summary>
        /// This endpoint is used to get Application Types
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/local Government 
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("ApplicationTypes")]
        [HttpGet]

        public async Task<IActionResult> ApplicationType() => Response(await libraryService.ApplicationType());


        /// <summary>
        /// This endpoint is used to get Facility Types
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/local Government 
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("FacilityTypes")]
        [HttpGet]

        public async Task<IActionResult> FacilityTypes() => Response(await libraryService.FacilityTypes());


        /// <summary>
        /// This endpoint is used to get All Roles
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/local Government 
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("Roles")]
        [HttpGet]

        public async Task<IActionResult> GetRoles() => Response(await libraryService.GetRoles());


        /// <summary>
        /// This endpoint is used to get products
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/local Government 
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("GetProducts")]
        [HttpGet]

        public async Task<IActionResult> GetProducts() => Response(await libraryService.GetProducts());


        /// <summary>
        /// This endpoint is used to get all VesselTypes
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/VesselType 
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("VesselType")]
        [HttpGet]

        public async Task<IActionResult> GetVesselType() => Response(await libraryService.GetVesselType());


        /// <summary>
        /// This endpoint is used to get all Users in HQ
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/AllUsersHQ
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("All-UsersFO")]
        [HttpGet]

        public async Task<IActionResult> AllUserFO() => Response(await libraryService.AllUsersFO());



        /// <summary>
        /// This endpoint is used to get all locations
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/AllLocations
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("All-Locations")]
        [HttpGet]

        public async Task<IActionResult> AllLocations() => Response(await libraryService.AllLocations());

        /// <summary>
        /// This endpoint is used to get all Offices
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/AllOffices
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("All-Offices")]
        [HttpGet]

        public async Task<IActionResult> AllOffices() => Response(await libraryService.AllOffices());


        /// <summary>
        /// This endpoint is used to get all Jetty
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/AllJetty
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("All-Jetty")]
        [HttpGet]

        public async Task<IActionResult> AllJetty() => Response(await _jettyService.AllJetty());



        /// <summary>
        /// This endpoint is used to get all Depot
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/AllDepot
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("All-Depot")]
        [HttpGet]

        public async Task<IActionResult> AllDepot() => Response(await _depotservice.GetAllDepot());

        /// <summary>
        /// This endpoint is used to get all Depot by noa id
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/AllDepot-by-AppId
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("All-Depot-By-AppId")]
        [HttpGet]

        public async Task<IActionResult> AllDepotByAppId(int AppId) => Response(await _depotservice.AllDepotByAppId(AppId));



        /// <summary>
        /// This endpoint is used to get all Nominated Surveyor
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/AllNominatedSurveyor
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("All-NominatedSurveyor")]
        [HttpGet]

        public async Task<IActionResult> AllNominatedSurveyor() => Response(await _nominatedSurveyorService.AllNominatedSurveyor());
    }
}
