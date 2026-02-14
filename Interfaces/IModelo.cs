using SmartAdmin.Models.Catalogo.Modelo;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Interfaces
{
    public interface IModelo
    {
        public Task<ApiResponse<List<ModeloViewModel>>> GetAllAsync();
        public Task<ApiResponse<ModeloViewModel>> GetDetails(string modeloId);
        public Task<ApiResponse<ModeloViewModel>> CreateAsync(CreateModeloViewModel model);
        public Task<ApiResponse<ModeloViewModel>> EditAsync(EditModeloViewModel model);
        public Task<ApiResponse<bool>> DeleteAsync(int modeloId);
        public Task<ApiResponse<List<ModeloViewModel>>> GetByMarcaAsync(string marcaId);

    }

}
