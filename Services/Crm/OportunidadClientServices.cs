using SmartAdmin.Interfaces;
using SmartAdmin.Interfaces.Crm;
using SmartAdmin.Models.Crm;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Services.Crm
{
    public class OportunidadClientServices : IOportunidadClient
    {
        private readonly IApiClient apiClient;

        public OportunidadClientServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponse<List<OportunidadViewModel>>> GetAllAsync()
        {
            return await apiClient.GetAsync<List<OportunidadViewModel>>("api/oportunidades/GetAll");
        }

        public async Task<ApiResponse<OportunidadDetalleViewModel>> GetDetalleAsync(int oportunidadId)
        {
            return await apiClient.GetAsync<OportunidadDetalleViewModel>($"api/oportunidades/GetDetalle/{oportunidadId}");
        }

        public async Task<ApiResponse<List<OportunidadViewModel>>> GetByLeadAsync(int leadId)
        {
            return await apiClient.GetAsync<List<OportunidadViewModel>>($"api/oportunidades/GetByLead/{leadId}");
        }

        public async Task<ApiResponse<List<OportunidadViewModel>>> GetByVendedorAsync(string vendedorId)
        {
            return await apiClient.GetAsync<List<OportunidadViewModel>>($"api/oportunidades/GetByVendedor/{vendedorId}");
        }

        public async Task<ApiResponse<List<OportunidadViewModel>>> GetByEtapaAsync(int etapa)
        {
            return await apiClient.GetAsync<List<OportunidadViewModel>>($"api/oportunidades/GetByEtapa/{etapa}");
        }

        public async Task<ApiResponse<List<PipelineResumenViewModel>>> GetPipelineAsync(int sucursalId)
        {
            return await apiClient.GetAsync<List<PipelineResumenViewModel>>($"api/oportunidades/GetPipeline?sucursalId={sucursalId}");
        }

        public async Task<ApiResponse<OportunidadViewModel>> CreateAsync(CreateOportunidadViewModel model)
        {
            return await apiClient.PostAsync<OportunidadViewModel>("api/oportunidades/Create", model);
        }

        public async Task<ApiResponse<OportunidadViewModel>> UpdateAsync(UpdateOportunidadViewModel model)
        {
            return await apiClient.PutAsync<OportunidadViewModel>("api/oportunidades/Update", model);
        }

        public async Task<ApiResponse<bool>> CambiarEtapaAsync(CambiarEtapaViewModel model)
        {
            return await apiClient.PutAsync<bool>("api/oportunidades/CambiarEtapa", model);
        }

        public async Task<ApiResponse<bool>> AvanzarEtapaAsync(int oportunidadId)
        {
            return await apiClient.PutAsync<bool>($"api/oportunidades/{oportunidadId}/AvanzarEtapa", new { });
        }

        public async Task<ApiResponse<bool>> RetrocederEtapaAsync(int oportunidadId)
        {
            return await apiClient.PutAsync<bool>($"api/oportunidades/{oportunidadId}/RetrocederEtapa", new { });
        }

        public async Task<ApiResponse<bool>> VincularVehiculoAsync(VincularVehiculoViewModel model)
        {
            return await apiClient.PutAsync<bool>($"api/oportunidades/{model.OportunidadId}/VincularVehiculo/{model.VehiculoId}", model);
        }

        public async Task<ApiResponse<bool>> CerrarGanadaAsync(CerrarOportunidadViewModel model)
        {
            return await apiClient.PutAsync<bool>("api/oportunidades/CerrarGanada", model);
        }

        public async Task<ApiResponse<bool>> CerrarPerdidaAsync(CerrarOportunidadViewModel model)
        {
            return await apiClient.PutAsync<bool>("api/oportunidades/CerrarPerdida", model);
        }

        public async Task<ApiResponse<bool>> CancelarAsync(CerrarOportunidadViewModel model)
        {
            return await apiClient.PutAsync<bool>("api/oportunidades/Cancelar", model);
        }
    }
}