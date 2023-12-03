using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Bunkering.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
        }
        // GET: HomeController
        public IActionResult Index()
        {
            return View();
        }


    }
}
