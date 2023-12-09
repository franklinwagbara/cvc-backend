using Bunkering.Access.Services;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ResponseController
    {

        private readonly MessageService _messageService;

        public MessageController( MessageService messageService)
        {
            _messageService = messageService;
        }


        /// <summary>
        /// This endpoint is used to Read message 
        /// </summary>
        /// <returns>Returns a success message or rotherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/company/ReadMessage
        /// 
        /// </remarks>
        /// <param name="model">This is Read message</param>
        /// <response code="200">Returns a summary of the message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Route("read-message")]
        [HttpGet]
        public async Task<IActionResult> ReadMessage(int id) => Response(await _messageService.Read(id));




    }
}
