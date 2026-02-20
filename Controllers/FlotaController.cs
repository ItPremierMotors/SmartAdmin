using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using SmartAdmin.Models.Vehiculo;

namespace SmartAdmin.Controllers
{
    public class FlotaController : Controller
    {
        private readonly IVehiculo vehiculoServices;

        public FlotaController(IVehiculo vehiculoServices)
        {
            this.vehiculoServices = vehiculoServices;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> DetailPartial(int id)
        {
            var response = await vehiculoServices.GetDetalleAsync(id);
            if (response.Success && response.Data != null)
                return PartialView("_DetailPartial", response.Data);

            return Content("<div class='alert alert-danger'>Vehículo no encontrado</div>");
        }

        [HttpGet]
        public async Task<IActionResult> EditPartial(int id)
        {
            var response = await vehiculoServices.GetDetalleAsync(id);
            if (response.Success && response.Data != null)
            {
                var d = response.Data;
                var editModel = new EditVehiculoViewModel
                {
                    VehiculoId = d.VehiculoId,
                    Vin = d.Vin,
                    Placa = d.Placa,
                    NumeroMotor = d.NumeroMotor,
                    NumeroChasis = d.NumeroChasis,
                    MarcaId = d.MarcaId,
                    ModeloId = d.ModeloId,
                    VersionId = d.VersionId,
                    Anio = d.Anio,
                    Color = d.Color,
                    Estado = d.Estado,
                    SucursalId = d.SucursalId,
                    Procedencia = d.Procedencia,
                    NumeroImportacion = d.NumeroImportacion,
                    NumeroPoliza = d.NumeroPoliza,
                    FechaIngresoPais = d.FechaIngresoPais,
                    FechaRecepcion = d.FechaRecepcion,
                    CostoImportacion = d.CostoImportacion,
                    ClienteId = d.ClienteId,
                    PrecioLista = d.PrecioLista,
                    PrecioVenta = d.PrecioVenta,
                    FechaVenta = d.FechaVenta,
                    FechaEntrega = d.FechaEntrega,
                    VendedorId = d.VendedorId,
                    KilometrajeActual = d.KilometrajeActual,
                    FechaPrimeraMatricula = d.FechaPrimeraMatricula,
                    GarantiaHasta = d.GarantiaHasta,
                    Observaciones = d.Observaciones
                };
                ViewBag.ClienteNombre = d.ClienteNombre;
                return PartialView("_EditPartial", editModel);
            }
            return Content("<div class='alert alert-danger'>Vehículo no encontrado</div>");
        }

        // Data endpoints
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await vehiculoServices.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                var all = await vehiculoServices.GetAllAsync();
                return StatusCode(all.StatusCode, all);
            }
            var response = await vehiculoServices.SearchAsync(term);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] EditVehiculoViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(new { success = false, message = ObtenerErroresValidacion() });
            var response = await vehiculoServices.EditAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarKilometraje([FromBody] ActualizarKilometrajeRequest request)
        {
            var response = await vehiculoServices.ActualizarKilometrajeAsync(request.VehiculoId, request.NuevoKilometraje);
            return StatusCode(response.StatusCode, response);
        }

        private string ObtenerErroresValidacion()
        {
            return string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
        }
    }
}
