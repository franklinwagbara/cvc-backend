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

        [Route("add-plant")]
        [HttpPost]
        public async Task<IActionResult> AddPlant(PlantDTO model) => Response(await _plantService.CreatePlant(model));

        [Route("edit-plant/{plant}")]
        [HttpPut]
        public async Task<IActionResult> EditPlant(PlantDTO model) => Response(await _plantService.EditPlant(model));

        [Route("edit-plantTank/{plantTank}")]
        [HttpPut]
        public async Task<IActionResult> EditPlantTank(PlantTankDTO model) => Response(await _plantService.EditPlantTanks(model));


        [Route("get-plant/{id}")]
        [HttpPut]
        public async Task<IActionResult> GetPlantById(int id) => Response(await _plantService.GetPlantById(id));

        [Route("delete-plant")]
        [HttpDelete]
        public async Task<IActionResult> DeletePlant(int id) => Response(await _plantService.DeletePlant(id));
        
        [Route("delete-plantTank")]
        [HttpDelete]
        public async Task<IActionResult> DeletePlantTank(int id) => Response(await _plantService.DeletePlantTank(id));


    }
}
