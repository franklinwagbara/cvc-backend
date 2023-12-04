
using Bunkering.Access;
using Bunkering.Access.Services;
using Bunkering.Controllers;
using Bunkering.Core.Data;
using Bunkering.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Buner.Controllers.API
{
    [AllowAnonymous]
    [Route("api/bunkering/[controller]")]
    public class AuthController : ResponseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppSetting _appSetting;
        private readonly UserManager<ApplicationUser> _user;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AuthService _authService;

        public AuthController(
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSetting> appSetting,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            AuthService authService)
        {
            _httpContextAccessor = httpContextAccessor;
            _appSetting = appSetting.Value;
            _user = userManager;
            _signInManager = signInManager;
            _authService = authService;
        }

        [HttpGet]
        [Route("validate-user")]
        public async Task<IActionResult> ValidateUser(string id) => Response(await _authService.ValidateUser(id));

    }
}
