using SmartAdmin.Models.Enums;
using SmartAdmin.Models.htpp;
using SmartAdmin.Models.Taller;

namespace SmartAdmin.Interfaces
{
    public interface ICita
    {
        Task<ApiResponse<List<CitaViewModel>>> GetAllAsync();
        Task<ApiResponse<CitaViewModel>> GetByIdAsync(int citaId);
        Task<ApiResponse<List<CitaViewModel>>> GetByFechaAsync(DateTime fecha, int? sucursalId = null);
        Task<ApiResponse<List<CitaViewModel>>> GetByRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin, int? sucursalId = null);
        Task<ApiResponse<List<CitaViewModel>>> GetByClienteAsync(int clienteId);
        Task<ApiResponse<List<CitaViewModel>>> GetByVehiculoAsync(int vehiculoId);
        Task<ApiResponse<List<CitaViewModel>>> GetActivasDelDiaAsync(DateTime fecha, int? sucursalId = null);
        Task<ApiResponse<CitaViewModel>> AgendarAsync(CreateCitaViewModel model);
        Task<ApiResponse<bool>> ConfirmarAsync(int citaId);
        Task<ApiResponse<bool>> CancelarAsync(CancelarCitaViewModel model);
        Task<ApiResponse<bool>> MarcarNoShowAsync(int citaId);
        Task<ApiResponse<bool>> IniciarAtencionAsync(int citaId);
        Task<ApiResponse<bool>> CompletarAsync(int citaId);
        Task<ApiResponse<CitaViewModel>> TransferirAsync(TransferirCitaViewModel model);
        Task<ApiResponse<CitaViewModel>> ReprogramarAsync(ReprogramarCitaViewModel model);
    }
}
