using SmartAdmin.Interfaces;
using SmartAdmin.Models.htpp;
using SmartAdmin.Models.Taller;

namespace SmartAdmin.Services
{
    public class RecepcionServices : IRecepcion
    {
        private readonly IApiClient apiClient;

        public RecepcionServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponse<RecepcionWizardViewModel>> GetDatosCitaAsync(int citaId)
        {
            return await apiClient.GetAsync<RecepcionWizardViewModel>($"api/Recepciones/DatosCita/{citaId}");
        }

        public async Task<ApiResponse<object>> IniciarDesdeCitaAsync(IniciarRecepcionRequest model)
        {
            return await apiClient.PostAsync<object>("api/Recepciones/IniciarDesdeCita", model);
        }

        public async Task<ApiResponse<object>> SubirEvidenciaBase64Async(SubirEvidenciaRequest model)
        {
            return await apiClient.PostAsync<object>("api/Evidencias/base64", model);
        }
    }
}
