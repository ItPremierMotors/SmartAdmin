using SmartAdmin.Interfaces;
using SmartAdmin.Models.Catalogo.EstadoOs;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Services
{
    public class EstadoOsServices : IEstadoOs
    {
        private readonly IApiClient apiClient;

        public EstadoOsServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponse<List<EstadoOsViewModel>>> GetAllAsync()
        {
            return await apiClient.GetAsync<List<EstadoOsViewModel>>("api/EstadosOs/GetAlL");
        }

        public async Task<ApiResponse<EstadoOsViewModel>> GetDetails(int estadoId)
        {
            return await apiClient.GetAsync<EstadoOsViewModel>($"api/EstadosOs/GetById/{estadoId}");
        }
    }
}
