using SmartAdmin.Models.htpp;
using static SmartAdmin.Models.UserRole.RoleViewModels;

namespace SmartAdmin.Interfaces
{
    public interface IAuth
    {
        Task<ApiResponse<List<RoleViewModel>>> GetAllAsync();
        Task<ApiResponse<List<string>>> GetByUserIdAsync(string UserId);
    }
}
