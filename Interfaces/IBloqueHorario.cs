using SmartAdmin.Models.htpp;
using SmartAdmin.Models.Taller;

namespace SmartAdmin.Interfaces
{
    public interface IBloqueHorario
    {
        Task<ApiResponse<List<BloqueHorarioViewModel>>> GetByCapacidadIdAsync(int capacidadId);
        Task<ApiResponse<List<BloqueHorarioViewModel>>> GetByFechaAsync(DateTime fecha, int? sucursalId = null);
        Task<ApiResponse<List<BloqueHorarioViewModel>>> GetDisponiblesByFechaAsync(DateTime fecha, int? sucursalId = null);
        Task<ApiResponse<BloqueHorarioViewModel>> CreateAsync(CreateBloqueHorarioViewModel model);
        Task<ApiResponse<BloqueHorarioViewModel>> UpdateAsync(UpdateBloqueHorarioViewModel model);
        Task<ApiResponse<bool>> DeleteAsync(int bloqueId);
        Task<ApiResponse<List<BloqueHorarioViewModel>>> GenerarAutomaticosAsync(GenerarBloquesViewModel model);
    }
}
