using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Helpers;
using SmartAdmin.Interfaces;

namespace Smartadmin.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IApiClient _apiClient;

        public DashboardController(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public IActionResult Index()
        {
            if (!User.TienePermiso("Dashboard.Ver"))
                return RedirectToAction("Index", "Home");
            return View();
        }

        // =============================================
        // Proxy endpoints para Resumen Operativo
        // =============================================

        [HttpGet]
        public async Task<IActionResult> Resumen(int? sucursalId = null)
        {
            if (!User.TienePermiso("Dashboard.Ver")) return Forbid();
            var url = "api/Dashboard/Resumen";
            if (sucursalId.HasValue) url += $"?sucursalId={sucursalId.Value}";
            var result = await _apiClient.GetAsync<object>(url);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Taller(DateTime fechaInicio, DateTime fechaFin, int? sucursalId = null)
        {
            if (!User.TienePermiso("Dashboard.Ver")) return Forbid();
            var url = $"api/Dashboard/Taller?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}";
            if (sucursalId.HasValue) url += $"&sucursalId={sucursalId.Value}";
            var result = await _apiClient.GetAsync<object>(url);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Ventas(DateTime fechaInicio, DateTime fechaFin, int? sucursalId = null)
        {
            if (!User.TienePermiso("Dashboard.Ver")) return Forbid();
            var url = $"api/Dashboard/Ventas?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}";
            if (sucursalId.HasValue) url += $"&sucursalId={sucursalId.Value}";
            var result = await _apiClient.GetAsync<object>(url);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Inventario(int? sucursalId = null)
        {
            if (!User.TienePermiso("Dashboard.Ver")) return Forbid();
            var url = "api/Dashboard/Inventario";
            if (sucursalId.HasValue) url += $"?sucursalId={sucursalId.Value}";
            var result = await _apiClient.GetAsync<object>(url);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Sucursales()
        {
            if (!User.TienePermiso("Dashboard.Ver")) return Forbid();
            var result = await _apiClient.GetAsync<object>("api/Sucursales/GetAll");
            return Json(result);
        }
    }
}
