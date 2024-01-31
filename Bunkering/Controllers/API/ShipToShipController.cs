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
    public class ShipToShipController : ResponseController
    {
        private readonly ShipToShipService _shipToShipService;
        public ShipToShipController(ShipToShipService shipToShipService)
        {
            _shipToShipService = shipToShipService;
        }

        [AllowAnonymous]
        [Route("get-all-Records")]
        [HttpGet]
        public async Task<IActionResult> GetAll() => Response(await _shipToShipService.GetAllTransferRecords());

        
        [Route("add-Records")]
        [HttpGet]
        public async Task<IActionResult> AddRecord(TransferRecordDTO tr) => Response(await _shipToShipService.AddRecord(tr));

    }
}
