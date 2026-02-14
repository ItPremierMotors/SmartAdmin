using System.ComponentModel.DataAnnotations;
using SmartAdmin.Models.Enums;

namespace SmartAdmin.Models.Catalogo.Modelo
{
    /// <summary>
    /// Para listar y mostrar detalles
    /// </summary>
    public class ModeloViewModel
    {
        public int ModeloId { get; set; }

        public int MarcaId { get; set; }

        public string Codigo { get; set; } = null!;

        public string Nombre { get; set; } = null!;

        public SegmentoVehiculo Segmento { get; set; } 

        public int? AnioInicio { get; set; }

        public int? AnioFin { get; set; }

        public string? Descripcion { get; set; }

        public string? ImagenUrl { get; set; }

        public string? MarcaNombre { get; set; }

        public bool EstaEnProduccion { get; set; }

        // Propiedad calculada para mostrar el segmento como string
        public string SegmentoNombre => Segmento.ToString();
    }

    /// <summary>
    /// Para crear nuevo modelo
    /// </summary>
    public class CreateModeloViewModel
    {
        [Display(Name = "Marca")]
        [Required(ErrorMessage = "La marca es obligatoria")]
        public int MarcaId { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(20, ErrorMessage = "El código no puede exceder 20 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Segmento")]
        [Required(ErrorMessage = "El segmento es obligatorio")]
        public SegmentoVehiculo Segmento { get; set; } = SegmentoVehiculo.Sedan;

        [Display(Name = "Año Inicio")]
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
        public int? AnioInicio { get; set; }

        [Display(Name = "Año Fin")]
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
        public int? AnioFin { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }

        [Display(Name = "URL de Imagen")]
        [Url(ErrorMessage = "La URL no es válida")]
        public string? ImagenUrl { get; set; }
    }

    /// <summary>
    /// Para editar modelo existente
    /// </summary>
    public class EditModeloViewModel
    {
        private string? _imagenUrl;

        public int ModeloId { get; set; }

        [Display(Name = "Marca")]
        [Required(ErrorMessage = "La marca es obligatoria")]
        public int MarcaId { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(20, ErrorMessage = "El código no puede exceder 20 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Segmento")]
        [Required(ErrorMessage = "El segmento es obligatorio")]
        public SegmentoVehiculo Segmento { get; set; } 

        [Display(Name = "Año Inicio")]
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
        public int? AnioInicio { get; set; }

        [Display(Name = "Año Fin")]
        [Range(1900, 2100, ErrorMessage = "El año debe estar entre 1900 y 2100")]
        public int? AnioFin { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }

        [Display(Name = "URL de Imagen")]
        [Url(ErrorMessage = "La URL no es válida")]
        public string? ImagenUrl
        {
            get => string.IsNullOrWhiteSpace(_imagenUrl) ? null : _imagenUrl;
            set => _imagenUrl = value;
        }
    }

}
