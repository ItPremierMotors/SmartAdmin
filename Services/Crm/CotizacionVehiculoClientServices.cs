using SmartAdmin.Interfaces;
using SmartAdmin.Interfaces.Crm;
using SmartAdmin.Models.Crm;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Services.Crm
{
    public class CotizacionVehiculoClientServices : ICotizacionVehiculoClient
    {
        private readonly IApiClient apiClient;

        public CotizacionVehiculoClientServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponse<List<CotizacionVehiculoViewModel>>> GetByOportunidadAsync(int oportunidadId)
        {
            return await apiClient.GetAsync<List<CotizacionVehiculoViewModel>>($"api/cotizacionesvehiculo/GetByOportunidad/{oportunidadId}");
        }

        public async Task<ApiResponse<CotizacionVehiculoViewModel>> CreateAsync(CreateCotizacionVehiculoViewModel model)
        {
            return await apiClient.PostAsync<CotizacionVehiculoViewModel>("api/cotizacionesvehiculo/Create", model);
        }

        public async Task<ApiResponse<bool>> AceptarAsync(int cotizacionId)
        {
            return await apiClient.PutAsync<bool>($"api/cotizacionesvehiculo/Aceptar/{cotizacionId}", new { });
        }

        public async Task<ApiResponse<bool>> RechazarAsync(int cotizacionId)
        {
            return await apiClient.PutAsync<bool>($"api/cotizacionesvehiculo/Rechazar/{cotizacionId}", new { });
        }
    }
}