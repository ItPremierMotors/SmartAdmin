using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using SmartAdmin.Models.Catalogo.VersionVehiculo;

namespace SmartAdmin.Controllers
{
    public class VersionesVehiculoController : Controller
    {
        private readonly IVersionVehiculo versionServices;

        public VersionesVehiculoController(IVersionVehiculo versionServices)
        {
            this.versionServices = versionServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> VersionDetailPartial(int id)
        {
            var response = await versionServices.GetDetails(id);
            if (response.Success && response.Data != null)
            {
                return PartialView("_DetailPartial", response.Data);
            }
            return Content("<div class='alert alert-danger'>Versión no encontrada</div>");
        }

        [HttpGet]
        public IActionResult DeletePartial(int id)
        {
            ViewBag.VersionId = id;
            return PartialView("_DeletePartial");
        }

        [HttpGet]
        public async Task<IActionResult> CreatePartial()
        {
            return PartialView("_CreatePartial", new CreateVersionVehiculoViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> EditPartial(int id)
        {
            var response = await versionServices.GetDetails(id);
            if (response.Success && response.Data != null)
            {
                var editModel = new EditVersionVehiculoViewModel
                {
                    VersionId = response.Data.VersionId,
                    ModeloId = response.Data.ModeloId,
                    Codigo = response.Data.Codigo,
                    Nombre = response.Data.Nombre,
                    Motor = response.Data.Motor,
                    Transmision = response.Data.Transmision,
                    Traccion = response.Data.Traccion,
                    NumPuertas = response.Data.NumPuertas,
                    NumPasajeros = response.Data.NumPasajeros,
                    TipoCombustible = response.Data.TipoCombustible,
                    Cilindraje = response.Data.Cilindraje,
                    PotenciaHp = response.Data.PotenciaHp,
                    TorqueNm = response.Data.TorqueNm,
                    PrecioBase = response.Data.PrecioBase,
                    AnioVersion = response.Data.AnioVersion,
                    CaracteristicasPrincipales = response.Data.CaracteristicasPrincipales
                };
                return PartialView("_EditPartial", editModel);
            }
            return Content("<div class='alert alert-danger'>Versión no encontrada</div>");
        }

        // API methods
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await versionServices.GetAllAsync();
            if (!response.Success)
            {
                TempData["Error"] = response.Message ?? "Error al obtener versiones";
            }
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int versionId)
        {
            var response = await versionServices.GetDetails(versionId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByModelo(int modeloId)
        {
            var response = await versionServices.GetByModeloAsync(modeloId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVersionVehiculoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await versionServices.CreateAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] EditVersionVehiculoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await versionServices.EditAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var response = await versionServices.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
