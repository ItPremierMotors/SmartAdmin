using SmartAdmin.Interfaces;
using SmartAdmin.Models.Catalogo.TipoServicio;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Services
{
    public class TipoServicioServices : ITipoServicio
    {
        private readonly IApiClient apiClient;

        public TipoServicioServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponse<List<TipoServicioViewModel>>> GetAllAsync()
        {
            return await apiClient.GetAsync<List<TipoServicioViewModel>>("api/TiposServicio/GetAll");
        }

        public async Task<ApiResponse<TipoServicioViewModel>> GetDetails(int tipoServicioId)
        {
            return await apiClient.GetAsync<TipoServicioViewModel>($"api/TiposServicio/GetById/{tipoServicioId}");
        }

        public async Task<ApiResponse<List<TipoServicioViewModel>>> GetByClasificacionAsync(string clasificacion)
        {
            return await apiClient.GetAsync<List<TipoServicioViewModel>>($"api/TiposServicio/GetByClasificacion/{clasificacion}");
        }

        public async Task<ApiResponse<List<TipoServicioViewModel>>> GetWalkInAsync()
        {
            return await apiClient.GetAsync<List<TipoServicioViewModel>>("api/TiposServicio/GetWalkin");
        }

        public async Task<ApiResponse<TipoServicioViewModel>> CreateAsync(CreateTipoServicioViewModel model)
        {
            return await apiClient.PostAsync<TipoServicioViewModel>("api/TiposServicio/Create", model);
        }

        public async Task<ApiResponse<TipoServicioViewModel>> EditAsync(EditTipoServicioViewModel model)
        {
            return await apiClient.PutAsync<TipoServicioViewModel>("api/TiposServicio/Update", model);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int tipoServicioId)
        {
            return await apiClient.DeleteAsync<bool>($"api/TiposServicio/Delete/{tipoServicioId}");
        }
    }
}
