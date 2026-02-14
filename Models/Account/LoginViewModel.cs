using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SmartAdmin.Models.Account
{
    public class LoginViewModel
    {

        [Display(Name = "Ingresa tu correo")]
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Ingresa tu contraseña")]
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
    public class LoginRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public UserInfo User { get; set; } = new();
    }

    public class UserInfo
    {
        public string Id { get; set; } = null!;
        public string? UserName { get; set; }
        public string Email { get; set; } = null!;
    }
}
