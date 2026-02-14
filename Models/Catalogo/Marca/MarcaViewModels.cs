using System.ComponentModel.DataAnnotations;

namespace SmartAdmin.Models.Catalogo.Marca
{
    /// <summary>
    /// Para listar y mostrar detalles
    /// </summary>
    public class MarcaViewModel
    {
        public int MarcaId { get; set; }

        public string Codigo { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public string? PaisOrigen { get; set; }

        public bool EsMarcaPropia { get; set; }

        public string? LogoUrl { get; set; }

        public string? Observaciones { get; set; }
    }

    /// <summary>
    /// Para crear nueva marca
    /// </summary>
    public class CreateMarcaViewModel
    {
        [Display(Name = "Código")]
        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(20, ErrorMessage = "El código no puede exceder 20 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "País de Origen")]
        [StringLength(50, ErrorMessage = "El país no puede exceder 50 caracteres")]
        public string? PaisOrigen { get; set; }

        [Display(Name = "¿Es marca propia?")]
        public bool EsMarcaPropia { get; set; } = true;

        [Display(Name = "URL del Logo")]
        [Url(ErrorMessage = "La URL no es válida")]
        public string? LogoUrl { get; set; }

        [Display(Name = "Observaciones")]
        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        public string? Observaciones { get; set; }
    }

    /// <summary>
    /// Para editar marca existente
    /// </summary>
    public class EditMarcaViewModel
    {
        private string? _logoUrl;
        public int MarcaId { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(20, ErrorMessage = "El código no puede exceder 20 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "País de Origen")]
        [StringLength(50, ErrorMessage = "El país no puede exceder 50 caracteres")]
        public string? PaisOrigen { get; set; }

        [Display(Name = "¿Es marca propia?")]
        public bool EsMarcaPropia { get; set; }

        [Display(Name = "URL del Logo")]
        [Url(ErrorMessage = "La URL no es válida")]
        public string? LogoUrl
        {
            get => string.IsNullOrWhiteSpace(_logoUrl) ? null : _logoUrl;
            set => _logoUrl = value;
        }

        [Display(Name = "Observaciones")]
        [StringLength(800, ErrorMessage = "Las observaciones no pueden exceder 500 caracteres")]
        public string? Observaciones { get; set; }
    }

}
