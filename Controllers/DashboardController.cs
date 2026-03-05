using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

        // =============================================
        // Proxy endpoints para el Dashboard Gerencial
        // =============================================

        [HttpGet]
        public async Task<IActionResult> Resumen()
        {
            var result = await _apiClient.GetAsync<object>("api/Dashboard/Resumen");
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Taller(DateTime fechaInicio, DateTime fechaFin)
        {
            var result = await _apiClient.GetAsync<object>(
                $"api/Dashboard/Taller?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}");
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Ventas(DateTime fechaInicio, DateTime fechaFin)
        {
            var result = await _apiClient.GetAsync<object>(
                $"api/Dashboard/Ventas?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}");
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Inventario()
        {
            var result = await _apiClient.GetAsync<object>("api/Dashboard/Inventario");
            return Json(result);
        }

        // =============================================
        // Dashboards de ejemplo (existentes)
        // =============================================

        public IActionResult ProjectManagement()
        {
            return View();
        }

        public IActionResult ControlCenter()
        {
            return View();
        }

        public IActionResult Subscription()
        {
            return View();
        }

        public IActionResult Marketing()
        {
            return View();
        }
    }
}
