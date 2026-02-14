using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;

namespace SmartAdmin.Controllers
{
    public class EstadosOsController : Controller
    {
        private readonly IEstadoOs estadoOsServices;

        public EstadosOsController(IEstadoOs estadoOsServices)
        {
            this.estadoOsServices = estadoOsServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DetailPartial(int id)
        {
            var response = await estadoOsServices.GetDetails(id);
            if (response.Success && response.Data != null)
            {
                return PartialView("_DetailPartial", response.Data);
            }
            return Content("<div class='alert alert-danger'>Estado no encontrado</div>");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await estadoOsServices.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}
