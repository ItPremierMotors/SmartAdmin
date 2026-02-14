using SmartAdmin.Models.Catalogo.VersionVehiculo;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Interfaces
{
    public interface IVersionVehiculo
    {
        Task<ApiResponse<List<VersionVehiculoViewModel>>> GetAllAsync();
        Task<ApiResponse<VersionVehiculoViewModel>> GetDetails(int versionId);
        Task<ApiResponse<List<VersionVehiculoViewModel>>> GetByModeloAsync(int modeloId);
        Task<ApiResponse<VersionVehiculoViewModel>> CreateAsync(CreateVersionVehiculoViewModel model);
        Task<ApiResponse<VersionVehiculoViewModel>> EditAsync(EditVersionVehiculoViewModel model);
        Task<ApiResponse<bool>> DeleteAsync(int versionId);
    }
}
