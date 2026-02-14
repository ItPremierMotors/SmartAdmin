using SmartAdmin.Models.htpp;

namespace SmartAdmin.Interfaces
{

        public interface IApiClient
        {
            Task<ApiResponse<T>> GetAsync<T>(string endpoint);
            Task<ApiResponse<T>> PostAsync<T>(string endpoint, object? data = null);
            Task<ApiResponse<T>> PutAsync<T>(string endpoint, object data);
            Task<ApiResponse<T>> DeleteAsync<T>(string endpoint);
            Task<ApiResponse<T>> PatchAsync<T>(string endpoint, object? data = null);
        }
   
}
