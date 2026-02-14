using SmartAdmin.Models.htpp;
using SmartAdmin.Interfaces;
using System.Text.Json;

namespace SmartAdmin.Services
{
    public class ApiClientServices : IApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _jsonOptions;
        private const string ClientName = "PremierFlowApi";

        public ApiClientServices(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase // <-- Esto convierte Email -> email

            };
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient(ClientName);

            // Agregar JWT desde la cookie
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        #region ENDPOINTS
        public async Task<ApiResponse<T>> GetAsync<T>(string endpoint)
        {
            try
            {
                using var client = CreateClient();
                var response = await client.GetAsync(endpoint);
                return await HandleResponse<T>(response);
            }
            catch (Exception ex)
            {
                return CreateErrorResponse<T>(ex);
            }
        }
        public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object? data = null)
        {
            try
            {
                using var client = CreateClient();
                var response = await client.PostAsJsonAsync(endpoint, data);
                return await HandleResponse<T>(response);

            }
            catch (Exception ex)
            {
                return CreateErrorResponse<T>(ex);
            }
        }

        public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object data)
        {
            try
            {
                using var client = CreateClient();
                var response = await client.PutAsJsonAsync(endpoint, data);
                return await HandleResponse<T>(response);

            }
            catch (Exception ex)
            {
                return CreateErrorResponse<T>(ex);
            }
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(string endpoint)
        {
            try
            {
                using var client = CreateClient();
                var response = await client.DeleteAsync(endpoint);
                return await HandleResponse<T>(response);

            }
            catch (Exception ex)
            {
                return CreateErrorResponse<T>(ex);
            }
        }

        public async Task<ApiResponse<T>> PatchAsync<T>(string endpoint, object? data = null)
        {
            try
            {
                using var client = CreateClient();
                var json = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PatchAsync(endpoint, content);
                return await HandleResponse<T>(response);
            }
            catch (Exception ex)
            {
                return CreateErrorResponse<T>(ex);
            }
        }

        #endregion


        private async Task<ApiResponse<T>> HandleResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response: " + content); // Debug
            if (string.IsNullOrEmpty(content))
            {
                return new ApiResponse<T>
                {
                    Success = response.IsSuccessStatusCode,
                    StatusCode = (int)response.StatusCode,
                    Message = response.IsSuccessStatusCode ? "OK" : "Error"
                };
            }

            try
            {
                var result = JsonSerializer.Deserialize<ApiResponse<T>>(content, _jsonOptions);
                return result ?? new ApiResponse<T>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Error deserializando respuesta"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deserializando: " + ex.Message); // Debug
                return new ApiResponse<T>
                {
                    Success = false,
                    StatusCode = (int)response.StatusCode,
                    Message = content
                };
            }
        }

        private static ApiResponse<T> CreateErrorResponse<T>(Exception ex)
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = 500,
                Message = "Error de conexión con el servidor",
                Errors = new[] { ex.Message }
            };
        }


    }
}
