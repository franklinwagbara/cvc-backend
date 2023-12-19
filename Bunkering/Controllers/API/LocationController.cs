using Bunkering.Access.Services;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;

namespace Bunkering.Controllers.API
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ResponseController
    {
        private readonly LocationService _locationService;
        private readonly OfficeService _officeService;
        private readonly JettyService _jettyService;
        private readonly DepotService _depotService;


        public LocationController(LocationService locationService, OfficeService officeService, JettyService jettyService, DepotService depotService)
        {
            _locationService = locationService;
            _officeService = officeService;
            _jettyService = jettyService;
            _depotService = depotService;

        }

        /// <summary>
        /// This endpoint is used to create location
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/location
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
        [Route("create-locations")]
        [HttpPost]

        public async Task<IActionResult> CreateLocation(LocationViewModel model) => Response(await _locationService.CreateLocation(model));

        /// <summary>
        /// This endpoint is used to edit location
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/location
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
        [Route("edit-locations")]
        [HttpPost]

        public async Task<IActionResult> EditLocation(LocationViewModel model) => Response(await _locationService.EditLocation(model));


        /// <summary>
        /// This endpoint is used to create Office
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/location
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
        [Route("create-office")]
        [HttpPost]

        public async Task<IActionResult> CreateOffcie(OfficeViewModel model) => Response(await _officeService.CreateOffice(model));


        /// <summary>
        /// This endpoint is used to edit Office
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/location/location
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
        [Route("edit-office")]
        [HttpPost]

        public async Task<IActionResult> EditOffice(OfficeViewModel model) => Response(await _officeService.EditOffice(model));



        /// <summary>
        /// This endpoint is used to create Jetty
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/Location/Jetty
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
        [Route("Create-Jetty")]
        [HttpPost]

        public async Task<IActionResult> CreateJetty(JettyViewModel model) => Response(await _jettyService.CreateJetty(model));



        /// <summary>
        /// This endpoint is used to edit Jetty
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/Location/Jetty
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
        [Route("Edit-Jetty")]
        [HttpPost]

        public async Task<IActionResult> EditJetty(JettyViewModel model) => Response(await _jettyService.EditJetty(model));



        /// <summary>
        /// This endpoint is used to delete Jetty
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// Delete: api/Location/Jetty
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
        [Route("Delete-Jetty")]
        [HttpDelete]

        public async Task<IActionResult> DeleteJetty(int id) => Response(await _jettyService.DeleteJetty(id));


        /// <summary>
        /// This endpoint is used to C  reate Depot
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/Location/Depot
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
        [Route("Create-Depot")]
        [HttpPost]

        public async Task<IActionResult> CreateDepot(DepotViewModel model) => Response(await _depotService.CreateDepot(model));



        /// <summary>
        /// This endpoint is used to Edit Depot
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/Location/Depot
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
        [Route("Edit-Depot")]
        [HttpPost]

        public async Task<IActionResult> EditDepot(DepotViewModel model) => Response(await _depotService.EditDepot(model));



        /// <summary>
        /// This endpoint is used to Delete Depot
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// DELETE: api/Location/Depot
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
        [Route("Delete-Depot")]
        [HttpDelete]

        public async Task<IActionResult> DeleteDepot(int id) => Response(await _depotService.DeleteDepot(id));







    }



}
