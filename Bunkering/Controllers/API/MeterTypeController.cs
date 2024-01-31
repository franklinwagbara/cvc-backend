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
    public class MeterTypeController : ResponseController
    {
        private readonly MeterTypeService _meterTypeService;
        public MeterTypeController(MeterTypeService meterTypeService)
        {
            _meterTypeService = meterTypeService;
        }

        [AllowAnonymous]
        [Route("get-all-meterTypes")]
        [HttpGet]
        public async Task<IActionResult> GetAllMeterTypes() => Response(await _meterTypeService.GetAllMeterTypes());

        [Route("add-meterTypes")]
        [HttpPost]
        public async Task<IActionResult> AddMeterTypes(MeterType model) => Response(await _meterTypeService.CreateMeterType(model));

        [Route("edit-meterTypes/{id}")]
        [HttpPut]
        public async Task<IActionResult> EditMeterTypes(int id, MeterType model) => Response(await _meterTypeService.EditMeterType(id, model));

        [Route("get-meterTypes-byId")]
        [HttpGet]
        public async Task<IActionResult> GetMeterTypesById(int id) => Response(await _meterTypeService.GetMeterTypeByID(id));

        [Route("delete-meterTypes")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMeterType(int id) => Response(await _meterTypeService.DeleteMeterType(id));

    }
}
