using Bunkering.Access.Services;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JettyOfficerController : ResponseController 
    {
        private readonly JettyOfficerService _jettyOfficerService;
        public JettyOfficerController(JettyOfficerService jettyOfficerService)
        {
            _jettyOfficerService = jettyOfficerService;
        }

        [AllowAnonymous]
        [Route("get-all-mappings")]
        [HttpGet]
        public async Task<IActionResult> GetAllJettyOfficerMapping() => Response(await _jettyOfficerService.GetAllJettyOfficerMapping());

        [Route("add-mapping")]
        [HttpPost]
        public async Task<IActionResult> AddMapping(JettyFieldOfficerViewModel model) => Response(await _jettyOfficerService.CreateJettyOfficerMapping(model));

        [Route("edit-mapping/{id}")]
        [HttpPut]
        public async Task<IActionResult> EditMapping(int id, JettyFieldOfficerViewModel model) => Response(await _jettyOfficerService.EditJettyOfficerMapping(id, model));

        [Route("get-mapping-byId")]
        [HttpGet]
        public async Task<IActionResult> GetMappingById(int id) => Response(await _jettyOfficerService.GetJettyOfficerByID(id));

        [Route("delete-mapping")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMapping(int id) => Response(await _jettyOfficerService.DeleteMapping(id));
    }
}
