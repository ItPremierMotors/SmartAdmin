using SmartAdmin.Interfaces;
using SmartAdmin.Interfaces.Crm;
using SmartAdmin.Models.Crm;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Services.Crm
{
    public class VendedorSucursalClientServices : IVendedorSucursalClient
    {
        private readonly IApiClient apiClient;

        public VendedorSucursalClientServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponse<VendedorSucursalVM>> AsignarAsync(AsignarVendedorSucursalVM model)
            => await apiClient.PostAsync<VendedorSucursalVM>("api/VendedoresSucursal/Asignar", model);

        public async Task<ApiResponse<bool>> ActivarAsync(int vendedorSucursalId)
            => await apiClient.PutAsync<bool>($"api/VendedoresSucursal/Activar/{vendedorSucursalId}", new { });

        public async Task<ApiResponse<bool>> DesactivarAsync(int vendedorSucursalId)
            => await apiClient.PutAsync<bool>($"api/VendedoresSucursal/Desactivar/{vendedorSucursalId}", new { });

        public async Task<ApiResponse<List<VendedorSucursalVM>>> GetBySucursalAsync(int sucursalId)
            => await apiClient.GetAsync<List<VendedorSucursalVM>>($"api/VendedoresSucursal/GetBySucursal/{sucursalId}");

        public async Task<ApiResponse<List<VendedorSucursalVM>>> GetByVendedorAsync(string vendedorId)
            => await apiClient.GetAsync<List<VendedorSucursalVM>>($"api/VendedoresSucursal/GetByVendedor/{vendedorId}");
    }
}
