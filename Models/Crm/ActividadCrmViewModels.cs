using System.ComponentModel.DataAnnotations;

namespace SmartAdmin.Models.Crm
{
    public class ActividadCrmViewModel
    {
        public int ActividadCrmId { get; set; }
        public int Tipo { get; set; }
        public int Direccion { get; set; }
        public string Asunto { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int Estado { get; set; }
        public DateTime? FechaProgramada { get; set; }
        public DateTime? FechaRealizacion { get; set; }
        public int? DuracionMinutos { get; set; }
        public string? Resultado { get; set; }
        public DateTime? ProximoContacto { get; set; }

        // Datos relacionados
        public int? LeadId { get; set; }
        public string? LeadNombre { get; set; }
        public int? OportunidadId { get; set; }
        public string? OportunidadCodigo { get; set; }
        public string RealizadaPorId { get; set; } = null!;
        public string RealizadaPorNombre { get; set; } = null!;

        // Extras para UI
        public string? TipoNombre { get; set; }
        public string? DireccionNombre { get; set; }
        public string? EstadoNombre { get; set; }
        public bool EstaVencida { get; set; }
    }

    public class CreateActividadCrmViewModel
    {
        public int? LeadId { get; set; }
        public int? OportunidadId { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "Debe seleccionar un tipo")]
        public int Tipo { get; set; }

        [Display(Name = "Dirección")]
        public int Direccion { get; set; }

        [Display(Name = "Asunto")]
        [Required(ErrorMessage = "El asunto es obligatorio")]
        [StringLength(200)]
        public string Asunto { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        [StringLength(1000)]
        public string? Descripcion { get; set; }

        [Display(Name = "Fecha Programada")]
        public DateTime? FechaProgramada { get; set; }
    }

    public class CompletarActividadViewModel
    {
        public int ActividadCrmId { get; set; }

        [Display(Name = "Resultado")]
        [Required(ErrorMessage = "Debe indicar el resultado")]
        [StringLength(500)]
        public string Resultado { get; set; } = string.Empty;

        [Display(Name = "Duración (minutos)")]
        public int? DuracionMinutos { get; set; }

        [Display(Name = "Próximo Contacto")]
        public DateTime? ProximoContacto { get; set; }
    }
}