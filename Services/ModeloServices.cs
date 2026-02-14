using SmartAdmin.Interfaces;
using SmartAdmin.Models.Catalogo.Modelo;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Services
{
    public class ModeloServices : IModelo
    {
        private readonly IApiClient apiClient;
        public ModeloServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }
        public async Task<ApiResponse<List<ModeloViewModel>>> GetAllAsync()
        {
            return await apiClient.GetAsync<List<ModeloViewModel>>("api/Modelos/GetAll");
        }
        public async Task<ApiResponse<ModeloViewModel>> GetDetails(string modeloId)
        {
            return await apiClient.GetAsync<ModeloViewModel>($"api/Modelos/GetById/{modeloId}");
        }
        public async Task<ApiResponse<ModeloViewModel>> CreateAsync(CreateModeloViewModel model)
        {
            return await apiClient.PostAsync<ModeloViewModel>("api/Modelos/Create", model);
        }

        public async Task<ApiResponse<ModeloViewModel>> EditAsync(EditModeloViewModel model)
        {
            return await apiClient.PutAsync<ModeloViewModel>("api/Modelos/Update", model);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int modeloId)
        {
            return await apiClient.DeleteAsync<bool>($"api/Modelos/Delete/{modeloId}");
        }
        public async Task<ApiResponse<List<ModeloViewModel>>> GetByMarcaAsync(string marcaId)
        {
            // return apiClient.GetAsync<List<ModeloViewModel>>($"api/Modelos/GetByMarca/{marcaId}");
            return await apiClient.GetAsync<List<ModeloViewModel>>($"api/Modelos/GetByMarca/{marcaId}");
        }

    }
}
