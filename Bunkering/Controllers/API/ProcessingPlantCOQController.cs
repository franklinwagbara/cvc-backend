using Bunkering.Access.Services;
using Bunkering.Core.Data;
using Bunkering.Core.Dtos;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var result = await _processingPlantCoQService.CreateLiquidDynamicCOQ(dto);
            return Response(result);
        }

        [Route("create-liquid-coq")]
        [HttpPost]
        public async Task<IActionResult> CreateLiquidCOQ(LiquidCOQPostDto dto)
        {
            var result = await _processingPlantCoQService.CreateLiquidCOQ(dto);
            return Response(result);
        }


        [Route("create-condensate-coq")]
        [HttpPost]
        public async Task<IActionResult> CreateCondensateCOQ(CondensateCOQPostDto dto)
        {
            //var result = await _processingPlantCoQService.CreateLiquidCOQ(dto);
            return Ok("Coming soon");
        }
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("coq_details/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(int id) => Response(await _processingPlantCoQService.GetPPCOQDetailsById(id));


        /// <summary>
        /// This endpoint is used to process a COQ Application
        /// </summary>
        /// <returns>Returns a message after submission </returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/coq/submit/xxxx
        /// 
        /// </remarks>
        /// <param name="id">The coq id used to fetch coq application</param>
        /// <response code="200">Returns success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("process")]
        [HttpPost]
        public async Task<IActionResult> Process(int id, string act, string comment) => Response(await _processingPlantCoQService.Process(id, act, comment));
    }
}
