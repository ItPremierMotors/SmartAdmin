using System.ComponentModel.DataAnnotations;

namespace SmartAdmin.Models.Catalogo.Tecnico
{
    public class TecnicoViewModel
    {
        public int TecnicoId { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string? Especialidad { get; set; }
        public int? BahiaAsignada { get; set; }
        public string? UsuarioId { get; set; }
        public int? SucursalId { get; set; }

        // Extras para UI
        public string NombreCompleto { get; set; } = null!;
        public string? SucursalNombre { get; set; }
    }

    public class CreateTecnicoViewModel
    {
        [Display(Name = "Código")]
        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(20, ErrorMessage = "El código no puede exceder 20 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100, ErrorMessage = "Los apellidos no pueden exceder 100 caracteres")]
        public string Apellidos { get; set; } = string.Empty;

        [Display(Name = "Especialidad")]
        [StringLength(100, ErrorMessage = "La especialidad no puede exceder 100 caracteres")]
        public string? Especialidad { get; set; }

        [Display(Name = "Bahía Asignada")]
        [Range(1, 100, ErrorMessage = "La bahía debe estar entre 1 y 100")]
        public int? BahiaAsignada { get; set; }

        [Display(Name = "Usuario del Sistema")]
        public string? UsuarioId { get; set; }

        [Display(Name = "Sucursal")]
        public int? SucursalId { get; set; }
    }

    public class EditTecnicoViewModel
    {
        public int TecnicoId { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(20, ErrorMessage = "El código no puede exceder 20 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100, ErrorMessage = "Los apellidos no pueden exceder 100 caracteres")]
        public string Apellidos { get; set; } = string.Empty;

        [Display(Name = "Especialidad")]
        [StringLength(100, ErrorMessage = "La especialidad no puede exceder 100 caracteres")]
        public string? Especialidad { get; set; }

        [Display(Name = "Bahía Asignada")]
        [Range(1, 100, ErrorMessage = "La bahía debe estar entre 1 y 100")]
        public int? BahiaAsignada { get; set; }

        [Display(Name = "Usuario del Sistema")]
        public string? UsuarioId { get; set; }

        [Display(Name = "Sucursal")]
        public int? SucursalId { get; set; }
    }
}
