using SmartAdmin.Models.htpp;
using SmartAdmin.Models.Taller;

namespace SmartAdmin.Interfaces
{
    public interface IRecepcion
    {
        Task<ApiResponse<RecepcionWizardViewModel>> GetDatosCitaAsync(int citaId);
        Task<ApiResponse<object>> IniciarDesdeCitaAsync(IniciarRecepcionRequest model);
        Task<ApiResponse<object>> SubirEvidenciaBase64Async(SubirEvidenciaRequest model);
        Task<ApiResponse<List<EvidenciaViewModel>>> GetEvidenciasByOsIdAsync(int osId);
        Task<ApiResponse<object>> IniciarWalkInAsync(IniciarRecepcionWalkInRequest model);
    }
}
