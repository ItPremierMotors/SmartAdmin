using SmartAdmin.Interfaces;
using SmartAdmin.Models.Catalogo.Tecnico;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Services
{
    public class TecnicoServices : ITecnico
    {
        private readonly IApiClient apiClient;

        public TecnicoServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponse<List<TecnicoViewModel>>> GetAllAsync()
        {
            return await apiClient.GetAsync<List<TecnicoViewModel>>("api/Tecnicos/GetAll");
        }

        public async Task<ApiResponse<TecnicoViewModel>> GetDetails(int tecnicoId)
        {
            return await apiClient.GetAsync<TecnicoViewModel>($"api/Tecnicos/GetById/{tecnicoId}");
        }

        public async Task<ApiResponse<List<TecnicoViewModel>>> GetBySucursalAsync(int sucursalId)
        {
            return await apiClient.GetAsync<List<TecnicoViewModel>>($"api/Tecnicos/GetBySucursal/{sucursalId}");
        }

        public async Task<ApiResponse<TecnicoViewModel>> CreateAsync(CreateTecnicoViewModel model)
        {
            return await apiClient.PostAsync<TecnicoViewModel>("api/Tecnicos/Create", model);
        }

        public async Task<ApiResponse<TecnicoViewModel>> EditAsync(EditTecnicoViewModel model)
        {
            return await apiClient.PutAsync<TecnicoViewModel>("api/Tecnicos/Update", model);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int tecnicoId)
        {
            return await apiClient.DeleteAsync<bool>($"api/Tecnicos/Delete/{tecnicoId}");
        }

        public async Task<ApiResponse<TecnicoViewModel>> GetByUsuarioIdAsync(string usuarioId)
        {
            return await apiClient.GetAsync<TecnicoViewModel>($"api/Tecnicos/GetByUsuarioId/{usuarioId}");
        }
    }
}
