using System.ComponentModel.DataAnnotations;

namespace SmartAdmin.Models.Cliente
{
    public class ClienteViewModel
    {
        public int ClienteId { get; set; }
        public int TipoCliente { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Apellidos { get; set; }
        public string? DNI { get; set; }
        public string? RTN { get; set; }
        public string Telefono { get; set; } = null!;
        public string? TelefonoSecundario { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int NoShowCount { get; set; }

        // UI extras
        public string NombreCompleto { get; set; } = null!;
        public bool TieneAlertaNoShow { get; set; }
        public int CantidadVehiculos { get; set; }
    }

    public class CreateClienteViewModel
    {
        [Display(Name = "Tipo de Cliente")]
        public int TipoCliente { get; set; } = 1;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Apellidos")]
        [StringLength(100)]
        public string? Apellidos { get; set; }

        [Display(Name = "DNI")]
        [StringLength(15)]
        public string? DNI { get; set; }

        [Display(Name = "RTN")]
        [StringLength(16)]
        public string? RTN { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Display(Name = "Teléfono Secundario")]
        [StringLength(20)]
        public string? TelefonoSecundario { get; set; }

        [Display(Name = "Correo Electrónico")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Correo no válido")]
        public string? Email { get; set; }

        [Display(Name = "Dirección")]
        [StringLength(250)]
        public string? Direccion { get; set; }

        [Display(Name = "Ciudad")]
        [StringLength(100)]
        public string? Ciudad { get; set; }
    }

    public class EditClienteViewModel
    {
        public int ClienteId { get; set; }

        [Display(Name = "Tipo de Cliente")]
        public int TipoCliente { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Apellidos")]
        [StringLength(100)]
        public string? Apellidos { get; set; }

        [Display(Name = "DNI")]
        [StringLength(15)]
        public string? DNI { get; set; }

        [Display(Name = "RTN")]
        [StringLength(16)]
        public string? RTN { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Display(Name = "Teléfono Secundario")]
        [StringLength(20)]
        public string? TelefonoSecundario { get; set; }

        [Display(Name = "Correo Electrónico")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Correo no válido")]
        public string? Email { get; set; }

        [Display(Name = "Dirección")]
        [StringLength(250)]
        public string? Direccion { get; set; }

        [Display(Name = "Ciudad")]
        [StringLength(100)]
        public string? Ciudad { get; set; }
    }
}
