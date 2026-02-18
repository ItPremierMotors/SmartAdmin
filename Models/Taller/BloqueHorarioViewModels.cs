using SmartAdmin.Models.Enums;

namespace SmartAdmin.Models.Taller
{
    public class BloqueHorarioViewModel
    {
        public int BloqueId { get; set; }
        public int CapacidadId { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int CapacidadMaximaVehiculos { get; set; }
        public int VehiculosAgendados { get; set; }
        public TipoBloqueHorario TipoBloque { get; set; }

        // Extras UI
        public int EspaciosDisponibles { get; set; }
        public bool TieneEspacioDisponible { get; set; }
        public int DuracionMinutos { get; set; }
        public string HoraInicioFormateada => HoraInicio.ToString(@"hh\:mm");
        public string HoraFinFormateada => HoraFin.ToString(@"hh\:mm");
        public string RangoHorario => $"{HoraInicioFormateada} - {HoraFinFormateada}";
        public string TipoBloqueNombre => TipoBloque.ToString();
        public DateTime? Fecha { get; set; }
        public string? SucursalNombre { get; set; }
    }

    public class CreateBloqueHorarioViewModel
    {
        public int CapacidadId { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int CapacidadMaximaVehiculos { get; set; } = 1;
        public TipoBloqueHorario TipoBloque { get; set; } = TipoBloqueHorario.Estandar;
    }

    public class UpdateBloqueHorarioViewModel
    {
        public int BloqueId { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int CapacidadMaximaVehiculos { get; set; }
        public TipoBloqueHorario TipoBloque { get; set; }
    }

    public class GenerarBloquesViewModel
    {
        public int CapacidadId { get; set; }
        public TimeSpan HoraInicioJornada { get; set; } = new TimeSpan(8, 0, 0);
        public TimeSpan HoraFinJornada { get; set; } = new TimeSpan(17, 0, 0);
        public int DuracionBloqueMinutos { get; set; } = 60;
        public int CapacidadPorBloque { get; set; } = 2;
        public TipoBloqueHorario TipoBloque { get; set; } = TipoBloqueHorario.Estandar;
    }
}
