using SmartAdmin.Models.Cliente;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Interfaces
{
    public interface ICliente
    {
        Task<ApiResponse<List<ClienteViewModel>>> GetAllAsync();
        Task<ApiResponse<ClienteViewModel>> GetByIdAsync(int clienteId);
        Task<ApiResponse<List<ClienteViewModel>>> SearchAsync(string termino);
        Task<ApiResponse<ClienteViewModel>> CreateAsync(CreateClienteViewModel model);
        Task<ApiResponse<ClienteViewModel>> EditAsync(EditClienteViewModel model);
        Task<ApiResponse<bool>> DeleteAsync(int clienteId);
    }
}
