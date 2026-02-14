using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using SmartAdmin.Models.Cliente;

namespace SmartAdmin.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ICliente clienteServices;

        public ClientesController(ICliente clienteServices)
        {
            this.clienteServices = clienteServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreatePartial()
        {
            return PartialView("_CreatePartial", new CreateClienteViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> EditPartial(int id)
        {
            var response = await clienteServices.GetByIdAsync(id);
            if (response.Success && response.Data != null)
            {
                var editModel = new EditClienteViewModel
                {
                    ClienteId = response.Data.ClienteId,
                    TipoCliente = response.Data.TipoCliente,
                    Nombre = response.Data.Nombre,
                    Apellidos = response.Data.Apellidos,
                    DNI = response.Data.DNI,
                    RTN = response.Data.RTN,
                    Telefono = response.Data.Telefono,
                    TelefonoSecundario = response.Data.TelefonoSecundario,
                    Email = response.Data.Email,
                    Direccion = response.Data.Direccion,
                    Ciudad = response.Data.Ciudad
                };
                return PartialView("_EditPartial", editModel);
            }
            return Content("<div class='alert alert-danger'>Cliente no encontrado</div>");
        }

        [HttpGet]
        public async Task<IActionResult> DetailPartial(int id)
        {
            var response = await clienteServices.GetByIdAsync(id);
            if (response.Success && response.Data != null)
            {
                return PartialView("_DetailPartial", response.Data);
            }
            return Content("<div class='alert alert-danger'>Cliente no encontrado</div>");
        }

        [HttpGet]
        public IActionResult DeletePartial(int id)
        {
            ViewBag.ClienteId = id;
            return PartialView("_DeletePartial");
        }

        // Data endpoints (AJAX)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await clienteServices.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                var all = await clienteServices.GetAllAsync();
                return StatusCode(all.StatusCode, all);
            }
            var response = await clienteServices.SearchAsync(term);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClienteViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await clienteServices.CreateAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] EditClienteViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await clienteServices.EditAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var response = await clienteServices.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
