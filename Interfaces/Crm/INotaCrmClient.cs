using SmartAdmin.Models.Crm;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Interfaces.Crm
{
    public interface INotaCrmClient
    {
        Task<ApiResponse<List<NotaCrmViewModel>>> GetByLeadAsync(int leadId);
        Task<ApiResponse<List<NotaCrmViewModel>>> GetByOportunidadAsync(int oportunidadId);
        Task<ApiResponse<NotaCrmViewModel>> CreateAsync(CreateNotaCrmViewModel model);
        Task<ApiResponse<bool>> DeleteAsync(int notaId);
    }
}