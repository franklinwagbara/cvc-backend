using Bunkering.Access.Services;
using Bunkering.Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DippingMethodController : ResponseController
    {
        private readonly DippingMethodService _dippingMethodService;
        public DippingMethodController(DippingMethodService dippingMethodService)
        {
            _dippingMethodService = dippingMethodService;
        }

        [AllowAnonymous]
        [Route("get-all-dippingMethod")]
        [HttpGet]
        public async Task<IActionResult> GetAlldippingMethods() => Response(await _dippingMethodService.GetAllDippingMethods());

        [Route("add-dippingMethods")]
        [HttpPost]
        public async Task<IActionResult> AddDippingMethods(DippingMethod model) => Response(await _dippingMethodService.CreateDippingMethod(model));

        [Route("edit-dippingMethods/{id}")]
        [HttpPut]
        public async Task<IActionResult> EditMeterTypes(int id, DippingMethod model) => Response(await _dippingMethodService.EditDippingMethod(id, model));

        [Route("get-dippingMethod-byId")]
        [HttpGet]
        public async Task<IActionResult> GetDippingMethodById(int id) => Response(await _dippingMethodService.GetDippingMethodByID(id));

        [Route("delete-dippingMethod")]
        [HttpDelete]
        public async Task<IActionResult> DeleteDippingMethod(int id) => Response(await _dippingMethodService.DeleteDippingMethod(id));

    }
}
