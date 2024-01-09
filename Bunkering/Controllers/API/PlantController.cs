using Bunkering.Access.Services;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlantController : ResponseController
    {
        private readonly PlantService _plantService;

        public PlantController(PlantService plantService)
        {
            _plantService = plantService;
        }


        /// <summary>
        /// This endpoint is used to fetch  all the Plants summary
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/application/get-all-plants
        /// 
        /// </remarks>
        /// <response code="200">Returns the Plants  </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [AllowAnonymous]
        [Route("get-all-Plants")]
        [HttpGet]
        public async Task<IActionResult> GetAllPlants() => Response(await _plantService.GetAllPlants());


        [AllowAnonymous]
        [Route("get-all-depots-list")]
        [HttpGet]
        public async Task<IActionResult> GetAllDepotsFromDepotsOnline() => Response(await _plantService.GetDepotsList());

        [Route("get-all-PlantsByCompany")]
        [HttpGet]
        public async Task<IActionResult> GetAllPlantsByCompany() => Response(await _plantService.GetAllPlantsByCompany());

        [AllowAnonymous]
        [Route("get-all-Tanks")]
        [HttpGet]
        public async Task<IActionResult> GetAllTanksByPlantId(int plantId) => Response(await _plantService.GetAllPlantTanksByPlantId(plantId));

        [Route("add-plant")]
        [HttpPost]
        public async Task<IActionResult> AddPlant(PlantDTO model) => Response(await _plantService.CreatePlant(model));
        
        [Route("add-plantTank")]
        [HttpPost]
        public async Task<IActionResult> AddPlantTank(PlantTankDTO model, int id) => Response(await _plantService.CreatePlantTank(model, id));

        [Route("edit-plant/{id}")]
        [HttpPut]
        public async Task<IActionResult> EditPlant(int id, PlantDTO model) => Response(await _plantService.EditPlant(id, model));

        [Route("edit-plantTank/{id}")]
        [HttpPut]
        public async Task<IActionResult> EditPlantTank(int id, PlantTankDTO model) => Response(await _plantService.EditPlantTanks(id, model));


        [Route("get-plant/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetPlantById(int id) => Response(await _plantService.GetPlantById(id));

        [Route("delete-plant")]
        [HttpDelete]
        public async Task<IActionResult> DeletePlant(int id) => Response(await _plantService.DeletePlant(id));
        
        [Route("delete-plantTank")]
        [HttpDelete]
        public async Task<IActionResult> DeletePlantTank(int id) => Response(await _plantService.DeletePlantTank(id));


        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("All-processing-plant")]
        [HttpGet]

        public async Task<IActionResult> AllProcessingPlants() => Response(await _plantService.GetAllProcessingPlants());

    }
}
