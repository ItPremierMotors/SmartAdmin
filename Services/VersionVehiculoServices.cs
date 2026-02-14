using SmartAdmin.Interfaces;
using SmartAdmin.Models.Catalogo.VersionVehiculo;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Services
{
    public class VersionVehiculoServices : IVersionVehiculo
    {
        private readonly IApiClient apiClient;

        public VersionVehiculoServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponse<List<VersionVehiculoViewModel>>> GetAllAsync()
        {
            return await apiClient.GetAsync<List<VersionVehiculoViewModel>>("api/VersionesVehiculo/GetAll");
        }

        public async Task<ApiResponse<VersionVehiculoViewModel>> GetDetails(int versionId)
        {
            return await apiClient.GetAsync<VersionVehiculoViewModel>($"api/VersionesVehiculo/GetById/{versionId}");
        }

        public async Task<ApiResponse<List<VersionVehiculoViewModel>>> GetByModeloAsync(int modeloId)
        {
            return await apiClient.GetAsync<List<VersionVehiculoViewModel>>($"api/VersionesVehiculo/GetByModelo/{modeloId}");
        }

        public async Task<ApiResponse<VersionVehiculoViewModel>> CreateAsync(CreateVersionVehiculoViewModel model)
        {
            return await apiClient.PostAsync<VersionVehiculoViewModel>("api/VersionesVehiculo/Create", model);
        }

        public async Task<ApiResponse<VersionVehiculoViewModel>> EditAsync(EditVersionVehiculoViewModel model)
        {
            return await apiClient.PutAsync<VersionVehiculoViewModel>("api/VersionesVehiculo/Update", model);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int versionId)
        {
            return await apiClient.DeleteAsync<bool>($"api/VersionesVehiculo/Delete/{versionId}");
        }
    }
}
