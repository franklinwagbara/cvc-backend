using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bunkering.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppProductsController : ResponseController
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AppProductsController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }


    }

}