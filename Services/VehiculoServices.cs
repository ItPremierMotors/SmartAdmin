using SmartAdmin.Interfaces;
using SmartAdmin.Models.htpp;
using SmartAdmin.Models.Vehiculo;

namespace SmartAdmin.Services
{
    public class VehiculoServices : IVehiculo
    {
        private readonly IApiClient apiClient;

        public VehiculoServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponse<List<VehiculoViewModel>>> GetAllAsync()
        {
            return await apiClient.GetAsync<List<VehiculoViewModel>>("api/Vehiculos/GetAll");
        }

        public async Task<ApiResponse<VehiculoViewModel>> GetByIdAsync(int id)
        {
            return await apiClient.GetAsync<VehiculoViewModel>($"api/Vehiculos/GetById/{id}");
        }

        public async Task<ApiResponse<VehiculoDetalleViewModel>> GetDetalleAsync(int id)
        {
            return await apiClient.GetAsync<VehiculoDetalleViewModel>($"api/Vehiculos/GetDetalle/{id}");
        }

        public async Task<ApiResponse<List<VehiculoViewModel>>> GetByClienteAsync(int clienteId)
        {
            return await apiClient.GetAsync<List<VehiculoViewModel>>($"api/Vehiculos/GetByCliente/{clienteId}");
        }

        public async Task<ApiResponse<List<VehiculoViewModel>>> GetByEstadoAsync(string estado)
        {
            return await apiClient.GetAsync<List<VehiculoViewModel>>($"api/Vehiculos/GetByEstado/{estado}");
        }

        public async Task<ApiResponse<List<VehiculoViewModel>>> SearchAsync(string term)
        {
            return await apiClient.GetAsync<List<VehiculoViewModel>>($"api/Vehiculos/search/{term}");
        }

        public async Task<ApiResponse<VehiculoViewModel>> CreateAsync(CreateVehiculoViewModel model)
        {
            return await apiClient.PostAsync<VehiculoViewModel>("api/Vehiculos/Create", model);
        }

        public async Task<ApiResponse<VehiculoViewModel>> EditAsync(EditVehiculoViewModel model)
        {
            return await apiClient.PutAsync<VehiculoViewModel>("api/Vehiculos/Update", model);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            return await apiClient.DeleteAsync<bool>($"api/Vehiculos/Delete/{id}");
        }

        public async Task<ApiResponse<bool>> CambiarEstadoAsync(int vehiculoId, string nuevoEstado)
        {
            return await apiClient.PatchAsync<bool>($"api/Vehiculos/CambiarEstado/{vehiculoId}", nuevoEstado);
        }

        public async Task<ApiResponse<bool>> ActualizarKilometrajeAsync(int vehiculoId, int nuevoKm)
        {
            return await apiClient.PatchAsync<bool>($"api/Vehiculos/ActualizarKilometraje/{vehiculoId}", nuevoKm);
        }

        public async Task<ApiResponse<List<VehiculoViewModel>>> GetBySucursalAsync(int sucursalId)
        {
            return await apiClient.GetAsync<List<VehiculoViewModel>>($"api/Vehiculos/GetBySucursal/{sucursalId}");
        }

        public async Task<ApiResponse<VehiculoViewModel>> GetByVinAsync(string vin)
        {
            return await apiClient.GetAsync<VehiculoViewModel>($"api/Vehiculos/GetByVin/{vin}");
        }

        public async Task<ApiResponse<VehiculoViewModel>> GetByPlacaAsync(string placa)
        {
            return await apiClient.GetAsync<VehiculoViewModel>($"api/Vehiculos/GetByPlaca/{placa}");
        }

        public async Task<ApiResponse<List<VehiculoViewModel>>> GetDisponiblesParaVentaAsync()
        {
            return await apiClient.GetAsync<List<VehiculoViewModel>>("api/Vehiculos/GetDisponiblesParaVenta");
        }
    }
}
