using SmartAdmin.Interfaces;
using SmartAdmin.Models.Catalogo.Marca;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Services
{
    public class MarcaServices : IMarca
    {
        private readonly IApiClient apiClient;
        public MarcaServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }
        public async Task<ApiResponse<List<MarcaViewModel>>> GetAllAsync()
        {
            return await apiClient.GetAsync<List<MarcaViewModel>>("api/Marcas/GetAll");
        }
        public async Task<ApiResponse<MarcaViewModel>> GetDetails(string marcaId)
        {
            return await apiClient.GetAsync<MarcaViewModel>($"api/Marcas/GetById/{marcaId}");
        }
        public async Task<ApiResponse<MarcaViewModel>> CreateAsync(CreateMarcaViewModel model)
        {
                return await apiClient.PostAsync<MarcaViewModel>("api/Marcas/Create", model);
        }

        public Task<ApiResponse<MarcaViewModel>> EditAsync(EditMarcaViewModel model)
        {
            return apiClient.PutAsync<MarcaViewModel>("api/Marcas/Update", model);
        }

        public Task<ApiResponse<bool>> DeleteAsync(int marcaId)
        {
            return apiClient.DeleteAsync<bool>($"api/Marcas/Delete/{marcaId}");
        }
      
    }
}
