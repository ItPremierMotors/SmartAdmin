using SmartAdmin.Interfaces;
using SmartAdmin.Interfaces.Crm;
using SmartAdmin.Models.Crm;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Services.Crm
{
    public class ActividadCrmClientServices : IActividadCrmClient
    {
        private readonly IApiClient apiClient;

        public ActividadCrmClientServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponse<List<ActividadCrmViewModel>>> GetByLeadAsync(int leadId)
        {
            return await apiClient.GetAsync<List<ActividadCrmViewModel>>($"api/actividadescrm/GetByLead/{leadId}");
        }

        public async Task<ApiResponse<List<ActividadCrmViewModel>>> GetByOportunidadAsync(int oportunidadId)
        {
            return await apiClient.GetAsync<List<ActividadCrmViewModel>>($"api/actividadescrm/GetByOportunidad/{oportunidadId}");
        }

        public async Task<ApiResponse<List<ActividadCrmViewModel>>> GetPendientesAsync(string vendedorId)
        {
            return await apiClient.GetAsync<List<ActividadCrmViewModel>>($"api/actividadescrm/GetPendientesVendedor/{vendedorId}");
        }

        public async Task<ApiResponse<List<ActividadCrmViewModel>>> GetProximosContactosAsync(string fecha)
        {
            return await apiClient.GetAsync<List<ActividadCrmViewModel>>($"api/actividadescrm/GetProximosContactos/{fecha}");
        }

        public async Task<ApiResponse<ActividadCrmViewModel>> CreateAsync(CreateActividadCrmViewModel model)
        {
            return await apiClient.PostAsync<ActividadCrmViewModel>("api/actividadescrm/Create", model);
        }

        public async Task<ApiResponse<bool>> CompletarAsync(CompletarActividadViewModel model)
        {
            return await apiClient.PutAsync<bool>("api/actividadescrm/Completar", model);
        }

        public async Task<ApiResponse<bool>> CancelarAsync(int actividadId)
        {
            return await apiClient.PutAsync<bool>($"api/actividadescrm/Cancelar/{actividadId}", new { });
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int actividadId)
        {
            return await apiClient.DeleteAsync<bool>($"api/actividadescrm/Delete/{actividadId}");
        }
    }
}