using Bunkering.Access.Services;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SourceVesselController : ResponseController
    {
       private readonly SourceRecipientVesselService _recipientVesselService;
        public SourceVesselController(SourceRecipientVesselService recipientVesselService)
        {
            _recipientVesselService = recipientVesselService;
        }

        [AllowAnonymous]
        [Route("get-all-mappings")]
        [HttpGet]
        public async Task<IActionResult> GetAllSourceRecipientMapping() => Response(await _recipientVesselService.GetAllSourceRecipientMapping());

        [Route("add-mapping")]
        [HttpPost]
        public async Task<IActionResult> AddMapping(SourceDestinationVesselViewModel model) => Response(await _recipientVesselService.CreateSourceRecipientVesselMapping(model));

        [Route("edit-mapping/{id}")]
        [HttpPut]
        public async Task<IActionResult> EditMapping(int id, SourceDestinationVesselViewModel model) => Response(await _recipientVesselService.EditSourceDestinationMapping(id, model));

        [Route("get-mapping-byId")]
        [HttpGet]
        public async Task<IActionResult> GetMappingById(int id) => Response(await _recipientVesselService.GetSourceRecipientVesselByID(id));

        [Route("delete-mapping")]
        [HttpDelete]
        public async Task<IActionResult> DeleteMapping(int id) => Response(await _recipientVesselService.DeleteMapping(id));
    }
}
