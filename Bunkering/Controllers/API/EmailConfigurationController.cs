using Bunkering.Access.Services;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class EmailConfigurationController : ResponseController
    {
       private readonly EmailConfigurationService _emailConfigurationService;

        public EmailConfigurationController(EmailConfigurationService emailConfigurationService)
        {
            _emailConfigurationService = emailConfigurationService;
        }

        /// <summary>
        /// This endpoint is used to create email configuration
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/email/create-email-configuration
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("create-email-configuration")]
        [HttpPost]

        public async Task<IActionResult> CreateEmailConfiguration(EmailConfigurationViewModel model) => Response(await _emailConfigurationService.CreateEmailConfiguration(model));


        /// <summary>
        /// This endpoint is used to update email configuration
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/email/update-email-configuration
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("update-email-configuration")]
        [HttpPost]

        public async Task<IActionResult> UpdateEmailConfiguration(EmailConfigurationViewModel model) => Response(await _emailConfigurationService.UpdateEmailConfiguration(model));




        /// <summary>
        /// This endpoint is to delete email configuration
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/email/delete-email-configuration
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("delete-email-configuration")]
        [HttpDelete]



        public async Task<IActionResult> DeleteEmailConfiguration(int id) => Response(await _emailConfigurationService.DeleteEmailConfiguration(id));


        /// <summary>
        /// This endpoint is used to fetch all email configurations
        /// </summary>
        /// <returns>Returns a success message</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/email/all-email-configuration
        /// 
        /// </remarks>
        /// <response code="200">Returns a success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("all-email-configuration")]
        [HttpGet]

        public async Task<IActionResult> AllEmailConfiguration() => Response(await _emailConfigurationService.AllEmailConfigurations());
    }
}
