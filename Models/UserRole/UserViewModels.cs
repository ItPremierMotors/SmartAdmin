using System.ComponentModel.DataAnnotations;

namespace SmartAdmin.Models.UserRole
{
    public class UserViewModels
    {
        public class UserViewModel
        {
            public string Id { get; set; } = null!;
            public string NombreCompleto { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string? Departamento { get; set; }
            public bool Activo { get; set; }
            public string Cargo { get; set; } = null!;
            public List<string> Roles { get; set; } = new();
        }

        public class CreateUserViewModel
        {
            [Display(Name = "Correo electrónico")]
            [Required(ErrorMessage = "El correo es obligatorio")]
            [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
            public string Email { get; set; } = string.Empty;

            [Display(Name = "Contraseña")]
            [Required(ErrorMessage = "La contraseña es obligatoria")]
            [DataType(DataType.Password)]
            [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
            public string Password { get; set; } = string.Empty;

            [Display(Name = "Confirmar contraseña")]
            [Required(ErrorMessage = "Confirme la contraseña")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
            public string ConfirmPassword { get; set; } = string.Empty;

            [Display(Name = "Nombre completo")]
            [Required(ErrorMessage = "El nombre es obligatorio")]
            public string NombreCompleto { get; set; } = string.Empty;

            [Display(Name = "Cargo")]
            public string? Cargo { get; set; }

            [Display(Name = "Departamento")]
            [Required(ErrorMessage = "El departamento es obligatorio")]
            public string Departamento { get; set; } = string.Empty;

            [Display(Name = "Activo")]
            public bool Activo { get; set; } = true;

            [Display(Name = "Roles")]
            public List<string> Roles { get; set; } = new();
        }
        public class EditUserViewModel
        {
            

            [Display(Name = "Nombre completo")]
            [Required(ErrorMessage = "El nombre es obligatorio")]
            public string NombreCompleto { get; set; } = string.Empty;

            [Display(Name = "Cargo")]
            public string? Cargo { get; set; }

            [Display(Name = "Activo")]
            public bool Activo { get; set; }

            [Display(Name = "Roles")]
            public List<string> Roles { get; set; } = new();
        }

        public class ResetPasswordViewModel
        {
            public string NewPassword { get; set; } = null!;
        }
    }
}
