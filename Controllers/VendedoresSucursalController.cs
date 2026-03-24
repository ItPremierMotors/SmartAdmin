using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using SmartAdmin.Interfaces.Crm;
using SmartAdmin.Models.Crm;

namespace SmartAdmin.Controllers
{
    [Authorize]
    public class VendedoresSucursalController : Controller
    {
        private readonly IVendedorSucursalClient _client;
        private readonly IApiClient _apiClient;

        public VendedoresSucursalController(IVendedorSucursalClient client, IApiClient apiClient)
        {
            _client = client;
            _apiClient = apiClient;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetBySucursal(int sucursalId)
            => Json(await _client.GetBySucursalAsync(sucursalId));

        [HttpGet]
        public async Task<IActionResult> GetByVendedor(string vendedorId)
            => Json(await _client.GetByVendedorAsync(vendedorId));

        [HttpPost]
        public async Task<IActionResult> Asignar([FromBody] AsignarVendedorSucursalVM model)
            => Json(await _client.AsignarAsync(model));

        [HttpPost]
        public async Task<IActionResult> Activar([FromBody] int vendedorSucursalId)
            => Json(await _client.ActivarAsync(vendedorSucursalId));

        [HttpPost]
        public async Task<IActionResult> Desactivar([FromBody] int vendedorSucursalId)
            => Json(await _client.DesactivarAsync(vendedorSucursalId));

        // Catálogos para dropdowns
        [HttpGet]
        public async Task<IActionResult> GetSucursales()
            => Json(await _apiClient.GetAsync<object>("api/Sucursales/GetAll"));

        [HttpGet]
        public async Task<IActionResult> GetVendedores()
            => Json(await _apiClient.GetAsync<object>("api/Auth/GetByDepartment/Ventas"));
    }
}
