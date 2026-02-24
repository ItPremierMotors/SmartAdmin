namespace SmartAdmin.Models.Taller
{
    // ─── Orden de Servicio ───

    public class OrdenServicioViewModel
    {
        public int OsId { get; set; }
        public string NumeroOs { get; set; } = null!;
        public int? CitaId { get; set; }
        public int VehiculoId { get; set; }
        public int ClienteId { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public int EstadoId { get; set; }
        public int KilometrajeIngreso { get; set; }
        public decimal? NivelCombustible { get; set; }
        public int TipoIngreso { get; set; }
        public bool EsGarantia { get; set; }
        public string? ObservacionesApertura { get; set; }
        public string? ObservacionesCierre { get; set; }
        public string? AsesorId { get; set; }
        public string? CoordinadorId { get; set; }
        public decimal TotalManoObra { get; set; }
        public decimal TotalRepuestos { get; set; }
        public decimal TotalGeneral { get; set; }
        public int? SucursalId { get; set; }

        // Extras UI
        public string? CodigoCita { get; set; }
        public string ClienteNombre { get; set; } = null!;
        public string? ClienteTelefono { get; set; }
        public string VehiculoDescripcion { get; set; } = null!;
        public string? VehiculoPlaca { get; set; }
        public string EstadoNombre { get; set; } = null!;
        public string EstadoCodigo { get; set; } = null!;
        public string? SucursalNombre { get; set; }
        public int? NivelCombustiblePorcentaje { get; set; }
        public bool EsWalkIn { get; set; }
        public bool EstaAbierta { get; set; }
        public bool PuedeModificarse { get; set; }
        public string? TipoIngresoNombre { get; set; }
    }

    public class OrdenServicioDetalleViewModel : OrdenServicioViewModel
    {
        public string? ClienteEmail { get; set; }
        public string? VehiculoVin { get; set; }
        public int VehiculoKilometrajeAnterior { get; set; }
        public bool VehiculoEnGarantia { get; set; }
        public int CantidadServicios { get; set; }
        public int CantidadTecnicos { get; set; }
        public int CantidadEvidencias { get; set; }
    }

    // ─── Create/Update OS ───

    public class CreateOsFromCitaViewModel
    {
        public int CitaId { get; set; }
        public int KilometrajeIngreso { get; set; }
        public decimal? NivelCombustible { get; set; }
        public bool EsGarantia { get; set; }
        public string? ObservacionesApertura { get; set; }
        public string? AsesorId { get; set; }
        public string? CoordinadorId { get; set; }
    }

    public class CreateOsWalkInViewModel
    {
        public int ClienteId { get; set; }
        public int VehiculoId { get; set; }
        public int KilometrajeIngreso { get; set; }
        public decimal? NivelCombustible { get; set; }
        public bool EsGarantia { get; set; }
        public string? ObservacionesApertura { get; set; }
        public string? AsesorId { get; set; }
        public string? CoordinadorId { get; set; }
        public int? SucursalId { get; set; }
    }

    public class UpdateOrdenServicioViewModel
    {
        public int OsId { get; set; }
        public int KilometrajeIngreso { get; set; }
        public decimal? NivelCombustible { get; set; }
        public bool EsGarantia { get; set; }
        public string? ObservacionesApertura { get; set; }
        public string? AsesorId { get; set; }
        public string? CoordinadorId { get; set; }
    }

    public class CambiarEstadoOsViewModel
    {
        public int OsId { get; set; }
        public int NuevoEstadoId { get; set; }
        public string? Observaciones { get; set; }
    }

    public class CerrarOsViewModel
    {
        public int OsId { get; set; }
        public string? ObservacionesCierre { get; set; }
        public string? ProximaRevision { get; set; }
    }

    public class CancelarOsViewModel
    {
        public int OsId { get; set; }
        public string Motivo { get; set; } = null!;
    }

    // ─── OsServicio ───

    public class OsServicioViewModel
    {
        public int OsServicioId { get; set; }
        public int OsId { get; set; }
        public int TipoServicioId { get; set; }
        public string DescripcionTrabajo { get; set; } = null!;
        public int Estado { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? TecnicoAsignadoId { get; set; }
        public string? Observaciones { get; set; }

        // Extras UI
        public string NumeroOs { get; set; } = null!;
        public string TipoServicioNombre { get; set; } = null!;
        public string? TipoServicioCodigo { get; set; }
        public string? TecnicoNombre { get; set; }
        public string? EstadoNombre { get; set; }
        public bool EstaCompletado { get; set; }
        public bool EstaCancelado { get; set; }
        public bool EstaEnProceso { get; set; }
        public int? TiempoTrabajoMinutos { get; set; }
        public int CantidadAsignaciones { get; set; }
    }

    public class AgregarServicioViewModel
    {
        public int OsId { get; set; }
        public int TipoServicioId { get; set; }
        public string? DescripcionTrabajo { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public int Cantidad { get; set; } = 1;
        public int? TecnicoAsignadoId { get; set; }
        public string? Observaciones { get; set; }
    }

    public class UpdateOsServicioViewModel
    {
        public int OsServicioId { get; set; }
        public string DescripcionTrabajo { get; set; } = null!;
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public int? TecnicoAsignadoId { get; set; }
        public string? Observaciones { get; set; }
    }

    // ─── AsignacionTecnico ───

    public class AsignacionTecnicoViewModel
    {
        public int AsignacionId { get; set; }
        public int OsId { get; set; }
        public int TecnicoId { get; set; }
        public int? OsServicioId { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int Estado { get; set; }
        public string? Observaciones { get; set; }

        // Extras UI
        public string NumeroOs { get; set; } = null!;
        public string TecnicoNombre { get; set; } = null!;
        public string? TecnicoCodigo { get; set; }
        public string? ServicioDescripcion { get; set; }
        public string? EstadoNombre { get; set; }
        public bool EstaActiva { get; set; }
        public int? TiempoTrabajadoMinutos { get; set; }
    }

    public class CreateAsignacionViewModel
    {
        public int OsId { get; set; }
        public int TecnicoId { get; set; }
        public int? OsServicioId { get; set; }
        public string? Observaciones { get; set; }
    }

    public class ReasignarViewModel
    {
        public int AsignacionId { get; set; }
        public int NuevoTecnicoId { get; set; }
        public string? Observaciones { get; set; }
    }

    // ─── Reporte OS ───

    public class ReporteOsViewModel
    {
        public OrdenServicioDetalleViewModel Detalle { get; set; } = null!;
        public List<OsServicioViewModel> Servicios { get; set; } = new();
    }
}
