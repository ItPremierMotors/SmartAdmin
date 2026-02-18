using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using SmartAdmin.Models.Taller;

namespace SmartAdmin.Controllers
{
    public class CapacidadController : Controller
    {
        private readonly ICapacidadTaller capacidadServices;
        private readonly IBloqueHorario bloqueServices;
        private readonly ISucursal sucursalServices;

        public CapacidadController(ICapacidadTaller capacidadServices, IBloqueHorario bloqueServices, ISucursal sucursalServices)
        {
            this.capacidadServices = capacidadServices;
            this.bloqueServices = bloqueServices;
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
        public async Task<IActionResult> GetByRangoFechas(DateTime fechaInicio, DateTime fechaFin, int? sucursalId = null)
        {
            var response = await capacidadServices.GetByRangoFechasAsync(fechaInicio, fechaFin, sucursalId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByFecha(DateTime fecha, int? sucursalId = null)
        {
            var response = await capacidadServices.GetByFechaAsync(fecha, sucursalId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetBloquesByCapacidad(int capacidadId)
        {
            var response = await bloqueServices.GetByCapacidadIdAsync(capacidadId);
            return StatusCode(response.StatusCode, response);
        }

        // Partials
        [HttpGet]
        public IActionResult GenerarSemanaPartial()
        {
            return PartialView("_GenerarSemanaPartial");
        }

        [HttpGet]
        public async Task<IActionResult> DetalleDiaPartial(DateTime fecha, int? sucursalId = null)
        {
            var response = await capacidadServices.GetByFechaAsync(fecha, sucursalId);
            ViewBag.Fecha = fecha;
            if (response.Success && response.Data != null)
            {
                var bloques = await bloqueServices.GetByCapacidadIdAsync(response.Data.CapacidadId);
                ViewBag.Bloques = bloques.Success ? bloques.Data : new List<BloqueHorarioViewModel>();
                return PartialView("_DetalleDiaPartial", response.Data);
            }
            ViewBag.Bloques = new List<BloqueHorarioViewModel>();
            return PartialView("_DetalleDiaPartial", null);
        }

        [HttpGet]
        public async Task<IActionResult> EditPartial(int capacidadId)
        {
            var response = await capacidadServices.GetByIdAsync(capacidadId);
            if (!response.Success || response.Data == null)
                return Content("<div class='alert alert-danger'>Capacidad no encontrada</div>");
            return PartialView("_EditPartial", response.Data);
        }

        [HttpGet]
        public IActionResult GenerarBloquesPartial(int capacidadId)
        {
            ViewBag.CapacidadId = capacidadId;
            return PartialView("_GenerarBloquesPartial");
        }

        // Commands
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCapacidadTallerViewModel model)
        {
            var response = await capacidadServices.CreateAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UpdateCapacidadTallerViewModel model)
        {
            var response = await capacidadServices.UpdateAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> GenerarSemana([FromBody] GenerarSemanaRequest request)
        {
            var response = await capacidadServices.GenerarSemanaAsync(request.FechaInicio, request.Plantilla);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> GenerarBloques([FromBody] GenerarBloquesViewModel model)
        {
            var response = await bloqueServices.GenerarAutomaticosAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBloque([FromBody] CreateBloqueHorarioViewModel model)
        {
            var response = await bloqueServices.CreateAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBloque([FromBody] UpdateBloqueHorarioViewModel model)
        {
            var response = await bloqueServices.UpdateAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBloque([FromBody] int bloqueId)
        {
            var response = await bloqueServices.DeleteAsync(bloqueId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> BloquearDia([FromBody] BloquearDiaRequest request)
        {
            var response = await capacidadServices.BloquearDiaAsync(request.CapacidadId, request.Motivo);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> DesbloquearDia([FromBody] int capacidadId)
        {
            var response = await capacidadServices.DesbloquearDiaAsync(capacidadId);
            return StatusCode(response.StatusCode, response);
        }
    }

    // Request models
    public class GenerarSemanaRequest
    {
        public DateTime FechaInicio { get; set; }
        public CreateCapacidadTallerViewModel Plantilla { get; set; } = null!;
    }

    public class BloquearDiaRequest
    {
        public int CapacidadId { get; set; }
        public string Motivo { get; set; } = null!;
    }
}
