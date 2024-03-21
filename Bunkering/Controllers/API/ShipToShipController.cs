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

        [AllowAnonymous]
        [Route("get-all-Records-by-company")]
        [HttpGet]
        public async Task<IActionResult> GetAllRecordsByCompany() => Response(await _shipToShipService.GetAllTransferRecordsByCompany());


        [Route("add-Records")]
        [HttpPost]
        public async Task<IActionResult> AddRecord(DestinationVesselDTO tr) => Response(await _shipToShipService.AddRecord(tr));

        //[Route("add-documents")]
        //[HttpPost]
        //public async Task<IActionResult> AddDocuments(int id) => Response(await _shipToShipService.AddDocuments(id));

        [AllowAnonymous]
        [Route("get-STS-documents")]
        [HttpGet]
        public async Task<IActionResult> GetSTSDocuments(int id) => Response(await _shipToShipService.GetSTSDocuments(id));
    }
}
