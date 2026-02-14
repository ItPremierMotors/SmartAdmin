using SmartAdmin.Models.Catalogo.Sucursal;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Interfaces
{
    public interface ISucursal
    {
        Task<ApiResponse<List<SucursalViewModel>>> GetAllAsync();
    }
}
