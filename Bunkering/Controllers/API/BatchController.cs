using Bunkering.Access.IContracts;
using Bunkering.Access.Services;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BatchController : ResponseController
    {
        private readonly BatchService _batchService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        ApiResponse _response;

        public BatchController(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _batchService = new BatchService(unitOfWork, contextAccessor);
        }


        /// <summary>
        /// This endpoint is used to  add Batch
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/Batch/AddBatch
        /// 
        /// </remarks>
        /// <response code="200">Returns the fees  </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>


        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("Add-Batch")]
        [HttpPost]

        public async Task<IActionResult> AddBatch(BatchViewModel model) => Response(await _batchService.AddBatch(model));


        /// <summary>
        /// This endpoint is used to update Batch
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/Batch/UpdateBatch
        /// 
        /// </remarks>
        /// <response code="200">Returns the fees  </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>


        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("Update-Batch")]
        [HttpPost]

        public async Task<IActionResult> UpdateBatch(BatchViewModel model) => Response(await _batchService.UpdateBatch(model));


        /// <summary>
        /// This endpoint is used to delete Batch
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// DELETE: api/Batch/DeleteBatch
        /// 
        /// </remarks>
        /// <response code="200">Returns the fees  </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>


        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("Delete-Batch")]
        [HttpDelete]

        public async Task<IActionResult> DeleteBatch(int id) => Response(await _batchService.DeleteBatch(id));



        /// <summary>
        /// This endpoint is used to fetch all Batch
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/Batch/AllBatch
        /// 
        /// </remarks>
        /// <response code="200">Returns the fees  </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>


        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("All-Batch")]
        [HttpGet]

        public async Task<IActionResult> AllBatches() => Response(await _batchService.AllBatches());


        /// <summary>
        /// This endpoint is used to fetch  Batches By Id
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/Batch/BatchById
        /// 
        /// </remarks>
        /// <response code="200">Returns the fees  </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>


        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("Batch-By-{id}")]
        [HttpGet]

        public async Task<IActionResult> BatchById(int id) => Response(await _batchService.BatchById(id));
    }
}

