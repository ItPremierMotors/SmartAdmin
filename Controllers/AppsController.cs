using Microsoft.AspNetCore.Mvc;

namespace Smartadmin.Controllers
{
    public class AppsController : Controller
    {
        public IActionResult Systemmail()
        {
            return View();
        }

        public IActionResult SystemmailRead()
        {
            return View();
        }

        public IActionResult Emaildesign()
        {
            return View();
        }

        public IActionResult UserProfile()
        {
            return View();
        }
    }
}
