using SmartAdmin.Interfaces;
using SmartAdmin.Interfaces.Crm;
using SmartAdmin.Models.Crm;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Services.Crm
{
    public class LeadClientServices : ILeadClient
    {
        private readonly IApiClient apiClient;
        

        public LeadClientServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponse<List<LeadViewModel>>> GetAllAsync()
        {
            return await apiClient.GetAsync<List<LeadViewModel>>("api/leads/GetAll");
        }

        public async Task<ApiResponse<LeadViewModel>> GetByIdAsync(int leadId)
        {
            return await apiClient.GetAsync<LeadViewModel>($"api/leads/GetById/{leadId}");
        }

        public async Task<ApiResponse<List<LeadViewModel>>> GetBySucursalAsync(int sucursalId)
        {
            return await apiClient.GetAsync<List<LeadViewModel>>($"api/leads/GetBySucursal/{sucursalId}");
        }

        public async Task<ApiResponse<List<LeadViewModel>>> GetByVendedorAsync(string vendedorId)
        {
            return await apiClient.GetAsync<List<LeadViewModel>>($"api/leads/GetByVendedor/{vendedorId}");
        }

        public async Task<ApiResponse<List<LeadViewModel>>> GetByEstadoAsync(int estado)
        {
            return await apiClient.GetAsync<List<LeadViewModel>>($"api/leads/GetByEstado/{estado}");
        }

        public async Task<ApiResponse<LeadViewModel>> CreateAsync(CreateLeadViewModel model)
        {
            return await apiClient.PostAsync<LeadViewModel>("api/leads/Create", model);
        }

        public async Task<ApiResponse<LeadViewModel>> EditAsync(EditLeadViewModel model)
        {
            return await apiClient.PutAsync<LeadViewModel>("api/leads/Update", model);
        }

        public async Task<ApiResponse<bool>> CalificarAsync(CalificarLeadViewModel model)
        {
            return await apiClient.PutAsync<bool>("api/leads/Calificar", model);
        }

        public async Task<ApiResponse<bool>> DescartarAsync(DescartarLeadViewModel model)
        {
            return await apiClient.PutAsync<bool>("api/leads/Descartar", model);
        }

        public async Task<ApiResponse<bool>> AsignarVendedorAsync(AsignarVendedorLeadViewModel model)
        {
            return await apiClient.PutAsync<bool>("api/leads/AsignarVendedor", model);
        }

        public async Task<ApiResponse<bool>> MarcarContactadoAsync(int leadId)
        {
            return await apiClient.PutAsync<bool>($"api/leads/MarcarContactado/{leadId}", new { });
        }

        public async Task<ApiResponse<AlertasLeadsFriosVM>> GetAlertasFriosAsync(int sucursalId)
        {
            return await apiClient.GetAsync<AlertasLeadsFriosVM>($"api/leads/alertas-frios?sucursalId={sucursalId}");
        }
    }
}