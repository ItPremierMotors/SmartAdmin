using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using SmartAdmin.Interfaces.Crm;
using SmartAdmin.Models.Crm;
using SmartAdmin.Models.UserRole;

namespace SmartAdmin.Controllers
{
    public class LeadsController : Controller
    {
        private readonly ILeadClient leadClient;
        private readonly ISucursal sucursalServices;
        private readonly IApiClient apiClient;
        private readonly IActividadCrmClient actividadClient;
        private readonly INotaCrmClient  notaClient;

        public LeadsController(ILeadClient leadClient, ISucursal sucursalServices, IApiClient apiClient, IActividadCrmClient actividadClient, INotaCrmClient notaClient)
        {
            this.leadClient = leadClient;
            this.sucursalServices = sucursalServices;
            this.apiClient = apiClient;
            this.actividadClient = actividadClient;
            this.notaClient = notaClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        // ── Partials para modales ──

        [HttpGet]
        public IActionResult CreatePartial()
        {
            return PartialView("_CreatePartial", new CreateLeadViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> EditPartial(int id)
        {
            var response = await leadClient.GetByIdAsync(id);
            if (response.Success && response.Data != null)
            {
                var model = new EditLeadViewModel
                {
                    LeadId = response.Data.LeadId,
                    NombreCompleto = response.Data.NombreCompleto,
                    Telefono = response.Data.Telefono,
                    Email = response.Data.Email,
                    Empresa = response.Data.Empresa,
                    Ciudad = response.Data.Ciudad,
                    DetalleOrigen = response.Data.DetalleOrigen,
                    VehiculoInteres = response.Data.VehiculoInteres,
                    TipoVehiculoInteres = response.Data.TipoVehiculoInteres,
                    PresupuestoEstimado = response.Data.PresupuestoEstimado
                };
                return PartialView("_EditPartial", model);
            }
            return Content("<div class='alert alert-danger'>Lead no encontrado</div>");
        }

        [HttpGet]
        public async Task<IActionResult> DetailPartial(int id)
        {
            var response = await leadClient.GetByIdAsync(id);
            if (response.Success && response.Data != null)
            {
                return PartialView("_DetailPartial", response.Data);
            }
            return Content("<div class='alert alert-danger'>Lead no encontrado</div>");
        }
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            
          var response = await leadClient.GetByIdAsync(id);
            if (response.Success && response.Data != null)
            {
                
                return View(response.Data);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DescartarPartial(int id)
        {
            return PartialView("_DescartarPartial", new DescartarLeadViewModel { LeadId = id });
        }

        [HttpGet]
        public async Task<IActionResult> AsignarVendedorPartial(int id)
        {
            var lead = await leadClient.GetByIdAsync(id);
            var sucursalId = lead.Data?.SucursalId;
            return PartialView("_AsignarVendedorPartial", new AsignarVendedorLeadViewModel { LeadId = id, SucursalId = sucursalId });
        }

        // ── Endpoints AJAX (proxy a PremierFlow API) ──

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await leadClient.GetAllAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByEstado(int estado)
        {
            var response = await leadClient.GetByEstadoAsync(estado);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLeadViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await leadClient.CreateAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] EditLeadViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await leadClient.EditAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Calificar([FromBody] CalificarLeadViewModel model)
        {
            var response = await leadClient.CalificarAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Descartar([FromBody] DescartarLeadViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await leadClient.DescartarAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> AsignarVendedor([FromBody] AsignarVendedorLeadViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await leadClient.AsignarVendedorAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        // ── Endpoints auxiliares (proxy) ──

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
        
        //partial para actividades y notas en detalle del lead
        [HttpGet]
        public IActionResult ActividadPartial(int leadId)
        {
            var model = new CreateActividadCrmViewModel
            {
                LeadId = leadId
            };
            return PartialView("~/Views/Oportunidades/_ActividadPartial.cshtml", model); //reutilizo el mismo partial de oportunidades para mostrar actividades, ya que es igual el proceso
        }

        [HttpGet]
        public IActionResult CompletarActividadPartial(int id)
        {
            return PartialView("~/Views/Oportunidades/_CompletarActividadPartial.cshtml", new CompletarActividadViewModel { ActividadCrmId = id });  //reutilizo el mismo partial de oportunidades para completar actividad, ya que es igual el proceso
        }
        [HttpGet]
        public async Task<IActionResult> GetActividades(int leadId)
        {
            var response = await actividadClient.GetByLeadAsync(leadId);
            return StatusCode(response.StatusCode, response);
        }
         [HttpGet]
        public IActionResult NotaPartial(int leadId)
        {
            var model = new CreateNotaCrmViewModel
            {
                LeadId = leadId,
            };
            return PartialView("~/Views/Oportunidades/_NotaPartial.cshtml", model); //reutilizo el mismo partial de oportunidades para mostrar notas, ya que es igual el proceso
        }

        //Fin partial para actividades y notas en detalle del lead

        //endpoints para actividades y notas en detalle del lead
        [HttpPost]
        public async Task<IActionResult> CreateActividad([FromBody] CreateActividadCrmViewModel model)
        {
            var response = await actividadClient.CreateAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CompletarActividad([FromBody] CompletarActividadViewModel model)
        {
            var response = await actividadClient.CompletarAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CancelarActividad([FromBody] int actividadId)
        {
            var response = await actividadClient.CancelarAsync(actividadId);
            return StatusCode(response.StatusCode, response);
        }
        
        // ── Endpoints AJAX: Notas del Lead ──
        [HttpGet]
        public async Task<IActionResult> GetNotas(int leadId)
        {
            var response = await notaClient.GetByLeadAsync(leadId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNota([FromBody] CreateNotaCrmViewModel model)
        {
            var response = await notaClient.CreateAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNota([FromBody] int notaId)
        {
            var response = await notaClient.DeleteAsync(notaId);
            return StatusCode(response.StatusCode, response);
        }

        // ═══ ALERTAS LEADS FRÍOS ═══

        public IActionResult AlertasFrios()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAlertasFrios(int sucursalId)
        {
            var result = await leadClient.GetAlertasFriosAsync(sucursalId);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> MarcarContactado([FromBody] int leadId)
        {
            var result = await leadClient.MarcarContactadoAsync(leadId);
            return Json(result);
        }
    }
}