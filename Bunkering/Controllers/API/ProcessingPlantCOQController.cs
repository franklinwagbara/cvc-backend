using Bunkering.Access.Services;
using Bunkering.Core.Data;
using Bunkering.Core.Dtos;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
