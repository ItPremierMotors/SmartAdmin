using SmartAdmin.Models.Catalogo.Marca;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Interfaces
{
    public interface IMarca
    {
        public Task<ApiResponse<List<MarcaViewModel>>> GetAllAsync();
        public Task<ApiResponse<MarcaViewModel>> GetDetails(string marcaId);
        public Task<ApiResponse<MarcaViewModel>> CreateAsync(CreateMarcaViewModel model);
        public Task<ApiResponse<MarcaViewModel>> EditAsync(EditMarcaViewModel model);
        public Task<ApiResponse<bool>> DeleteAsync(int marcaId);

    }

}
