using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using SmartAdmin.Models.Catalogo.TipoServicio;

namespace SmartAdmin.Controllers
{
    public class TiposServicioController : Controller
    {
        private readonly ITipoServicio tipoServicioServices;

        public TiposServicioController(ITipoServicio tipoServicioServices)
        {
            this.tipoServicioServices = tipoServicioServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DetailPartial(int id)
        {
            var response = await tipoServicioServices.GetDetails(id);
            if (response.Success && response.Data != null)
            {
                return PartialView("_DetailPartial", response.Data);
            }
            return Content("<div class='alert alert-danger'>Tipo de servicio no encontrado</div>");
        }

        [HttpGet]
        public IActionResult DeletePartial(int id)
        {
            ViewBag.TipoServicioId = id;
            return PartialView("_DeletePartial");
        }

        [HttpGet]
        public IActionResult CreatePartial()
        {
            return PartialView("_CreatePartial", new CreateTipoServicioViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> EditPartial(int id)
        {
            var response = await tipoServicioServices.GetDetails(id);
            if (response.Success && response.Data != null)
            {
                var editModel = new EditTipoServicioViewModel
                {
                    TipoServicioId = response.Data.TipoServicioId,
                    Codigo = response.Data.Codigo,
                    Nombre = response.Data.Nombre,
                    Descripcion = response.Data.Descripcion,
                    DuracionEstimadaMin = response.Data.DuracionEstimadaMin,
                    Clasificacion = response.Data.Clasificacion,
                    PermiteWalkIn = response.Data.PermiteWalkIn,
                    RequiereCita = response.Data.RequiereCita,
                    PrecioBase = response.Data.PrecioBase,
                    StockRequerido = response.Data.StockRequerido
                };
                return PartialView("_EditPartial", editModel);
            }
            return Content("<div class='alert alert-danger'>Tipo de servicio no encontrado</div>");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await tipoServicioServices.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByClasificacion(string clasificacion)
        {
            var response = await tipoServicioServices.GetByClasificacionAsync(clasificacion);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTipoServicioViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await tipoServicioServices.CreateAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] EditTipoServicioViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await tipoServicioServices.EditAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var response = await tipoServicioServices.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
