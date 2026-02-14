using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Smartadmin.Controllers
    {
    [Authorize] // cualquier usuario logueado
    public class DashboardController : Controller
        {
            public IActionResult ProjectManagement()
        {
            return View();
        }

        public IActionResult ControlCenter()
        {
            return View();
        }

        public IActionResult Subscription()
        {
            return View();
        }

        public IActionResult Marketing()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
        }
    }