using SmartAdmin.Models.Crm;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Interfaces.Crm
{
    public interface IDashboardCrmClient
    {
        Task<ApiResponse<DashboardLeadsVM>> GetLeadsKpisAsync(DateTime desde, DateTime hasta, int? sucursalId);
        Task<ApiResponse<DashboardPipelineVM>> GetPipelineKpisAsync(DateTime desde, DateTime hasta, int? sucursalId);
        Task<ApiResponse<DashboardEquipoVM>> GetEquipoKpisAsync(DateTime desde, DateTime hasta, int? sucursalId);
        Task<ApiResponse<DashboardRetencionVM>> GetRetencionKpisAsync(DateTime desde, DateTime hasta, int? sucursalId);
        Task<ApiResponse<DashboardActividadVM>> GetActividadKpisAsync(DateTime desde, DateTime hasta, int? sucursalId);
        Task<ApiResponse<DashboardOrigenVM>> GetOrigenKpisAsync(DateTime desde, DateTime hasta, int? sucursalId);
        Task<ApiResponse<DashboardTiempoVM>> GetTiempoKpisAsync(DateTime desde, DateTime hasta, int? sucursalId);
    }
}
