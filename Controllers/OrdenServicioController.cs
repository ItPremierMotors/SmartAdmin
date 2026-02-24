using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using SmartAdmin.Models.Taller;

namespace SmartAdmin.Controllers
{
    public class OrdenServicioController : Controller
    {
        private readonly IOrdenServicio osServices;
        private readonly IEstadoOs estadoServices;
        private readonly ITecnico tecnicoServices;
        private readonly ITipoServicio tipoServicioServices;
        private readonly ISucursal sucursalServices;

        public OrdenServicioController(
            IOrdenServicio osServices,
            IEstadoOs estadoServices,
            ITecnico tecnicoServices,
            ITipoServicio tipoServicioServices,
            ISucursal sucursalServices)
        {
            this.osServices = osServices;
            this.estadoServices = estadoServices;
            this.tecnicoServices = tecnicoServices;
            this.tipoServicioServices = tipoServicioServices;
            this.sucursalServices = sucursalServices;
        }

        // ─── Vistas ───

        public IActionResult Index() => View();

        public async Task<IActionResult> Detalle(int id)
        {
            var response = await osServices.GetDetalleAsync(id);
            if (!response.Success || response.Data == null)
                return RedirectToAction("Index");
            return View(response.Data);
        }

        public async Task<IActionResult> Reporte(int id)
        {
            var detalleResp = await osServices.GetDetalleAsync(id);
            if (!detalleResp.Success || detalleResp.Data == null)
                return RedirectToAction("Index");

            var serviciosResp = await osServices.GetServiciosByOsIdAsync(id);

            var model = new ReporteOsViewModel
            {
                Detalle = detalleResp.Data,
                Servicios = serviciosResp.Data ?? new List<OsServicioViewModel>()
            };

            return View(model);
        }

        // ─── Partials ───

        [HttpGet]
        public IActionResult AgregarServicioPartial(int osId)
        {
            ViewBag.OsId = osId;
            return PartialView("_AgregarServicioPartial");
        }

        [HttpGet]
        public IActionResult AsignarTecnicoPartial(int osId)
        {
            ViewBag.OsId = osId;
            return PartialView("_AsignarTecnicoPartial");
        }

        [HttpGet]
        public async Task<IActionResult> CambiarEstadoPartial(int osId)
        {
            var response = await osServices.GetByIdAsync(osId);
            if (!response.Success || response.Data == null)
                return Content("<div class='alert alert-danger'>Orden no encontrada</div>");
            return PartialView("_CambiarEstadoPartial", response.Data);
        }

        // ─── Data endpoints ───

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await osServices.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAbiertas(int? sucursalId = null)
        {
            var response = await osServices.GetAbiertasAsync(sucursalId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByFecha(DateTime fecha, int? sucursalId = null)
        {
            var response = await osServices.GetByFechaAsync(fecha, sucursalId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByRango(DateTime fechaInicio, DateTime fechaFin, int? sucursalId = null)
        {
            var response = await osServices.GetByRangoAsync(fechaInicio, fechaFin, sucursalId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByEstado(int estadoId, int? sucursalId = null)
        {
            var response = await osServices.GetByEstadoAsync(estadoId, sucursalId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetDetalle(int osId)
        {
            var response = await osServices.GetDetalleAsync(osId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetEstados()
        {
            var response = await estadoServices.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetTransicionesValidas(int osId)
        {
            var response = await osServices.GetTransicionesValidasAsync(osId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetTecnicos()
        {
            var response = await tecnicoServices.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetTiposServicio()
        {
            var response = await tipoServicioServices.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetSucursales()
        {
            var response = await sucursalServices.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        // ─── Servicios de la OS ───

        [HttpGet]
        public async Task<IActionResult> GetServicios(int osId)
        {
            var response = await osServices.GetServiciosByOsIdAsync(osId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarServicio([FromBody] AgregarServicioViewModel model)
        {
            var response = await osServices.AgregarServicioAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateServicio([FromBody] UpdateOsServicioViewModel model)
        {
            var response = await osServices.UpdateServicioAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> QuitarServicio([FromBody] int osServicioId)
        {
            var response = await osServices.QuitarServicioAsync(osServicioId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> IniciarTrabajoServicio([FromBody] int osServicioId)
        {
            var response = await osServices.IniciarTrabajoServicioAsync(osServicioId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CompletarServicio([FromBody] int osServicioId)
        {
            var response = await osServices.CompletarServicioAsync(osServicioId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CancelarServicio([FromBody] int osServicioId)
        {
            var response = await osServices.CancelarServicioAsync(osServicioId);
            return StatusCode(response.StatusCode, response);
        }

        // ─── Asignaciones de Tecnico ───

        [HttpGet]
        public async Task<IActionResult> GetAsignaciones(int osId)
        {
            var response = await osServices.GetAsignacionesByOsIdAsync(osId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> AsignarTecnico([FromBody] CreateAsignacionViewModel model)
        {
            var response = await osServices.AsignarTecnicoAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> IniciarAsignacion([FromBody] int asignacionId)
        {
            var response = await osServices.IniciarAsignacionAsync(asignacionId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> PausarAsignacion([FromBody] int asignacionId)
        {
            var response = await osServices.PausarAsignacionAsync(asignacionId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> ReanudarAsignacion([FromBody] int asignacionId)
        {
            var response = await osServices.ReanudarAsignacionAsync(asignacionId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CompletarAsignacion([FromBody] int asignacionId)
        {
            var response = await osServices.CompletarAsignacionAsync(asignacionId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Reasignar([FromBody] ReasignarViewModel model)
        {
            var response = await osServices.ReasignarAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CancelarAsignacion([FromBody] int asignacionId)
        {
            var response = await osServices.CancelarAsignacionAsync(asignacionId);
            return StatusCode(response.StatusCode, response);
        }

        // ─── Comandos OS ───

        [HttpPost]
        public async Task<IActionResult> CambiarEstado([FromBody] CambiarEstadoOsViewModel model)
        {
            var response = await osServices.CambiarEstadoAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Cerrar([FromBody] CerrarOsViewModel model)
        {
            var response = await osServices.CerrarAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Cancelar([FromBody] CancelarOsViewModel model)
        {
            var response = await osServices.CancelarAsync(model.OsId, model.Motivo);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> RecalcularTotales([FromBody] int osId)
        {
            var response = await osServices.RecalcularTotalesAsync(osId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
