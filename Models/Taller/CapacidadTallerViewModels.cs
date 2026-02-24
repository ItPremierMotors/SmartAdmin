using SmartAdmin.Models.Enums;

namespace SmartAdmin.Models.Taller
{
    public class CapacidadTallerViewModel
    {
        public int CapacidadId { get; set; }
        public DateTime Fecha { get; set; }
        public TurnoTaller Turno { get; set; }
        public int TecnicosDisponibles { get; set; }
        public int BahiasDisponibles { get; set; }
        public int MinutosPorTecnico { get; set; }
        public int MinutosDisponibles { get; set; }
        public int MinutosReservados { get; set; }
        public int MinutosUtilizados { get; set; }
        public int MinutosSobretiempo { get; set; }
        public bool PermiteSobretiempo { get; set; }
        public bool PermiteAgendamiento { get; set; }
        public string? Observaciones { get; set; }
        public int? SucursalId { get; set; }

        // Extras UI
        public string? SucursalNombre { get; set; }
        public int MinutosLibres { get; set; }
        public decimal PorcentajeOcupacion { get; set; }
        public decimal PorcentajeEficiencia { get; set; }
        public bool TuvoSobretiempo { get; set; }
        public string TurnoNombre => Turno.ToString();
    }

    public class CreateCapacidadTallerViewModel
    {
        public DateTime Fecha { get; set; }
        public TurnoTaller Turno { get; set; } = TurnoTaller.Completo;
        public int TecnicosDisponibles { get; set; }
        public int BahiasDisponibles { get; set; }
        public int MinutosPorTecnico { get; set; } = 480;
        public bool PermiteSobretiempo { get; set; } = true;
        public bool PermiteAgendamiento { get; set; } = true;
        public int? SucursalId { get; set; }
    }

    public class UpdateCapacidadTallerViewModel
    {
        public int CapacidadId { get; set; }
        public TurnoTaller Turno { get; set; }
        public int TecnicosDisponibles { get; set; }
        public int BahiasDisponibles { get; set; }
        public int MinutosPorTecnico { get; set; }
        public bool PermiteSobretiempo { get; set; }
        public bool PermiteAgendamiento { get; set; }
        public string? Observaciones { get; set; }
        public int? SucursalId { get; set; }
    }
}
