using Bunkering.Access.Services;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepotOfficerController : ResponseController
    {
        private readonly DepotOfficerService _depotOfficerService;

        public DepotOfficerController(DepotOfficerService depotOfficerService)
        {
            _depotOfficerService = depotOfficerService;
        }


        /// <summary>
        /// This endpoint is used to fetch  all the Fees summary
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/application/get-all-fees
        /// 
        /// </remarks>
        /// <response code="200">Returns the fees  </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [AllowAnonymous]
        [Route("get-all-mappings")]
        [HttpGet]
        public async Task<IActionResult> GetAllDepotOfficerMapping() => Response(await _depotOfficerService.GetAllDepotOfficerMapping());

        [Route("add-mapping")]
        [HttpPost]
        public async Task<IActionResult> AddMapping(DepotFieldOfficerViewModel model) => Response(await _depotOfficerService.CreateDepotOfficerMapping(model));

        [Route("edit-mapping/{id}")]
        [HttpPut]
        public async Task<IActionResult> EditMapping(int id, DepotFieldOfficerViewModel model) => Response(await _depotOfficerService.EditDepotOfficerMapping(id, model));

        [Route("get-mapping-byId")]
        [HttpGet]
        public async Task<IActionResult> GetMappingById(int id) => Response(await _depotOfficerService.GetDepotOfficerByID(id));

        [Route("delete-mapping")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMapping(int id) => Response(await _depotOfficerService.DeleteMapping(id));
    }
}
