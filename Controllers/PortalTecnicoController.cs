using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using System.Security.Claims;

namespace SmartAdmin.Controllers
{
    [Authorize]
    public class PortalTecnicoController : Controller
    {
        private readonly ITecnico _tecnicoServices;
        private readonly IOrdenServicio _osServices;

        public PortalTecnicoController(ITecnico tecnicoServices, IOrdenServicio osServices)
        {
            _tecnicoServices = tecnicoServices;
            _osServices = osServices;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(usuarioId))
                return RedirectToAction("Login", "Auth");

            var tecnicoResponse = await _tecnicoServices.GetByUsuarioIdAsync(usuarioId);
            if (!tecnicoResponse.Success || tecnicoResponse.Data == null)
                return View("SinAcceso");

            return View(tecnicoResponse.Data);
        }

        // ─── Data Endpoints ───

        [HttpGet]
        public async Task<IActionResult> GetAsignacionesActivas(int tecnicoId)
        {
            var response = await _osServices.GetActivasByTecnicoIdAsync(tecnicoId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetHistorialVehiculo(int vehiculoId)
        {
            var response = await _osServices.GetHistorialVehiculoAsync(vehiculoId);
            return StatusCode(response.StatusCode, response);
        }

        // ─── Assignment Actions ───

        [HttpPost]
        public async Task<IActionResult> IniciarAsignacion([FromBody] int asignacionId)
        {
            var response = await _osServices.IniciarAsignacionAsync(asignacionId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> PausarAsignacion([FromBody] int asignacionId)
        {
            var response = await _osServices.PausarAsignacionAsync(asignacionId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> ReanudarAsignacion([FromBody] int asignacionId)
        {
            var response = await _osServices.ReanudarAsignacionAsync(asignacionId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CompletarAsignacion([FromBody] int asignacionId)
        {
            var response = await _osServices.CompletarAsignacionAsync(asignacionId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
