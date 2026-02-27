using SmartAdmin.Models.Catalogo.EstadoOs;
using SmartAdmin.Models.htpp;
using SmartAdmin.Models.Taller;

namespace SmartAdmin.Interfaces
{
    public interface IOrdenServicio
    {
        // ─── OrdenesServicio ───
        Task<ApiResponse<List<OrdenServicioViewModel>>> GetAllAsync();
        Task<ApiResponse<OrdenServicioViewModel>> GetByIdAsync(int osId);
        Task<ApiResponse<OrdenServicioDetalleViewModel>> GetDetalleAsync(int osId);
        Task<ApiResponse<List<OrdenServicioViewModel>>> GetAbiertasAsync(int? sucursalId = null);
        Task<ApiResponse<List<OrdenServicioViewModel>>> GetByFechaAsync(DateTime fecha, int? sucursalId = null);
        Task<ApiResponse<List<OrdenServicioViewModel>>> GetByRangoAsync(DateTime fechaInicio, DateTime fechaFin, int? sucursalId = null);
        Task<ApiResponse<List<OrdenServicioViewModel>>> GetByEstadoAsync(int estadoId, int? sucursalId = null);
        Task<ApiResponse<OrdenServicioViewModel>> CreateFromCitaAsync(CreateOsFromCitaViewModel model);
        Task<ApiResponse<OrdenServicioViewModel>> CreateWalkInAsync(CreateOsWalkInViewModel model);
        Task<ApiResponse<OrdenServicioViewModel>> UpdateAsync(UpdateOrdenServicioViewModel model);
        Task<ApiResponse<bool>> CambiarEstadoAsync(CambiarEstadoOsViewModel model);
        Task<ApiResponse<int?>> CerrarAsync(CerrarOsViewModel model);
        Task<ApiResponse<bool>> CancelarAsync(int osId, string motivo);
        Task<ApiResponse<bool>> RecalcularTotalesAsync(int osId);
        Task<ApiResponse<List<EstadoOsViewModel>>> GetTransicionesValidasAsync(int osId);

        // ─── OsServicios ───
        Task<ApiResponse<List<OsServicioViewModel>>> GetServiciosByOsIdAsync(int osId);
        Task<ApiResponse<OsServicioViewModel>> AgregarServicioAsync(AgregarServicioViewModel model);
        Task<ApiResponse<OsServicioViewModel>> UpdateServicioAsync(UpdateOsServicioViewModel model);
        Task<ApiResponse<bool>> QuitarServicioAsync(int osServicioId);
        Task<ApiResponse<bool>> IniciarTrabajoServicioAsync(int osServicioId);
        Task<ApiResponse<bool>> CompletarServicioAsync(int osServicioId);
        Task<ApiResponse<bool>> CancelarServicioAsync(int osServicioId);

        // ─── AsignacionesTecnico ───
        Task<ApiResponse<List<AsignacionTecnicoViewModel>>> GetAsignacionesByOsIdAsync(int osId);
        Task<ApiResponse<AsignacionTecnicoViewModel>> AsignarTecnicoAsync(CreateAsignacionViewModel model);
        Task<ApiResponse<bool>> IniciarAsignacionAsync(int asignacionId);
        Task<ApiResponse<bool>> PausarAsignacionAsync(int asignacionId);
        Task<ApiResponse<bool>> ReanudarAsignacionAsync(int asignacionId);
        Task<ApiResponse<bool>> CompletarAsignacionAsync(int asignacionId);
        Task<ApiResponse<AsignacionTecnicoViewModel>> ReasignarAsync(ReasignarViewModel model);
        Task<ApiResponse<bool>> CancelarAsignacionAsync(int asignacionId);
    }
}
