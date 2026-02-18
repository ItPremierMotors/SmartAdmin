using SmartAdmin.Interfaces;
using SmartAdmin.Models.htpp;
using SmartAdmin.Models.Taller;

namespace SmartAdmin.Services
{
    public class CitaServices : ICita
    {
        private readonly IApiClient apiClient;

        public CitaServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponse<List<CitaViewModel>>> GetAllAsync()
        {
            return await apiClient.GetAsync<List<CitaViewModel>>("api/Citas/GetAll");
        }

        public async Task<ApiResponse<CitaViewModel>> GetByIdAsync(int citaId)
        {
            return await apiClient.GetAsync<CitaViewModel>($"api/Citas/GetById/{citaId}");
        }

        public async Task<ApiResponse<List<CitaViewModel>>> GetByFechaAsync(DateTime fecha, int? sucursalId = null)
        {
            var url = $"api/Citas/GetByFecha/{fecha:yyyy-MM-dd}";
            if (sucursalId.HasValue) url += $"?sucursalId={sucursalId}";
            return await apiClient.GetAsync<List<CitaViewModel>>(url);
        }

        public async Task<ApiResponse<List<CitaViewModel>>> GetByRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin, int? sucursalId = null)
        {
            var url = $"api/Citas/GetByRango?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}";
            if (sucursalId.HasValue) url += $"&sucursalId={sucursalId}";
            return await apiClient.GetAsync<List<CitaViewModel>>(url);
        }

        public async Task<ApiResponse<List<CitaViewModel>>> GetByClienteAsync(int clienteId)
        {
            return await apiClient.GetAsync<List<CitaViewModel>>($"api/Citas/GetByCliente/{clienteId}");
        }

        public async Task<ApiResponse<List<CitaViewModel>>> GetByVehiculoAsync(int vehiculoId)
        {
            return await apiClient.GetAsync<List<CitaViewModel>>($"api/Citas/GetByVehiculo/{vehiculoId}");
        }

        public async Task<ApiResponse<List<CitaViewModel>>> GetActivasDelDiaAsync(DateTime fecha, int? sucursalId = null)
        {
            var url = $"api/Citas/GetActivasDelDia/{fecha:yyyy-MM-dd}";
            if (sucursalId.HasValue) url += $"?sucursalId={sucursalId}";
            return await apiClient.GetAsync<List<CitaViewModel>>(url);
        }

        public async Task<ApiResponse<CitaViewModel>> AgendarAsync(CreateCitaViewModel model)
        {
            return await apiClient.PostAsync<CitaViewModel>("api/Citas/Agendar", model);
        }

        public async Task<ApiResponse<bool>> ConfirmarAsync(int citaId)
        {
            return await apiClient.PostAsync<bool>($"api/Citas/Confirmar/{citaId}");
        }

        public async Task<ApiResponse<bool>> CancelarAsync(CancelarCitaViewModel model)
        {
            return await apiClient.PostAsync<bool>("api/Citas/Cancelar", model);
        }

        public async Task<ApiResponse<bool>> MarcarNoShowAsync(int citaId)
        {
            return await apiClient.PostAsync<bool>($"api/Citas/MarcarNoShow/{citaId}");
        }

        public async Task<ApiResponse<bool>> IniciarAtencionAsync(int citaId)
        {
            return await apiClient.PostAsync<bool>($"api/Citas/IniciarAtencion/{citaId}");
        }

        public async Task<ApiResponse<bool>> CompletarAsync(int citaId)
        {
            return await apiClient.PostAsync<bool>($"api/Citas/Completar/{citaId}");
        }

        public async Task<ApiResponse<CitaViewModel>> TransferirAsync(TransferirCitaViewModel model)
        {
            return await apiClient.PostAsync<CitaViewModel>("api/Citas/Transferir", model);
        }
    }
}
