using Bunkering.Access.IContracts;
using Bunkering.Access.Services;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ResponseController
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// This endpoint is used to add new role
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/staff/add-role
        /// 
        /// </remarks>
        /// <response code="200">Returns success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        /// 
        [Route("add-role")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel model) => Response(await _roleService.CreateRole(model));


        /// <summary>
        /// This endpoint is used to edit role
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/staff/edit-role
        /// 
        /// </remarks>
        /// <response code="200">Returns success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [Route("edit-role")]
        [HttpPost]
        public async Task<IActionResult> EditRole(RoleViewModel model) => Response(await _roleService.EditRole(model));

        /// <summary>
        /// This endpoint is used to delete role
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// POST: api/staff/edit-role
        /// 
        /// </remarks>
        /// <response code="200">Returns success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        [Route("delete-role")]
        [HttpDelete]
        public async Task<IActionResult> DeleteRole(string Id) => Response(await _roleService.DeleteRole(Id));


        /// <summary>
        /// This endpoint is used to get all roles
        /// </summary>
        /// <returns>Returns a success message or otherwise</returns>
        /// <remarks>
        /// 
        /// Sample Request
        /// GET: api/staff/get-roles
        /// 
        /// </remarks>
        /// <response code="200">Returns success message </response>
        /// <response code="404">Returns not found </response>
        /// <response code="401">Unauthorized user </response>
        /// <response code="400">Internal server error - bad request </response>
        /// 
        [Route("get-roles")]
        [HttpGet]
        public async Task<IActionResult> AllRoles() => Response(await _roleService.AllRoles());
    }
}
