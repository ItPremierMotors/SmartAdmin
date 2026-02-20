using System;
using System.ComponentModel.DataAnnotations;

namespace SmartAdmin.Models.Enums
{
    /// <summary>
    /// Tipo de cliente (persona natural o empresa).
    /// </summary>
    public enum TipoCliente
    {
        Persona = 1,
        Empresa = 2
    }

    /// <summary>
    /// Segmento de vehículo según su carrocería.
    /// </summary>
  public enum SegmentoVehiculo
{
    [Display(Name = "Sedán")]
    Sedan = 1,
    [Display(Name = "SUV")]
    Suv = 2,
    [Display(Name = "Pickup")]
    Pickup = 3,
    [Display(Name = "Van")]
    Van = 4,
    [Display(Name = "Coupé")]
    Coupe = 5,
    [Display(Name = "Hatchback")]
    Hatchback = 6,
    [Display(Name = "Deportivo")]
    Deportivo = 7,
    [Display(Name = "Eléctrico")]
    Electrico = 8,
    [Display(Name = "Híbrido")]
    Hibrido = 9,
    [Display(Name = "Otro")]
    Otro = 99
}
    /// <summary>
    /// Tipo de tracción del vehículo.
    /// </summary>
    public enum TipoTraccion
    {
        [Display(Name = "Delantera (FWD)")]
        Fwd = 1,
        [Display(Name = "Trasera (RWD)")]
        Rwd = 2,
        [Display(Name = "Integral (AWD)")]
        Awd = 3,
        [Display(Name = "4x4 (4WD)")]
        FourWd = 4
    }

    /// <summary>
    /// Tipo de combustible del vehículo.
    /// </summary>
    public enum TipoCombustible
    {
        [Display(Name = "Gasolina")]
        Gasolina = 1,
        [Display(Name = "Diésel")]
        Diesel = 2,
        [Display(Name = "Híbrido")]
        Hibrido = 3,
        [Display(Name = "Eléctrico")]
        Electrico = 4,
        [Display(Name = "Gas Natural")]
        GasNatural = 5,
        [Display(Name = "GLP")]
        Glp = 6
    }

    /// <summary>
    /// Procedencia del vehículo.
    /// </summary>
    public enum ProcedenciaVehiculo
    {
        [Display(Name = "Local")]
        Local = 1,
        [Display(Name = "Importado")]
        Importado = 2
    }

    /// <summary>
    /// Clasificación de servicio según duración.
    /// </summary>
    public enum ClasificacionServicio
    {
        Rapido = 1,
        Estandar = 2,
        Largo = 3
    }

    /// <summary>
    /// Estado de la cita.
    /// </summary>
    public enum EstadoCita
    {
        Agendada = 1,
        Confirmada = 2,
        EnProceso = 3,
        Completada = 4,
        Cancelada = 5,
        NoShow = 6,
        Transferida = 7
    }

    /// <summary>
    /// Tipo de ingreso del vehículo.
    /// </summary>
    public enum TipoIngreso
    {
        Cita = 1,
        WalkIn = 2,
        Garantia = 3
    }

    /// <summary>
    /// Turno del taller.
    /// </summary>
    public enum TurnoTaller
    {
        Manana = 1,
        Tarde = 2,
        Completo = 3
    }

    /// <summary>
    /// Tipo de bloque horario.
    /// </summary>
    public enum TipoBloqueHorario
    {
        Estandar = 1,
        Rapido = 2,
        ColisionPintura = 3
    }

    /// <summary>
    /// Estado de un servicio dentro de la OS.
    /// </summary>
    public enum EstadoServicioOS
    {
        Pendiente = 1,
        EnProceso = 2,
        Completado = 3,
        Cancelado = 4
    }

    /// <summary>
    /// Estado de asignación de técnico.
    /// </summary>
    public enum EstadoAsignacion
    {
        Asignado = 1,
        EnProceso = 2,
        Completado = 3,
        Pausado = 4
    }

    /// <summary>
    /// Tipo de evidencia fotográfica.
    /// </summary>
    public enum TipoEvidencia
    {
        FotoFrontal = 1,
        FotoTrasera = 2,
        FotoLateralIzq = 3,
        FotoLateralDer = 4,
        FotoInterior = 5,
        FotoTablero = 6,
        FotoDano = 7,
        Otro = 99
    }

    /// <summary>
    /// Estado del vehículo en el inventario.
    /// </summary>
    public enum EstadoVehiculo
    {
        [Display(Name = "En Tránsito")]
        EnTransito = 1,
        [Display(Name = "En Aduana")]
        EnAduana = 2,
        [Display(Name = "En Bodega")]
        EnBodega = 3,
        [Display(Name = "En Exhibición")]
        EnExhibicion = 4,
        [Display(Name = "Reservado")]
        Reservado = 5,
        [Display(Name = "Vendido")]
        Vendido = 6,
        [Display(Name = "Entregado")]
        Entregado = 7
    }

    /// <summary>
    /// Tipo de transmisión del vehículo.
    /// </summary>
    public enum TipoTransmision
    {
        [Display(Name = "Manual")]
        Manual = 1,
        [Display(Name = "Automática")]
        Automatica = 2,
        [Display(Name = "CVT")]
        CVT = 3,
        [Display(Name = "Dual Clutch")]
        DualClutch = 4,
        [Display(Name = "Semiautomática")]
        Semiautomatica = 5
    }
}