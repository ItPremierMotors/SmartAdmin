using Microsoft.AspNetCore.Mvc;

namespace Smartadmin.Controllers
    {
        public class LandingController : Controller
        {
            public IActionResult Index()
        {
            return View();
        }
        }
    }