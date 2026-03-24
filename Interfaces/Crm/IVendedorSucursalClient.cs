using SmartAdmin.Models.Crm;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Interfaces.Crm
{
    public interface IVendedorSucursalClient
    {
        Task<ApiResponse<VendedorSucursalVM>> AsignarAsync(AsignarVendedorSucursalVM model);
        Task<ApiResponse<bool>> ActivarAsync(int vendedorSucursalId);
        Task<ApiResponse<bool>> DesactivarAsync(int vendedorSucursalId);
        Task<ApiResponse<List<VendedorSucursalVM>>> GetBySucursalAsync(int sucursalId);
        Task<ApiResponse<List<VendedorSucursalVM>>> GetByVendedorAsync(string vendedorId);
    }
}
