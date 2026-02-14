using SmartAdmin.Models.Catalogo.Tecnico;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Interfaces
{
    public interface ITecnico
    {
        Task<ApiResponse<List<TecnicoViewModel>>> GetAllAsync();
        Task<ApiResponse<TecnicoViewModel>> GetDetails(int tecnicoId);
        Task<ApiResponse<List<TecnicoViewModel>>> GetBySucursalAsync(int sucursalId);
        Task<ApiResponse<TecnicoViewModel>> CreateAsync(CreateTecnicoViewModel model);
        Task<ApiResponse<TecnicoViewModel>> EditAsync(EditTecnicoViewModel model);
        Task<ApiResponse<bool>> DeleteAsync(int tecnicoId);
    }
}
