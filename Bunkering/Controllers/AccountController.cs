using Bunkering.Core.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bunkering.Core.ViewModels;
using System.Diagnostics;
using Bunkering.Access.IContracts;
using Bunkering.Access;
using Microsoft.AspNetCore.Authorization;
using Bunkering.Access.Services;
using Microsoft.Extensions.Options;

namespace Bunkering.Controllers
{
    //[Authorize]
    [Route("api/bunkering/[controller]")]
    public class AccountController : ResponseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElps _elps;
        private readonly AuthService _authService;
        private readonly AppSetting _appSetting;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> singInManager,
            IUnitOfWork unitOfWork,
            IElps elps,
            AuthService authService,
            IOptions<AppSetting> appSetting)
        {
            _userManager = userManager;
            _signInManager = singInManager;
            _unitOfWork = unitOfWork;
            _elps = elps;
            _authService = authService;
            _appSetting = appSetting.Value;
        }
        //public IActionResult Index()
        //{
        //	return View();
        //}

        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        [ProducesResponseType(typeof(ApiResponse), 405)]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [Produces("application/json")]
        [Route("login-redirect")]
        [HttpPost]
        public async Task<IActionResult> LoginRedirect(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var login = await _authService.UserAuth(model);
                if (login != null && login.Success)
                    return Redirect($"{_appSetting.LoginUrl}/home?id={login.Data}");
            }
            return Redirect($"{_appSetting.LoginUrl}/home");
        }

        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var elpsLogOffUrl = $"{_appSetting.ElpsUrl}/Account/RemoteLogOff";
            var frm = "<form action='" + elpsLogOffUrl + "' id='frmTest' method='post'>" + "<input type='hidden' name='returnUrl' value='" + _appSetting.LoginUrl + "' />" + "<input type='hidden' name='appId' value='" + _appSetting.PublicKey + "' />" + "</form>" + "<script>document.getElementById('frmTest').submit();</script>";
            return Content(frm, "text/html");
        }



    }
}
