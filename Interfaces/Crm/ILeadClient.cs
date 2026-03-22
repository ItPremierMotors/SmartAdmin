using SmartAdmin.Models.Crm;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Interfaces.Crm
{
    public interface ILeadClient
    {
        Task<ApiResponse<List<LeadViewModel>>> GetAllAsync();
        Task<ApiResponse<LeadViewModel>> GetByIdAsync(int leadId);
        Task<ApiResponse<List<LeadViewModel>>> GetBySucursalAsync(int sucursalId);
        Task<ApiResponse<List<LeadViewModel>>> GetByVendedorAsync(string vendedorId);
        Task<ApiResponse<List<LeadViewModel>>> GetByEstadoAsync(int estado);
        Task<ApiResponse<LeadViewModel>> CreateAsync(CreateLeadViewModel model);
        Task<ApiResponse<LeadViewModel>> EditAsync(EditLeadViewModel model);
        Task<ApiResponse<bool>> CalificarAsync(CalificarLeadViewModel model);
        Task<ApiResponse<bool>> DescartarAsync(DescartarLeadViewModel model);
        Task<ApiResponse<bool>> AsignarVendedorAsync(AsignarVendedorLeadViewModel model);
    }
}