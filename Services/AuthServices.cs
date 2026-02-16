using SmartAdmin.Interfaces;
using SmartAdmin.Models.htpp;
using static SmartAdmin.Models.UserRole.RoleViewModels;


namespace SmartAdmin.Services
{
    public class AuthServices: IAuth
    {
   
        private readonly IApiClient apiClient;
        public AuthServices(IApiClient apiClient )
        {
           this.apiClient = apiClient;
        }

        public async Task<ApiResponse<List<RoleViewModel>>> GetAllAsync()
        {
            return await apiClient.GetAsync<List<RoleViewModel>>("api/Roles");
        }

        public async Task<ApiResponse<List<string>>> GetByUserIdAsync(string UserId)
        {
            return await apiClient.GetAsync<List<string>>($"/api/Roles/user/{UserId}");
        }
    }
}