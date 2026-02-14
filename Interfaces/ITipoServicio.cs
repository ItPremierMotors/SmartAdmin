using SmartAdmin.Models.Catalogo.TipoServicio;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Interfaces
{
    public interface ITipoServicio
    {
        Task<ApiResponse<List<TipoServicioViewModel>>> GetAllAsync();
        Task<ApiResponse<TipoServicioViewModel>> GetDetails(int tipoServicioId);
        Task<ApiResponse<List<TipoServicioViewModel>>> GetByClasificacionAsync(string clasificacion);
        Task<ApiResponse<List<TipoServicioViewModel>>> GetWalkInAsync();
        Task<ApiResponse<TipoServicioViewModel>> CreateAsync(CreateTipoServicioViewModel model);
        Task<ApiResponse<TipoServicioViewModel>> EditAsync(EditTipoServicioViewModel model);
        Task<ApiResponse<bool>> DeleteAsync(int tipoServicioId);
    }
}
