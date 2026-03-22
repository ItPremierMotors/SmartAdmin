using System.ComponentModel.DataAnnotations;

namespace SmartAdmin.Models.Crm
{
    public class OportunidadViewModel
    {
        public int OportunidadId { get; set; }
        public string CodigoOportunidad { get; set; } = null!;
        public int Etapa { get; set; }
        public int ProbabilidadCierre { get; set; }
        public DateTime? FechaCierreEstimada { get; set; }
        public int? Resultado { get; set; }
        public string? MotivoResultado { get; set; }
        public DateTime? FechaCierre { get; set; }
        public DateTime? FechaUltimaActividad { get; set; }

        // Datos relacionados
        public int LeadId { get; set; }
        public string LeadNombre { get; set; } = null!;
        // public int? ClienteId { get; set; }
        // public string? ClienteNombre { get; set; }
        // public int? VehiculoId { get; set; }
        // public string? VehiculoDescripcion { get; set; }
        public int? ModeloId { get; set; }

        public string VendedorId { get; set; } = null!;
        public string VendedorNombre { get; set; } = null!;
        public int SucursalId { get; set; }
        public string SucursalNombre { get; set; } = null!;

        // Extras para UI
        public string ModeloNombre { get; set; } = null!;
        public string? EtapaNombre { get; set; }
        public string? ResultadoNombre { get; set; }
        public bool EstaAbierta { get; set; }
        public int CantidadActividades { get; set; }
        public int CantidadCotizaciones { get; set; }
    }

    public class OportunidadDetalleViewModel : OportunidadViewModel
    {
        public List<ActividadCrmViewModel> Actividades { get; set; } = [];
        public List<NotaCrmViewModel> Notas { get; set; } = [];
        public List<CotizacionVehiculoViewModel> Cotizaciones { get; set; } = [];
    }

    public class CreateOportunidadViewModel
    {
        [Display(Name = "Lead")]
        [Required(ErrorMessage = "Debe seleccionar un lead")]
        public int LeadId { get; set; }

        public int? ClienteId { get; set; }

        [Display(Name = "Modelo de Vehículo")]
        public int? ModeloId { get; set; }

        [Display(Name = "Vendedor")]
        [Required(ErrorMessage = "Debe seleccionar un vendedor")]
        public string VendedorId { get; set; } = string.Empty;

        [Display(Name = "Sucursal")]
        [Required(ErrorMessage = "Debe seleccionar una sucursal")]
        public int SucursalId { get; set; }

        [Display(Name = "Probabilidad de Cierre (%)")]
        [Range(0, 100)]
        public int ProbabilidadCierre { get; set; }

        [Display(Name = "Fecha Cierre Estimada")]
        public DateTime? FechaCierreEstimada { get; set; }
    }

    public class UpdateOportunidadViewModel
    {
        public int OportunidadId { get; set; }

        public int? ClienteId { get; set; }

        [Display(Name = "Vehículo")]
        public int? VehiculoId { get; set; }

        [Display(Name = "Probabilidad de Cierre (%)")]
        [Range(0, 100)]
        public int ProbabilidadCierre { get; set; }

        [Display(Name = "Fecha Cierre Estimada")]
        public DateTime? FechaCierreEstimada { get; set; }
    }

    public class CerrarOportunidadViewModel
    {
        public int OportunidadId { get; set; }

        [Display(Name = "Motivo")]
        [StringLength(500)]
        public string? MotivoResultado { get; set; }
    }

    public class CambiarEtapaViewModel
    {
        public int OportunidadId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una etapa")]
        public int NuevaEtapa { get; set; }
    }

    public class VincularVehiculoViewModel
    {
        public int OportunidadId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un vehículo")]
        public int VehiculoId { get; set; }
    }

    public class PipelineResumenViewModel
    {
        public int Etapa { get; set; }
        public string EtapaNombre { get; set; } = null!;
        public int Cantidad { get; set; }
    }
}