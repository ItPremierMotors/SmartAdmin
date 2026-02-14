using SmartAdmin.Models.Catalogo.EstadoOs;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Interfaces
{
    public interface IEstadoOs
    {
        Task<ApiResponse<List<EstadoOsViewModel>>> GetAllAsync();
        Task<ApiResponse<EstadoOsViewModel>> GetDetails(int estadoId);
    }
}
