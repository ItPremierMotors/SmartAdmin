using SmartAdmin.Models.Crm;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Interfaces.Crm
{
    public interface IOportunidadClient
    {
        Task<ApiResponse<List<OportunidadViewModel>>> GetAllAsync();
        Task<ApiResponse<OportunidadDetalleViewModel>> GetDetalleAsync(int oportunidadId);
        Task<ApiResponse<List<OportunidadViewModel>>> GetByLeadAsync(int leadId);
        Task<ApiResponse<List<OportunidadViewModel>>> GetByVendedorAsync(string vendedorId);
        Task<ApiResponse<List<OportunidadViewModel>>> GetByEtapaAsync(int etapa);
        Task<ApiResponse<List<PipelineResumenViewModel>>> GetPipelineAsync(int sucursalId);
        Task<ApiResponse<OportunidadViewModel>> CreateAsync(CreateOportunidadViewModel model);
        Task<ApiResponse<OportunidadViewModel>> UpdateAsync(UpdateOportunidadViewModel model);
        Task<ApiResponse<bool>> CambiarEtapaAsync(CambiarEtapaViewModel model);
        Task<ApiResponse<bool>> AvanzarEtapaAsync(int oportunidadId);
        Task<ApiResponse<bool>> RetrocederEtapaAsync(int oportunidadId);
        Task<ApiResponse<bool>> VincularVehiculoAsync(VincularVehiculoViewModel model);
        Task<ApiResponse<bool>> CerrarGanadaAsync(CerrarOportunidadViewModel model);
        Task<ApiResponse<bool>> CerrarPerdidaAsync(CerrarOportunidadViewModel model);
        Task<ApiResponse<bool>> CancelarAsync(CerrarOportunidadViewModel model);
    }
}