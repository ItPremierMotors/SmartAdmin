using SmartAdmin.Interfaces;
using SmartAdmin.Models.htpp;
using SmartAdmin.Models.Taller;

namespace SmartAdmin.Services
{
    public class BloqueHorarioServices : IBloqueHorario
    {
        private readonly IApiClient apiClient;

        public BloqueHorarioServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponse<List<BloqueHorarioViewModel>>> GetByCapacidadIdAsync(int capacidadId)
        {
            return await apiClient.GetAsync<List<BloqueHorarioViewModel>>($"api/BloquesHorario/GetByCapacidadId/{capacidadId}");
        }

        public async Task<ApiResponse<List<BloqueHorarioViewModel>>> GetByFechaAsync(DateTime fecha, int? sucursalId = null)
        {
            var url = $"api/BloquesHorario/GetByFecha/{fecha:yyyy-MM-dd}";
            if (sucursalId.HasValue) url += $"?sucursalId={sucursalId}";
            return await apiClient.GetAsync<List<BloqueHorarioViewModel>>(url);
        }

        public async Task<ApiResponse<List<BloqueHorarioViewModel>>> GetDisponiblesByFechaAsync(DateTime fecha, int? sucursalId = null)
        {
            var url = $"api/BloquesHorario/GetDisponibles/{fecha:yyyy-MM-dd}";
            if (sucursalId.HasValue) url += $"?sucursalId={sucursalId}";
            return await apiClient.GetAsync<List<BloqueHorarioViewModel>>(url);
        }

        public async Task<ApiResponse<BloqueHorarioViewModel>> CreateAsync(CreateBloqueHorarioViewModel model)
        {
            return await apiClient.PostAsync<BloqueHorarioViewModel>("api/BloquesHorario/Create", model);
        }

        public async Task<ApiResponse<BloqueHorarioViewModel>> UpdateAsync(UpdateBloqueHorarioViewModel model)
        {
            return await apiClient.PutAsync<BloqueHorarioViewModel>("api/BloquesHorario/Update", model);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int bloqueId)
        {
            return await apiClient.DeleteAsync<bool>($"api/BloquesHorario/Delete/{bloqueId}");
        }

        public async Task<ApiResponse<List<BloqueHorarioViewModel>>> GenerarAutomaticosAsync(GenerarBloquesViewModel model)
        {
            return await apiClient.PostAsync<List<BloqueHorarioViewModel>>("api/BloquesHorario/generar-automaticos", model);
        }
    }
}
