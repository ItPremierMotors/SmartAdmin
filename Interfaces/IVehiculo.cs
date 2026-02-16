using SmartAdmin.Models.htpp;
using SmartAdmin.Models.Vehiculo;

namespace SmartAdmin.Interfaces
{
    public interface IVehiculo
    {
        Task<ApiResponse<List<VehiculoViewModel>>> GetAllAsync();
        Task<ApiResponse<VehiculoViewModel>> GetByIdAsync(int id);
        Task<ApiResponse<VehiculoDetalleViewModel>> GetDetalleAsync(int id);
        Task<ApiResponse<List<VehiculoViewModel>>> GetByClienteAsync(int clienteId);
        Task<ApiResponse<List<VehiculoViewModel>>> GetByEstadoAsync(string estado);
        Task<ApiResponse<List<VehiculoViewModel>>> SearchAsync(string term);
        Task<ApiResponse<VehiculoViewModel>> CreateAsync(CreateVehiculoViewModel model);
        Task<ApiResponse<VehiculoViewModel>> EditAsync(EditVehiculoViewModel model);
        Task<ApiResponse<bool>> DeleteAsync(int id);
        Task<ApiResponse<bool>> CambiarEstadoAsync(int vehiculoId, int nuevoEstado, int? clienteId = null, string? vendedorId = null);
        Task<ApiResponse<bool>> ActualizarKilometrajeAsync(int vehiculoId, int nuevoKm);
        Task<ApiResponse<int>> CancelarReservasVencidasAsync();
        Task<ApiResponse<List<VehiculoViewModel>>> GetBySucursalAsync(int sucursalId);
        Task<ApiResponse<VehiculoViewModel>> GetByVinAsync(string vin);
        Task<ApiResponse<VehiculoViewModel>> GetByPlacaAsync(string placa);
        Task<ApiResponse<List<VehiculoViewModel>>> GetDisponiblesParaVentaAsync();
    }
}
