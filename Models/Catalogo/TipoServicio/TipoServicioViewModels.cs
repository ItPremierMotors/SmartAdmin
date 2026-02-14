using System.ComponentModel.DataAnnotations;
using SmartAdmin.Models.Enums;

namespace SmartAdmin.Models.Catalogo.TipoServicio
{
    public class TipoServicioViewModel
    {
        public int TipoServicioId { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int DuracionEstimadaMin { get; set; }
        public ClasificacionServicio Clasificacion { get; set; }
        public bool PermiteWalkIn { get; set; }
        public bool RequiereCita { get; set; }
        public decimal PrecioBase { get; set; }
        public int StockRequerido { get; set; }
        public string DuracionFormateada { get; set; } = null!;

        public string ClasificacionNombre => Clasificacion.ToString();
    }

    public class CreateTipoServicioViewModel
    {
        [Display(Name = "Código")]
        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(20, ErrorMessage = "El código no puede exceder 20 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }

        [Display(Name = "Duración Estimada (min)")]
        [Required(ErrorMessage = "La duración es obligatoria")]
        [Range(1, 1440, ErrorMessage = "La duración debe estar entre 1 y 1440 minutos")]
        public int DuracionEstimadaMin { get; set; }

        [Display(Name = "Clasificación")]
        [Required(ErrorMessage = "La clasificación es obligatoria")]
        public ClasificacionServicio Clasificacion { get; set; } = ClasificacionServicio.Estandar;

        [Display(Name = "Permite Walk-In")]
        public bool PermiteWalkIn { get; set; } = false;

        [Display(Name = "Requiere Cita")]
        public bool RequiereCita { get; set; } = true;

        [Display(Name = "Precio Base")]
        [Range(0, 99999999, ErrorMessage = "El precio debe ser un valor positivo")]
        public decimal PrecioBase { get; set; } = 0;

        [Display(Name = "Stock Requerido")]
        [Range(0, 9999, ErrorMessage = "El stock debe ser un valor positivo")]
        public int StockRequerido { get; set; } = 0;
    }

    public class EditTipoServicioViewModel
    {
        public int TipoServicioId { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(20, ErrorMessage = "El código no puede exceder 20 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
        public string? Descripcion { get; set; }

        [Display(Name = "Duración Estimada (min)")]
        [Required(ErrorMessage = "La duración es obligatoria")]
        [Range(1, 1440, ErrorMessage = "La duración debe estar entre 1 y 1440 minutos")]
        public int DuracionEstimadaMin { get; set; }

        [Display(Name = "Clasificación")]
        [Required(ErrorMessage = "La clasificación es obligatoria")]
        public ClasificacionServicio Clasificacion { get; set; }

        [Display(Name = "Permite Walk-In")]
        public bool PermiteWalkIn { get; set; }

        [Display(Name = "Requiere Cita")]
        public bool RequiereCita { get; set; }

        [Display(Name = "Precio Base")]
        [Range(0, 99999999, ErrorMessage = "El precio debe ser un valor positivo")]
        public decimal PrecioBase { get; set; }

        [Display(Name = "Stock Requerido")]
        [Range(0, 9999, ErrorMessage = "El stock debe ser un valor positivo")]
        public int StockRequerido { get; set; }
    }
}
