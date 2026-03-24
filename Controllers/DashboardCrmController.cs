using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Helpers;
using SmartAdmin.Interfaces;
using SmartAdmin.Interfaces.Crm;

namespace SmartAdmin.Controllers
{
    /// <summary>
    /// Dashboard CRM — 7 categorías de KPIs para el gerente de ventas.
    /// Usa IDashboardCrmClient para comunicarse con el backend API.
    /// </summary>
    [Authorize]
    public class DashboardCrmController : Controller
    {
        private readonly IDashboardCrmClient _crmClient;
        private readonly IApiClient _apiClient;

        public DashboardCrmController(IDashboardCrmClient crmClient, IApiClient apiClient)
        {
            _crmClient = crmClient;
            _apiClient = apiClient;
        }

        public IActionResult Index()
        {
            if (!User.TienePermiso("Dashboard.Ver"))
                return RedirectToAction("Index", "Home");
            return View();
        }

        // ═══════════════════════════════════════════════
        //  KPIs — cada endpoint usa IDashboardCrmClient
        // ═══════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> Leads(DateTime desde, DateTime hasta, int? sucursalId)
        {
            if (!User.TienePermiso("Dashboard.Ver")) return Forbid();
            return Json(await _crmClient.GetLeadsKpisAsync(desde, hasta, sucursalId));
        }

        [HttpGet]
        public async Task<IActionResult> Pipeline(DateTime desde, DateTime hasta, int? sucursalId)
        {
            if (!User.TienePermiso("Dashboard.Ver")) return Forbid();
            return Json(await _crmClient.GetPipelineKpisAsync(desde, hasta, sucursalId));
        }

        [HttpGet]
        public async Task<IActionResult> Equipo(DateTime desde, DateTime hasta, int? sucursalId)
        {
            if (!User.TienePermiso("Dashboard.Ver")) return Forbid();
            return Json(await _crmClient.GetEquipoKpisAsync(desde, hasta, sucursalId));
        }

        [HttpGet]
        public async Task<IActionResult> Retencion(DateTime desde, DateTime hasta, int? sucursalId)
        {
            if (!User.TienePermiso("Dashboard.Ver")) return Forbid();
            return Json(await _crmClient.GetRetencionKpisAsync(desde, hasta, sucursalId));
        }

        [HttpGet]
        public async Task<IActionResult> Actividad(DateTime desde, DateTime hasta, int? sucursalId)
        {
            if (!User.TienePermiso("Dashboard.Ver")) return Forbid();
            return Json(await _crmClient.GetActividadKpisAsync(desde, hasta, sucursalId));
        }

        [HttpGet]
        public async Task<IActionResult> Origen(DateTime desde, DateTime hasta, int? sucursalId)
        {
            if (!User.TienePermiso("Dashboard.Ver")) return Forbid();
            return Json(await _crmClient.GetOrigenKpisAsync(desde, hasta, sucursalId));
        }

        [HttpGet]
        public async Task<IActionResult> Tiempo(DateTime desde, DateTime hasta, int? sucursalId)
        {
            if (!User.TienePermiso("Dashboard.Ver")) return Forbid();
            return Json(await _crmClient.GetTiempoKpisAsync(desde, hasta, sucursalId));
        }

        // ═══════════════════════════════════════════════
        //  Catálogos para filtros
        // ═══════════════════════════════════════════════

        [HttpGet]
        public async Task<IActionResult> Sucursales()
        {
            return Json(await _apiClient.GetAsync<object>("api/Sucursales/GetAll"));
        }
    }
}
