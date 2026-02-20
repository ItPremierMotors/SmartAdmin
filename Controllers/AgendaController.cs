using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using SmartAdmin.Models.Taller;

namespace SmartAdmin.Controllers
{
    public class AgendaController : Controller
    {
        private readonly ICita citaServices;
        private readonly IBloqueHorario bloqueServices;
        private readonly ICapacidadTaller capacidadServices;
        private readonly ISucursal sucursalServices;

        public AgendaController(ICita citaServices, IBloqueHorario bloqueServices, ICapacidadTaller capacidadServices, ISucursal sucursalServices)
        {
            this.citaServices = citaServices;
            this.bloqueServices = bloqueServices;
            this.capacidadServices = capacidadServices;
            this.sucursalServices = sucursalServices;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetSucursales()
        {
            var response = await sucursalServices.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        // Data endpoints
        [HttpGet]
        public async Task<IActionResult> GetByFecha(DateTime fecha, int? sucursalId = null)
        {
            var response = await citaServices.GetByFechaAsync(fecha, sucursalId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByRangoFechas(DateTime fechaInicio, DateTime fechaFin, int? sucursalId = null)
        {
            var response = await citaServices.GetByRangoFechasAsync(fechaInicio, fechaFin, sucursalId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetActivasDelDia(DateTime fecha, int? sucursalId = null)
        {
            var response = await citaServices.GetActivasDelDiaAsync(fecha, sucursalId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBloquesDisponibles(DateTime fecha, int? sucursalId = null)
        {
            var response = await bloqueServices.GetDisponiblesByFechaAsync(fecha, sucursalId);
            return StatusCode(response.StatusCode, response);
        }

        // Partials
        [HttpGet]
        public IActionResult AgendarPartial(DateTime? fecha, int? sucursalId = null)
        {
            ViewBag.FechaPreseleccionada = fecha?.ToString("yyyy-MM-dd") ?? DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.SucursalId = sucursalId;
            return PartialView("_AgendarCitaPartial");
        }

        [HttpGet]
        public async Task<IActionResult> DetalleCitaPartial(int citaId)
        {
            var response = await citaServices.GetByIdAsync(citaId);
            if (!response.Success || response.Data == null)
                return Content("<div class='alert alert-danger'>Cita no encontrada</div>");
            return PartialView("_DetalleCitaPartial", response.Data);
        }

        // Commands
        [HttpPost]
        public async Task<IActionResult> Agendar([FromBody] CreateCitaViewModel model)
        {
            var response = await citaServices.AgendarAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Confirmar([FromBody] int citaId)
        {
            var response = await citaServices.ConfirmarAsync(citaId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Cancelar([FromBody] CancelarCitaViewModel model)
        {
            var response = await citaServices.CancelarAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> MarcarNoShow([FromBody] int citaId)
        {
            var response = await citaServices.MarcarNoShowAsync(citaId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> IniciarAtencion([FromBody] int citaId)
        {
            var response = await citaServices.IniciarAtencionAsync(citaId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Completar([FromBody] int citaId)
        {
            var response = await citaServices.CompletarAsync(citaId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> TransferirPartial(int citaId)
        {
            var response = await citaServices.GetByIdAsync(citaId);
            if (!response.Success || response.Data == null)
                return Content("<div class='alert alert-danger'>Cita no encontrada</div>");
            return PartialView("_TransferirCitaPartial", response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Transferir([FromBody] TransferirCitaViewModel model)
        {
            var response = await citaServices.TransferirAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> ReprogramarPartial(int citaId)
        {
            var response = await citaServices.GetByIdAsync(citaId);
            if (!response.Success || response.Data == null)
                return Content("<div class='alert alert-danger'>Cita no encontrada</div>");
            return PartialView("_ReprogramarCitaPartial", response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Reprogramar([FromBody] ReprogramarCitaViewModel model)
        {
            var response = await citaServices.ReprogramarAsync(model);
            return StatusCode(response.StatusCode, response);
        }
    }
}
