using SmartAdmin.Interfaces;
using SmartAdmin.Models.htpp;
using SmartAdmin.Models.Taller;

namespace SmartAdmin.Services
{
    public class CapacidadTallerServices : ICapacidadTaller
    {
        private readonly IApiClient apiClient;

        public CapacidadTallerServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponse<CapacidadTallerViewModel>> GetByIdAsync(int capacidadId)
        {
            return await apiClient.GetAsync<CapacidadTallerViewModel>($"api/CapacidadTaller/GetById/{capacidadId}");
        }

        public async Task<ApiResponse<CapacidadTallerViewModel>> GetByFechaAsync(DateTime fecha, int? sucursalId = null)
        {
            var url = $"api/CapacidadTaller/GetByFecha/{fecha:yyyy-MM-dd}";
            if (sucursalId.HasValue) url += $"?sucursalId={sucursalId}";
            return await apiClient.GetAsync<CapacidadTallerViewModel>(url);
        }

        public async Task<ApiResponse<List<CapacidadTallerViewModel>>> GetByRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin, int? sucursalId = null)
        {
            var url = $"api/CapacidadTaller/GetByrangoFecha?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}";
            if (sucursalId.HasValue) url += $"&sucursalId={sucursalId}";
            return await apiClient.GetAsync<List<CapacidadTallerViewModel>>(url);
        }

        public async Task<ApiResponse<CapacidadTallerViewModel>> CreateAsync(CreateCapacidadTallerViewModel model)
        {
            return await apiClient.PostAsync<CapacidadTallerViewModel>("api/CapacidadTaller/create", model);
        }

        public async Task<ApiResponse<CapacidadTallerViewModel>> UpdateAsync(UpdateCapacidadTallerViewModel model)
        {
            return await apiClient.PutAsync<CapacidadTallerViewModel>("api/CapacidadTaller/Update", model);
        }

        public async Task<ApiResponse<bool>> BloquearDiaAsync(int capacidadId, string motivo)
        {
            return await apiClient.PostAsync<bool>($"api/CapacidadTaller/bloquear/{capacidadId}", motivo);
        }

        public async Task<ApiResponse<bool>> DesbloquearDiaAsync(int capacidadId)
        {
            return await apiClient.PostAsync<bool>($"api/CapacidadTaller/Desbloquear/{capacidadId}");
        }

        public async Task<ApiResponse<List<CapacidadTallerViewModel>>> GenerarSemanaAsync(DateTime fechaInicio, CreateCapacidadTallerViewModel plantilla)
        {
            return await apiClient.PostAsync<List<CapacidadTallerViewModel>>($"api/CapacidadTaller/generar-semana?fechaInicio={fechaInicio:yyyy-MM-dd}", plantilla);
        }
    }
}
