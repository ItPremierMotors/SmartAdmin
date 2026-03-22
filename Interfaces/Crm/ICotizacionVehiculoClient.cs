using SmartAdmin.Models.Crm;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Interfaces.Crm
{
    public interface ICotizacionVehiculoClient
    {
        Task<ApiResponse<List<CotizacionVehiculoViewModel>>> GetByOportunidadAsync(int oportunidadId);
        Task<ApiResponse<CotizacionVehiculoViewModel>> CreateAsync(CreateCotizacionVehiculoViewModel model);
        Task<ApiResponse<bool>> AceptarAsync(int cotizacionId);
        Task<ApiResponse<bool>> RechazarAsync(int cotizacionId);
    }
}