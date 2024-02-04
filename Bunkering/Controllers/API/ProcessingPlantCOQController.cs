using Bunkering.Access.Services;
using Bunkering.Core.Data;
using Bunkering.Core.Dtos;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bunkering.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessingPlantCOQController : ResponseController
    {
        private readonly ProcessingPlantCoQService _processingPlantCoQService;
        public ProcessingPlantCOQController(ProcessingPlantCoQService processingPlantCoQService)
        {
            _processingPlantCoQService = processingPlantCoQService;
        }

        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("all_coqs")]
        [HttpGet]
        public async Task<IActionResult> Index() => Response(await _processingPlantCoQService.GetAllCoQ());


        [Route("create-liquid-static-coq")]
        [HttpPost]
        public async Task<IActionResult> CreateLiquidStaticCOQ(UpsertPPlantCOQLiquidStaticDto dto)
        {
            var result = await _processingPlantCoQService.CreateLiquidStaticCOQ(dto);
            return Response(result);
        }

        [Route("create-liquid-dynamic-coq")]
        [HttpPost]
        public async Task<IActionResult> CreateLiquidDynamicCOQ(UpsertPPlantCOQLiquidDynamicDto dto)
        {
            return Ok("Coming Soon");
        }

    }
}
