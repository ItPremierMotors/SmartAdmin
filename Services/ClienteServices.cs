using SmartAdmin.Interfaces;
using SmartAdmin.Models.Cliente;
using SmartAdmin.Models.htpp;

namespace SmartAdmin.Services
{
    public class ClienteServices : ICliente
    {
        private readonly IApiClient apiClient;

        public ClienteServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        public async Task<ApiResponse<List<ClienteViewModel>>> GetAllAsync()
        {
            return await apiClient.GetAsync<List<ClienteViewModel>>("api/Clientes/GetAll");
        }

        public async Task<ApiResponse<ClienteViewModel>> GetByIdAsync(int clienteId)
        {
            return await apiClient.GetAsync<ClienteViewModel>($"api/Clientes/GetById/{clienteId}");
        }

        public async Task<ApiResponse<List<ClienteViewModel>>> SearchAsync(string termino)
        {
            return await apiClient.GetAsync<List<ClienteViewModel>>($"api/Clientes/Search/{termino}");
        }

        public async Task<ApiResponse<ClienteViewModel>> CreateAsync(CreateClienteViewModel model)
        {
            return await apiClient.PostAsync<ClienteViewModel>("api/Clientes/Create", model);
        }

        public async Task<ApiResponse<ClienteViewModel>> EditAsync(EditClienteViewModel model)
        {
            return await apiClient.PutAsync<ClienteViewModel>("api/Clientes/Update", model);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int clienteId)
        {
            return await apiClient.DeleteAsync<bool>($"api/Clientes/Delete/{clienteId}");
        }
    }
}
