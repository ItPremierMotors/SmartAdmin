using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using SmartAdmin.Interfaces.Crm;
using SmartAdmin.Models.Crm;
using SmartAdmin.Models.UserRole;

namespace SmartAdmin.Controllers
{
    public class OportunidadesController : Controller
    {
        private readonly IOportunidadClient oportunidadClient;
        private readonly IActividadCrmClient actividadClient;
        private readonly INotaCrmClient notaClient;
        private readonly ICotizacionVehiculoClient cotizacionClient;
        private readonly ILeadClient leadClient;
        private readonly ISucursal sucursalServices;
        private readonly IVehiculo vehiculoServices;
        private readonly IApiClient apiClient;

        public OportunidadesController(
            IOportunidadClient oportunidadClient,
            IActividadCrmClient actividadClient,
            INotaCrmClient notaClient,
            ICotizacionVehiculoClient cotizacionClient,
            ILeadClient leadClient,
            ISucursal sucursalServices,
            IVehiculo vehiculoServices,
            IApiClient apiClient)
        {
            this.oportunidadClient = oportunidadClient;
            this.actividadClient = actividadClient;
            this.notaClient = notaClient;
            this.cotizacionClient = cotizacionClient;
            this.leadClient = leadClient;
            this.sucursalServices = sucursalServices;
            this.vehiculoServices = vehiculoServices;
            this.apiClient = apiClient;
        }

        // ── Vistas principales ──

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var response = await oportunidadClient.GetDetalleAsync(id);
            if (response.Success && response.Data != null)
            {
                return View(response.Data);
            }
            TempData["Error"] = "Oportunidad no encontrada";
            return RedirectToAction("Index");
        }

        // ── Partials para modales ──

        [HttpGet]
        public IActionResult CreatePartial()
        {
            return PartialView("_CreatePartial", new CreateOportunidadViewModel());
        }

        [HttpGet]
        public IActionResult CerrarPartial(int id, string tipo)
        {
            ViewBag.TipoCierre = tipo; // "ganada", "perdida", "cancelar"
            return PartialView("_CerrarPartial", new CerrarOportunidadViewModel { OportunidadId = id });
        }

        [HttpGet]
        public IActionResult ActividadPartial(int? leadId, int? oportunidadId)
        {
            var model = new CreateActividadCrmViewModel
            {
                LeadId = leadId,
                OportunidadId = oportunidadId
            };
            return PartialView("_ActividadPartial", model);
        }

        [HttpGet]
        public IActionResult CompletarActividadPartial(int id)
        {
            return PartialView("_CompletarActividadPartial", new CompletarActividadViewModel { ActividadCrmId = id });
        }

        [HttpGet]
        public IActionResult NotaPartial(int? leadId, int? oportunidadId)
        {
            var model = new CreateNotaCrmViewModel
            {
                LeadId = leadId,
                OportunidadId = oportunidadId
            };
            return PartialView("_NotaPartial", model);
        }

        [HttpGet]
        public IActionResult CotizacionPartial(int oportunidadId)
        {
            return PartialView("_CotizacionPartial", new CreateCotizacionVehiculoViewModel { OportunidadId = oportunidadId });
        }

        // ── Endpoints AJAX: Oportunidades ──

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await oportunidadClient.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetPipeline(int sucursalId)
        {
            var response = await oportunidadClient.GetPipelineAsync(sucursalId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByEtapa(int etapa)
        {
            var response = await oportunidadClient.GetByEtapaAsync(etapa);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOportunidadViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await oportunidadClient.CreateAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEtapa([FromBody] CambiarEtapaViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await oportunidadClient.CambiarEtapaAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> AvanzarEtapa(int id)
        {
            var response = await oportunidadClient.AvanzarEtapaAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> RetrocederEtapa(int id)
        {
            var response = await oportunidadClient.RetrocederEtapaAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UpdateOportunidadViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await oportunidadClient.UpdateAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CerrarGanada([FromBody] CerrarOportunidadViewModel model)
        {
            var response = await oportunidadClient.CerrarGanadaAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CerrarPerdida([FromBody] CerrarOportunidadViewModel model)
        {
            var response = await oportunidadClient.CerrarPerdidaAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Cancelar([FromBody] CerrarOportunidadViewModel model)
        {
            var response = await oportunidadClient.CancelarAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> VincularVehiculo([FromBody] VincularVehiculoViewModel model)
        {
            var response = await oportunidadClient.VincularVehiculoAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        // ── Endpoints AJAX: Actividades ──

        [HttpGet]
        public async Task<IActionResult> GetActividades(int oportunidadId)
        {
            var response = await actividadClient.GetByOportunidadAsync(oportunidadId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateActividad([FromBody] CreateActividadCrmViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await actividadClient.CreateAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CompletarActividad([FromBody] CompletarActividadViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await actividadClient.CompletarAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CancelarActividad([FromBody] int actividadId)
        {
            var response = await actividadClient.CancelarAsync(actividadId);
            return StatusCode(response.StatusCode, response);
        }

        // ── Endpoints AJAX: Notas ──

        [HttpGet]
        public async Task<IActionResult> GetNotas(int oportunidadId)
        {
            var response = await notaClient.GetByOportunidadAsync(oportunidadId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNota([FromBody] CreateNotaCrmViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await notaClient.CreateAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNota([FromBody] int notaId)
        {
            var response = await notaClient.DeleteAsync(notaId);
            return StatusCode(response.StatusCode, response);
        }

        // ── Endpoints AJAX: Cotizaciones ──

        [HttpGet]
        public async Task<IActionResult> GetCotizaciones(int oportunidadId)
        {
            var response = await cotizacionClient.GetByOportunidadAsync(oportunidadId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCotizacion([FromBody] CreateCotizacionVehiculoViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await cotizacionClient.CreateAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> AceptarCotizacion([FromBody] int cotizacionId)
        {
            var response = await cotizacionClient.AceptarAsync(cotizacionId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> RechazarCotizacion([FromBody] int cotizacionId)
        {
            var response = await cotizacionClient.RechazarAsync(cotizacionId);
            return StatusCode(response.StatusCode, response);
        }

        // ── Endpoints auxiliares (proxy) ──

        [HttpGet]
        public async Task<IActionResult> GetLeadsCalificados()
        {
            var response = await leadClient.GetByEstadoAsync(3); // EstadoLead.Calificado = 3
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetSucursales()
        {
            var response = await sucursalServices.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetVendedores()
        {
            var response = await apiClient.GetAsync<List<UserViewModels.UserViewModel>>("api/Auth/GetByDepartment/Ventas");
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetModelos()
        {
            var response = await apiClient.GetAsync<List<object>>("api/Modelos/GetAll");
            return StatusCode(response.StatusCode, response);
        }
    }
}