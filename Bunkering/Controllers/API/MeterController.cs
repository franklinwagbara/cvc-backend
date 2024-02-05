using Bunkering.Access.IContracts;
using Bunkering.Access.Services;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MeterController : ResponseController
    {
        private readonly MeterService _meterService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public MeterController(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _meterService = new MeterService(unitOfWork, contextAccessor);
        }


        /// <summary>
        /// This endpoint is used to fetch  add Meter
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/Meter/AddMeter
        /// 
        /// </remarks>
        /// <response code="200">Returns the fees  </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>


        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("Add-Meter")]
        [HttpPost]

        public async Task<IActionResult> AddMeter(MeterViewModel model) => Response(await _meterService.AddMeter(model));


        /// <summary>
        /// This endpoint is used to update Meter
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/Meter/UpdateMeter
        /// 
        /// </remarks>
        /// <response code="200">Returns the fees  </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>


        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("Update-Meter")]
        [HttpPost]

        public async Task<IActionResult> UpdateMeter(MeterViewModel model) => Response(await _meterService.UpdateMeter(model));


        /// <summary>
        /// This endpoint is used to delete Meter
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// DELETE: api/Meter/DeleteMeter
        /// 
        /// </remarks>
        /// <response code="200">Returns the fees  </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>


        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("Delete-Meter")]
        [HttpDelete]

        public async Task<IActionResult> DeleteMeter(int id) => Response(await _meterService.DeleteMeter(id));



        /// <summary>
        /// This endpoint is used to fetch all Meters
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/Meter/AllMeters
        /// 
        /// </remarks>
        /// <response code="200">Returns the fees  </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>


        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("All-Meters")]
        [HttpGet]

        public async Task<IActionResult> AllMeters() => Response(await _meterService.AllMeters());


        /// <summary>
        /// This endpoint is used to fetch  Meters By Id
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/Meter/MetersById
        /// 
        /// </remarks>
        /// <response code="200">Returns the fees  </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>


        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("Meters-By-Id")]
        [HttpGet]

        public async Task<IActionResult> MetersById(int id) => Response(await _meterService.MeterById(id));

        /// <summary>
        /// This endpoint is used to fetch  Meters By PlantId
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/Meter/MetersByPlantId
        /// 
        /// </remarks>
        /// <response code="200">Returns the fees  </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>


        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("Meters-By-PlantId")]
        [HttpGet]

        public async Task<IActionResult> MeterByPlantId(int plantId) => Response(await _meterService.MeterByPlantId(plantId));
    }
}
