using SmartAdmin.Interfaces;
using SmartAdmin.Interfaces.Crm;
using SmartAdmin.Models.Crm;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Services.Crm
{
    public class NotaCrmClientServices : INotaCrmClient
    {
        private readonly IApiClient apiClient;

        public NotaCrmClientServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponse<List<NotaCrmViewModel>>> GetByLeadAsync(int leadId)
        {
            return await apiClient.GetAsync<List<NotaCrmViewModel>>($"api/notascrm/GetByLead/{leadId}");
        }

        public async Task<ApiResponse<List<NotaCrmViewModel>>> GetByOportunidadAsync(int oportunidadId)
        {
            return await apiClient.GetAsync<List<NotaCrmViewModel>>($"api/notascrm/GetByOportunidad/{oportunidadId}");
        }

        public async Task<ApiResponse<NotaCrmViewModel>> CreateAsync(CreateNotaCrmViewModel model)
        {
            return await apiClient.PostAsync<NotaCrmViewModel>("api/notascrm/Create", model);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int notaId)
        {
            return await apiClient.DeleteAsync<bool>($"api/notascrm/Delete/{notaId}");
        }
    }
}