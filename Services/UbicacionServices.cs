using SmartAdmin.Interfaces;
using SmartAdmin.Models.Catalogo.Ubicacion;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Services
{
    public class UbicacionServices : IUbicacion
    {
        private readonly IApiClient apiClient;

        public UbicacionServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponse<List<UbicacionViewModel>>> GetActivasAsync()
        {
            return await apiClient.GetAsync<List<UbicacionViewModel>>("api/Ubicaciones/GetActivas");
        }
    }
}
