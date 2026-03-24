using SmartAdmin.Interfaces;
using SmartAdmin.Interfaces.Crm;
using SmartAdmin.Models.Crm;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Services.Crm
{
    public class DashboardCrmClientServices : IDashboardCrmClient
    {
        private readonly IApiClient apiClient;

        public DashboardCrmClientServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        /// <summary>Construye la URL con filtros de periodo y sucursal.</summary>
        private static string BuildUrl(string endpoint, DateTime desde, DateTime hasta, int? sucursalId)
        {
            var url = $"api/DashboardCrm/{endpoint}?desde={desde:yyyy-MM-dd}&hasta={hasta:yyyy-MM-dd}";
            if (sucursalId.HasValue) url += $"&sucursalId={sucursalId.Value}";
            return url;
        }

        public async Task<ApiResponse<DashboardLeadsVM>> GetLeadsKpisAsync(DateTime desde, DateTime hasta, int? sucursalId)
            => await apiClient.GetAsync<DashboardLeadsVM>(BuildUrl("Leads", desde, hasta, sucursalId));

        public async Task<ApiResponse<DashboardPipelineVM>> GetPipelineKpisAsync(DateTime desde, DateTime hasta, int? sucursalId)
            => await apiClient.GetAsync<DashboardPipelineVM>(BuildUrl("Pipeline", desde, hasta, sucursalId));

        public async Task<ApiResponse<DashboardEquipoVM>> GetEquipoKpisAsync(DateTime desde, DateTime hasta, int? sucursalId)
            => await apiClient.GetAsync<DashboardEquipoVM>(BuildUrl("Equipo", desde, hasta, sucursalId));

        public async Task<ApiResponse<DashboardRetencionVM>> GetRetencionKpisAsync(DateTime desde, DateTime hasta, int? sucursalId)
            => await apiClient.GetAsync<DashboardRetencionVM>(BuildUrl("Retencion", desde, hasta, sucursalId));

        public async Task<ApiResponse<DashboardActividadVM>> GetActividadKpisAsync(DateTime desde, DateTime hasta, int? sucursalId)
            => await apiClient.GetAsync<DashboardActividadVM>(BuildUrl("Actividad", desde, hasta, sucursalId));

        public async Task<ApiResponse<DashboardOrigenVM>> GetOrigenKpisAsync(DateTime desde, DateTime hasta, int? sucursalId)
            => await apiClient.GetAsync<DashboardOrigenVM>(BuildUrl("Origen", desde, hasta, sucursalId));

        public async Task<ApiResponse<DashboardTiempoVM>> GetTiempoKpisAsync(DateTime desde, DateTime hasta, int? sucursalId)
            => await apiClient.GetAsync<DashboardTiempoVM>(BuildUrl("Tiempo", desde, hasta, sucursalId));
    }
}
