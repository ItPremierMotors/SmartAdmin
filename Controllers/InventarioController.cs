using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using SmartAdmin.Models.Vehiculo;

namespace SmartAdmin.Controllers
{
    public class InventarioController : Controller
    {
        private readonly IVehiculo vehiculoServices;
        private readonly IMarca marcaServices;
        private readonly ISucursal sucursalServices;
        private readonly IAuth authServices;
        private readonly IApiClient apiClient;

        // Transiciones pre-venta (5->6 se maneja via ProcesarVenta)
        private static readonly Dictionary<int, int[]> TransicionesPreVenta = new()
        {
            { 1, new[] { 2, 3 } },
            { 2, new[] { 3 } },
            { 3, new[] { 4, 5 } },
            { 4, new[] { 5, 3 } },
            { 5, new[] { 4 } }
        };

        private static readonly Dictionary<int, string> NombresEstado = new()
        {
            { 1, "En Tránsito" }, { 2, "En Aduana" }, { 3, "En Bodega" },
            { 4, "En Exhibición" }, { 5, "Reservado" }, { 6, "Vendido" }, { 7, "Entregado" }
        };

        public InventarioController(IVehiculo vehiculoServices, IMarca marcaServices, ISucursal sucursalServices, IAuth authServices, IApiClient apiClient)
        {
            this.vehiculoServices = vehiculoServices;
            this.marcaServices = marcaServices;
            this.sucursalServices = sucursalServices;
            this.authServices = authServices;
            this.apiClient = apiClient;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult CreatePartial()
        {
            return PartialView("_CreatePartial", new CreateVehiculoViewModel());
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
        public async Task<IActionResult> DetailPartial(int id)
        {
            var response = await vehiculoServices.GetDetalleAsync(id);
            if (response.Success && response.Data != null)
            {
                ViewBag.TransicionesValidas = GetTransicionesValidas(response.Data.Estado);
                ViewBag.NombresEstado = NombresEstado;
               
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId != null)
                {
                    var responseUser = await authServices.GetByUserIdAsync(userId);
                    ViewBag.RolesUsuario = responseUser.Data ?? new List<string>();
                }
                return PartialView("_DetailPartial", response.Data);
            }
            return Content("<div class='alert alert-danger'>Vehículo no encontrado</div>");
        }

        [HttpGet]
        public IActionResult DeletePartial(int id)
        {
            ViewBag.VehiculoId = id;
            return PartialView("_DeletePartial");
        }

        [HttpGet]
        public async Task<IActionResult> VentaPartial(int id)
        {
            var response = await vehiculoServices.GetDetalleAsync(id);
            if (response.Success && response.Data != null && response.Data.Estado == 5)
            {
                var model = new ProcesarVentaViewModel
                {
                    VehiculoId = id,
                    FechaVenta = DateTime.Now
                };
                ViewBag.VehiculoId = id;
                ViewBag.VehiculoInfo = $"{response.Data.DescripcionCompleta} - {response.Data.Identificador}";
                ViewBag.PrecioLista = response.Data.PrecioLista;
                ViewBag.ClienteId = response.Data.ClienteId;
                ViewBag.ClienteNombre = response.Data.ClienteNombre;
                return PartialView("_VentaPartial", model);
            }
            return Content("<div class='alert alert-danger'>El vehículo debe estar en estado Reservado para procesar la venta</div>");
        }

        [HttpGet]
        public async Task<IActionResult> ReservarPartial(int id)
        {
            var detalle = await vehiculoServices.GetDetalleAsync(id);
            if (!detalle.Success || detalle.Data == null)
                return Content("<div class='alert alert-danger'>Vehículo no encontrado</div>");

            ViewBag.VehiculoId = id;
            ViewBag.VehiculoInfo = detalle.Data.DescripcionCompleta;
            return PartialView("_ReservarPartial");
        }

        // Data endpoints
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await vehiculoServices.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByCliente(int clienteId)
        {
            var response = await vehiculoServices.GetByClienteAsync(clienteId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByEstado(string estado)
        {
            var response = await vehiculoServices.GetByEstadoAsync(estado);
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

        [HttpGet]
        public async Task<IActionResult> GetMarcas()
        {
            var response = await marcaServices.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetSucursales()
        {
            var response = await sucursalServices.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        // CRUD
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehiculoViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(new { success = false, message = ObtenerErroresValidacion() });
            var response = await vehiculoServices.CreateAsync(model);
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
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var response = await vehiculoServices.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado([FromBody] CambiarEstadoRequest request)
        {
            if (!NombresEstado.ContainsKey(request.NuevoEstado))
                return BadRequest(new { success = false, message = "Estado no válido" });

            if (request.NuevoEstado == 5 && !User.IsInRole("JefeVentas"))
                return StatusCode(403, new { success = false, message = "Solo el rol 'Jefe Ventas' puede reservar vehículos." });

            var response = await vehiculoServices.CambiarEstadoAsync(request.VehiculoId, request.NuevoEstado, request.ClienteId, request.VendedorId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetVendedores()
        {
            var response = await apiClient.GetAsync<List<Models.UserRole.UserViewModels.UserViewModel>>("api/Auth/GetByDepartment/Ventas");
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CancelarReservasVencidas()
        {
            var response = await vehiculoServices.CancelarReservasVencidasAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarKilometraje([FromBody] ActualizarKilometrajeRequest request)
        {
            var response = await vehiculoServices.ActualizarKilometrajeAsync(request.VehiculoId, request.NuevoKilometraje);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarVenta([FromBody] ProcesarVentaViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(new { success = false, message = ObtenerErroresValidacion() });

            // 1. Obtener detalle actual
            var detalle = await vehiculoServices.GetDetalleAsync(model.VehiculoId);
            if (!detalle.Success || detalle.Data == null)
                return StatusCode(detalle.StatusCode, detalle);

            if (detalle.Data.Estado != 5)
                return BadRequest(new { success = false, message = "El vehículo debe estar en estado Reservado para procesar la venta" });

            // 2. Actualizar con datos de venta
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
                Estado = d.Estado,
                SucursalId = d.SucursalId,
                Procedencia = d.Procedencia,
                NumeroImportacion = d.NumeroImportacion,
                NumeroPoliza = d.NumeroPoliza,
                FechaIngresoPais = d.FechaIngresoPais,
                FechaRecepcion = d.FechaRecepcion,
                CostoImportacion = d.CostoImportacion,
                PrecioLista = d.PrecioLista,
                KilometrajeActual = d.KilometrajeActual,
                FechaPrimeraMatricula = d.FechaPrimeraMatricula,
                GarantiaHasta = d.GarantiaHasta,
                Observaciones = d.Observaciones,
                VendedorId = d.VendedorId,
                // Datos de venta
                ClienteId = model.ClienteId,
                PrecioVenta = model.PrecioVenta,
                FechaVenta = model.FechaVenta
            };

            var editResult = await vehiculoServices.EditAsync(editModel);
            if (!editResult.Success)
                return StatusCode(editResult.StatusCode, editResult);

            // 3. Cambiar estado a Vendido
            var estadoResult = await vehiculoServices.CambiarEstadoAsync(model.VehiculoId, 6);
            if (!estadoResult.Success)
                return StatusCode(estadoResult.StatusCode, new { success = false, message = $"Los datos de venta se guardaron pero hubo un error al cambiar el estado: {estadoResult.Message}" });

            return Ok(new { success = true, message = "Venta procesada exitosamente" });
        }

        [HttpGet]
        public IActionResult CargaMasivaPartial()
        {
            return PartialView("_CargaMasivaPartial");
        }

        [HttpPost]
        public async Task<IActionResult> CrearLote([FromBody] List<CreateVehiculoViewModel> vehiculos)
        {
            if (vehiculos == null || vehiculos.Count == 0)
                return BadRequest(new { success = false, message = "La lista de vehículos está vacía." });

            var response = await vehiculoServices.CrearLoteAsync(vehiculos);
            return StatusCode(response.StatusCode, response);
        }

        private string ObtenerErroresValidacion()
        {
            return string.Join(" | ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
        }

        private Dictionary<int, string> GetTransicionesValidas(int estadoActual)
        {
            var result = new Dictionary<int, string>();
            if (TransicionesPreVenta.TryGetValue(estadoActual, out var validas))
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
