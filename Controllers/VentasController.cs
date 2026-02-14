using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using SmartAdmin.Models.Vehiculo;

namespace SmartAdmin.Controllers
{
    public class VentasController : Controller
    {
        private readonly IVehiculo vehiculoServices;

        // Transiciones post-venta: Vendido -> Entregado (final)
        private static readonly Dictionary<int, int[]> TransicionesPostVenta = new()
        {
            { 6, new[] { 7 } }
        };

        private static readonly Dictionary<int, string> NombresEstado = new()
        {
            { 1, "En Tránsito" }, { 2, "En Aduana" }, { 3, "En Bodega" },
            { 4, "En Exhibición" }, { 5, "Reservado" }, { 6, "Vendido" }, { 7, "Entregado" }
        };

        public VentasController(IVehiculo vehiculoServices)
        {
            this.vehiculoServices = vehiculoServices;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> DetailPartial(int id)
        {
            var response = await vehiculoServices.GetDetalleAsync(id);
            if (response.Success && response.Data != null)
            {
                ViewBag.TransicionesValidas = GetTransicionesValidas(response.Data.Estado);
                ViewBag.NombresEstado = NombresEstado;
                return PartialView("_DetailPartial", response.Data);
            }
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
                    TipoCombustible = d.TipoCombustible,
                    Transmision = d.Transmision,
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

        [HttpGet]
        public async Task<IActionResult> EntregaPartial(int id)
        {
            var response = await vehiculoServices.GetDetalleAsync(id);
            if (response.Success && response.Data != null && response.Data.Estado == 6)
            {
                var model = new ProcesarEntregaViewModel
                {
                    VehiculoId = id,
                    FechaEntrega = DateTime.Now
                };
                ViewBag.VehiculoInfo = $"{response.Data.DescripcionCompleta} - {response.Data.Identificador}";
                ViewBag.ClienteNombre = response.Data.ClienteNombre;
                return PartialView("_EntregaPartial", model);
            }
            return Content("<div class='alert alert-danger'>El vehículo debe estar en estado Vendido para procesar la entrega</div>");
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

        // CRUD
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] EditVehiculoViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await vehiculoServices.EditAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado([FromBody] CambiarEstadoRequest request)
        {
            if (!NombresEstado.TryGetValue(request.NuevoEstado, out var estadoStr))
                return BadRequest(new { success = false, message = "Estado no válido" });
            var response = await vehiculoServices.CambiarEstadoAsync(request.VehiculoId, estadoStr);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarKilometraje([FromBody] ActualizarKilometrajeRequest request)
        {
            var response = await vehiculoServices.ActualizarKilometrajeAsync(request.VehiculoId, request.NuevoKilometraje);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarEntrega([FromBody] ProcesarEntregaViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // 1. Obtener detalle actual
            var detalle = await vehiculoServices.GetDetalleAsync(model.VehiculoId);
            if (!detalle.Success || detalle.Data == null)
                return StatusCode(detalle.StatusCode, detalle);

            if (detalle.Data.Estado != 6)
                return BadRequest(new { success = false, message = "El vehículo debe estar en estado Vendido para procesar la entrega" });

            // 2. Actualizar con fecha de entrega
            var d = detalle.Data;
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
                TipoCombustible = d.TipoCombustible,
                Transmision = d.Transmision,
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
                KilometrajeActual = d.KilometrajeActual,
                FechaPrimeraMatricula = d.FechaPrimeraMatricula,
                GarantiaHasta = d.GarantiaHasta,
                Observaciones = d.Observaciones,
                // Dato de entrega
                FechaEntrega = model.FechaEntrega
            };

            var editResult = await vehiculoServices.EditAsync(editModel);
            if (!editResult.Success)
                return StatusCode(editResult.StatusCode, editResult);

            // 3. Cambiar estado a Entregado
            var estadoResult = await vehiculoServices.CambiarEstadoAsync(model.VehiculoId, "Entregado");
            if (!estadoResult.Success)
                return StatusCode(estadoResult.StatusCode, new { success = false, message = "La fecha de entrega se guardó pero hubo un error al cambiar el estado. Intente cambiar el estado manualmente." });

            return Ok(new { success = true, message = "Entrega procesada exitosamente" });
        }

        private Dictionary<int, string> GetTransicionesValidas(int estadoActual)
        {
            var result = new Dictionary<int, string>();
            if (TransicionesPostVenta.TryGetValue(estadoActual, out var validas))
            {
                foreach (var estado in validas)
                {
                    if (NombresEstado.TryGetValue(estado, out var nombre))
                    {
                        result[estado] = nombre;
                    }
                }
            }
            return result;
        }
    }
}
