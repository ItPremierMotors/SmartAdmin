using System.ComponentModel.DataAnnotations;

namespace SmartAdmin.Models.Crm
{
    public class LeadViewModel
    {
        public int LeadId { get; set; }
        public string CodigoLead { get; set; } = null!;
        public string NombreCompleto { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Empresa { get; set; }
        public string? Ciudad { get; set; }
        public int Origen { get; set; }
        public string? DetalleOrigen { get; set; }
        public int Estado { get; set; }
        public DateTime FechaIngreso { get; set; }
        public DateTime? FechaPrimeraRespuesta { get; set; }
        public DateTime? FechaUltimaActividad { get; set; }
        public string? VehiculoInteres { get; set; }
        public int? TipoVehiculoInteres { get; set; }
        public decimal? PresupuestoEstimado { get; set; }
        public int? SucursalId { get; set; }
        public string? SucursalNombre { get; set; }
        public string? VendedorAsignadoId { get; set; }
        public string? VendedorNombre { get; set; }
        public string? EstadoNombre { get; set; }
        public string? OrigenNombre { get; set; }
        public bool SinContactar { get; set; }
        public int? DiasInactivo { get; set; }
        public int CantidadOportunidades { get; set; }
        public int CantidadActividades { get; set; }
    }

    public class CreateLeadViewModel
    {
        [Display(Name = "Nombre Completo")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        [StringLength(20)]
        public string? Telefono { get; set; }

        [Display(Name = "Correo Electrónico")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Correo no válido")]
        public string? Email { get; set; }

        [Display(Name = "Empresa")]
        [StringLength(150)]
        public string? Empresa { get; set; }

        [Display(Name = "Ciudad")]
        [StringLength(100)]
        public string? Ciudad { get; set; }

        [Display(Name = "Origen")]
        public int Origen { get; set; }

        [Display(Name = "Detalle Origen")]
        [StringLength(250)]
        public string? DetalleOrigen { get; set; }

        [Display(Name = "Sucursal")]
        public int? SucursalId { get; set; }

        [Display(Name = "Vendedor")]
        public string? VendedorAsignadoId { get; set; }

        [Display(Name = "Vehículo de Interés")]
        [StringLength(200)]
        public string? VehiculoInteres { get; set; }

        [Display(Name = "Tipo Vehículo")]
        public int? TipoVehiculoInteres { get; set; }

        [Display(Name = "Presupuesto Estimado")]
        public decimal? PresupuestoEstimado { get; set; }
    }

    public class EditLeadViewModel
    {
        public int LeadId { get; set; }

        [Display(Name = "Nombre Completo")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        [StringLength(20)]
        public string? Telefono { get; set; }

        [Display(Name = "Correo Electrónico")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Correo no válido")]
        public string? Email { get; set; }

        [Display(Name = "Empresa")]
        [StringLength(150)]
        public string? Empresa { get; set; }

        [Display(Name = "Ciudad")]
        [StringLength(100)]
        public string? Ciudad { get; set; }

        [Display(Name = "Detalle Origen")]
        [StringLength(250)]
        public string? DetalleOrigen { get; set; }

        [Display(Name = "Vehículo de Interés")]
        [StringLength(200)]
        public string? VehiculoInteres { get; set; }

        [Display(Name = "Tipo Vehículo")]
        public int? TipoVehiculoInteres { get; set; }

        [Display(Name = "Presupuesto Estimado")]
        public decimal? PresupuestoEstimado { get; set; }
    }

    public class CalificarLeadViewModel
    {
        public int LeadId { get; set; }
    }

    public class DescartarLeadViewModel
    {
        public int LeadId { get; set; }

        [Required(ErrorMessage = "El motivo es obligatorio")]
        [StringLength(500)]
        public string Motivo { get; set; } = string.Empty;
    }

    public class AsignarVendedorLeadViewModel
    {
        public int LeadId { get; set; }
        public int? SucursalId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un vendedor")]
        public string VendedorId { get; set; } = string.Empty;
    }
}
