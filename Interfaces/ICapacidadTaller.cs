using SmartAdmin.Models.htpp;
using SmartAdmin.Models.Taller;

namespace SmartAdmin.Interfaces
{
    public interface ICapacidadTaller
    {
        Task<ApiResponse<CapacidadTallerViewModel>> GetByIdAsync(int capacidadId);
        Task<ApiResponse<CapacidadTallerViewModel>> GetByFechaAsync(DateTime fecha, int? sucursalId = null);
        Task<ApiResponse<List<CapacidadTallerViewModel>>> GetByRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin, int? sucursalId = null);
        Task<ApiResponse<CapacidadTallerViewModel>> CreateAsync(CreateCapacidadTallerViewModel model);
        Task<ApiResponse<CapacidadTallerViewModel>> UpdateAsync(UpdateCapacidadTallerViewModel model);
        Task<ApiResponse<bool>> BloquearDiaAsync(int capacidadId, string motivo);
        Task<ApiResponse<bool>> DesbloquearDiaAsync(int capacidadId);
        Task<ApiResponse<List<CapacidadTallerViewModel>>> GenerarSemanaAsync(DateTime fechaInicio, CreateCapacidadTallerViewModel plantilla);
    }
}
