using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using SmartAdmin.Models.Catalogo.Tecnico;

namespace SmartAdmin.Controllers
{
    public class TecnicosController : Controller
    {
        private readonly ITecnico tecnicoServices;
        private readonly ISucursal sucursalServices;

        public TecnicosController(ITecnico tecnicoServices, ISucursal sucursalServices)
        {
            this.tecnicoServices = tecnicoServices;
            this.sucursalServices = sucursalServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DetailPartial(int id)
        {
            var response = await tecnicoServices.GetDetails(id);
            if (response.Success && response.Data != null)
            {
                return PartialView("_DetailPartial", response.Data);
            }
            return Content("<div class='alert alert-danger'>Técnico no encontrado</div>");
        }

        [HttpGet]
        public IActionResult DeletePartial(int id)
        {
            ViewBag.TecnicoId = id;
            return PartialView("_DeletePartial");
        }

        [HttpGet]
        public IActionResult CreatePartial()
        {
            return PartialView("_CreatePartial", new CreateTecnicoViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> EditPartial(int id)
        {
            var response = await tecnicoServices.GetDetails(id);
            if (response.Success && response.Data != null)
            {
                var editModel = new EditTecnicoViewModel
                {
                    TecnicoId = response.Data.TecnicoId,
                    Codigo = response.Data.Codigo,
                    Nombre = response.Data.Nombre,
                    Apellidos = response.Data.Apellidos,
                    Especialidad = response.Data.Especialidad,
                    BahiaAsignada = response.Data.BahiaAsignada,
                    UsuarioId = response.Data.UsuarioId,
                    SucursalId = response.Data.SucursalId
                };
                return PartialView("_EditPartial", editModel);
            }
            return Content("<div class='alert alert-danger'>Técnico no encontrado</div>");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await tecnicoServices.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetSucursales()
        {
            var response = await sucursalServices.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTecnicoViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await tecnicoServices.CreateAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] EditTecnicoViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await tecnicoServices.EditAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var response = await tecnicoServices.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
