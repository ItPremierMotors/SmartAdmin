using System.ComponentModel.DataAnnotations;

namespace SmartAdmin.Models.Crm
{
    public class NotaCrmViewModel
    {
        public int NotaCrmId { get; set; }
        public string Contenido { get; set; } = null!;
        public bool EsPrivada { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Datos relacionados
        public int? LeadId { get; set; }
        public int? OportunidadId { get; set; }
        public string AutorId { get; set; } = null!;
        public string AutorNombre { get; set; } = null!;
    }

    public class CreateNotaCrmViewModel
    {
        public int? LeadId { get; set; }
        public int? OportunidadId { get; set; }

        [Display(Name = "Contenido")]
        [Required(ErrorMessage = "El contenido es obligatorio")]
        [StringLength(2000)]
        public string Contenido { get; set; } = string.Empty;

        [Display(Name = "Nota Privada")]
        public bool EsPrivada { get; set; }
    }
}