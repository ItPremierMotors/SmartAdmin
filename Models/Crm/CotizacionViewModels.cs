using System.ComponentModel.DataAnnotations;

namespace SmartAdmin.Models.Crm
{
    public class CotizacionVehiculoViewModel
    {
        public int CotizacionVehiculoId { get; set; }
        public string CodigoCotizacion { get; set; } = null!;
        public decimal Descuento { get; set; }
        public decimal PrecioOfertado { get; set; }
        public string? CondicionesPago { get; set; }
        public string? Observaciones { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int Estado { get; set; }

        // Datos relacionados
        public int OportunidadId { get; set; }
        public int VehiculoId { get; set; }
        public string VehiculoDescripcion { get; set; } = null!;
        public decimal VehiculoPrecioLista { get; set; }

        // Extras para UI
        public string? EstadoNombre { get; set; }
        public bool EstaVencida { get; set; }
    }

    public class CreateCotizacionVehiculoViewModel
    {
        [Required]
        public int OportunidadId { get; set; }

        [Display(Name = "Vehículo")]
        [Required(ErrorMessage = "Debe seleccionar un vehículo")]
        public int VehiculoId { get; set; }

        [Display(Name = "Descuento")]
        [Range(0, double.MaxValue)]
        public decimal Descuento { get; set; }

        [Display(Name = "Precio Ofertado")]
        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal PrecioOfertado { get; set; }

        [Display(Name = "Condiciones de Pago")]
        [StringLength(500)]
        public string? CondicionesPago { get; set; }

        [Display(Name = "Observaciones")]
        [StringLength(1000)]
        public string? Observaciones { get; set; }

        [Display(Name = "Fecha de Vencimiento")]
        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
        public DateTime FechaVencimiento { get; set; }
    }
}