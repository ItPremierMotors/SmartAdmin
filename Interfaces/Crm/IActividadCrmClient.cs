using SmartAdmin.Models.Crm;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Interfaces.Crm
{
    public interface IActividadCrmClient
    {
        Task<ApiResponse<List<ActividadCrmViewModel>>> GetByLeadAsync(int leadId);
        Task<ApiResponse<List<ActividadCrmViewModel>>> GetByOportunidadAsync(int oportunidadId);
        Task<ApiResponse<List<ActividadCrmViewModel>>> GetPendientesAsync(string vendedorId);
        Task<ApiResponse<List<ActividadCrmViewModel>>> GetProximosContactosAsync(string fecha);
        Task<ApiResponse<ActividadCrmViewModel>> CreateAsync(CreateActividadCrmViewModel model);
        Task<ApiResponse<bool>> CompletarAsync(CompletarActividadViewModel model);
        Task<ApiResponse<bool>> CancelarAsync(int actividadId);
        Task<ApiResponse<bool>> DeleteAsync(int actividadId);
    }
}