using SmartAdmin.Models.Catalogo.Ubicacion;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Interfaces
{
    public interface IUbicacion
    {
        Task<ApiResponse<List<UbicacionViewModel>>> GetActivasAsync();
    }
}
