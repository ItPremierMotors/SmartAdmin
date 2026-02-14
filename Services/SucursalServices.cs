using SmartAdmin.Interfaces;
using SmartAdmin.Models.Catalogo.Sucursal;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Services
{
    public class SucursalServices : ISucursal
    {
        private readonly IApiClient apiClient;

        public SucursalServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponse<List<SucursalViewModel>>> GetAllAsync()
        {
            return await apiClient.GetAsync<List<SucursalViewModel>>("api/Sucursales/GetAll");
        }
    }
}
