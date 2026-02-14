using System.ComponentModel.DataAnnotations;

namespace SmartAdmin.Models.Vehiculo
{
    public class VehiculoViewModel
    {
        public int VehiculoId { get; set; }
        public string Vin { get; set; } = null!;
        public string? Placa { get; set; }
        public int MarcaId { get; set; }
        public int ModeloId { get; set; }
        public int? VersionId { get; set; }
        public int Anio { get; set; }
        public string? Color { get; set; }
        public int TipoCombustible { get; set; }
        public int? Transmision { get; set; }
        public int Estado { get; set; }
        public int? ClienteId { get; set; }
        public int KilometrajeActual { get; set; }
        public DateTime? GarantiaHasta { get; set; }

        // UI extras
        public string? ClienteNombre { get; set; }
        public string MarcaNombre { get; set; } = null!;
        public string ModeloNombre { get; set; } = null!;
        public string? VersionNombre { get; set; }
        public string Identificador { get; set; } = null!;
        public string DescripcionCompleta { get; set; } = null!;
        public bool EnGarantia { get; set; }
        public bool EstaVendido { get; set; }
        public bool DisponibleParaVenta { get; set; }
    }

    public class VehiculoDetalleViewModel
    {
        public int VehiculoId { get; set; }

        // Identificación
        public string Vin { get; set; } = null!;
        public string? Placa { get; set; }
        public string? NumeroMotor { get; set; }
        public string? NumeroChasis { get; set; }

        // Catálogo
        public int MarcaId { get; set; }
        public int ModeloId { get; set; }
        public int? VersionId { get; set; }
        public int Anio { get; set; }
        public string? Color { get; set; }
        public int TipoCombustible { get; set; }
        public int? Transmision { get; set; }

        // Estado
        public int Estado { get; set; }
        public int? UbicacionId { get; set; }
        public int? SucursalId { get; set; }

        // Importación
        public int Procedencia { get; set; }
        public string? NumeroImportacion { get; set; }
        public string? NumeroPoliza { get; set; }
        public DateTime? FechaIngresoPais { get; set; }
        public DateTime? FechaRecepcion { get; set; }
        public decimal? CostoImportacion { get; set; }

        // Ventas
        public int? ClienteId { get; set; }
        public decimal? PrecioLista { get; set; }
        public decimal? PrecioVenta { get; set; }
        public DateTime? FechaVenta { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string? VendedorId { get; set; }

        // Taller
        public int KilometrajeActual { get; set; }
        public DateTime? FechaPrimeraMatricula { get; set; }
        public DateTime? GarantiaHasta { get; set; }

        // General
        public string? Observaciones { get; set; }
        public DateTime FechaRegistro { get; set; }

        // UI extras
        public string? ClienteNombre { get; set; }
        public string MarcaNombre { get; set; } = null!;
        public string ModeloNombre { get; set; } = null!;
        public string? VersionNombre { get; set; }
        public string? UbicacionNombre { get; set; }
        public string? SucursalNombre { get; set; }
        public string Identificador { get; set; } = null!;
        public string DescripcionCompleta { get; set; } = null!;
        public bool EnGarantia { get; set; }
        public bool EstaVendido { get; set; }
        public bool DisponibleParaVenta { get; set; }
        public int CantidadServicios { get; set; }
    }

    public class CreateVehiculoViewModel
    {
        [Display(Name = "VIN")]
        [Required(ErrorMessage = "El VIN es obligatorio")]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "El VIN debe tener 17 caracteres")]
        public string Vin { get; set; } = string.Empty;

        [Display(Name = "Placa")]
        [StringLength(15)]
        public string? Placa { get; set; }

        [Display(Name = "Número de Motor")]
        [StringLength(50)]
        public string? NumeroMotor { get; set; }

        [Display(Name = "Número de Chasis")]
        [StringLength(50)]
        public string? NumeroChasis { get; set; }

        [Display(Name = "Marca")]
        [Required(ErrorMessage = "La marca es obligatoria")]
        public int MarcaId { get; set; }

        [Display(Name = "Modelo")]
        [Required(ErrorMessage = "El modelo es obligatorio")]
        public int ModeloId { get; set; }

        [Display(Name = "Versión")]
        public int? VersionId { get; set; }

        [Display(Name = "Año")]
        [Required(ErrorMessage = "El año es obligatorio")]
        [Range(1900, 2100)]
        public int Anio { get; set; }

        [Display(Name = "Color")]
        [StringLength(50)]
        public string? Color { get; set; }

        [Display(Name = "Tipo de Combustible")]
        public int TipoCombustible { get; set; } = 1;

        [Display(Name = "Transmisión")]
        public int? Transmision { get; set; }

        [Display(Name = "Estado")]
        public int Estado { get; set; } = 1;

        [Display(Name = "Sucursal")]
        public int? SucursalId { get; set; }

        [Display(Name = "Procedencia")]
        public int Procedencia { get; set; } = 2;

        [Display(Name = "Número de Importación")]
        [StringLength(50)]
        public string? NumeroImportacion { get; set; }

        [Display(Name = "Número de Póliza")]
        [StringLength(50)]
        public string? NumeroPoliza { get; set; }

        [Display(Name = "Fecha Ingreso al País")]
        public DateTime? FechaIngresoPais { get; set; }

        [Display(Name = "Fecha Recepción")]
        public DateTime? FechaRecepcion { get; set; }

        [Display(Name = "Costo de Importación")]
        public decimal? CostoImportacion { get; set; }

        [Display(Name = "Cliente")]
        public int? ClienteId { get; set; }

        [Display(Name = "Precio de Lista")]
        public decimal? PrecioLista { get; set; }

        [Display(Name = "Kilometraje Actual")]
        [Range(0, int.MaxValue)]
        public int KilometrajeActual { get; set; } = 0;

        [Display(Name = "Fecha Primera Matrícula")]
        public DateTime? FechaPrimeraMatricula { get; set; }

        [Display(Name = "Garantía Hasta")]
        public DateTime? GarantiaHasta { get; set; }

        [Display(Name = "Observaciones")]
        [StringLength(500)]
        public string? Observaciones { get; set; }
    }

    public class EditVehiculoViewModel
    {
        public int VehiculoId { get; set; }

        [Display(Name = "VIN")]
        [Required(ErrorMessage = "El VIN es obligatorio")]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "El VIN debe tener 17 caracteres")]
        public string Vin { get; set; } = string.Empty;

        [Display(Name = "Placa")]
        [StringLength(15)]
        public string? Placa { get; set; }

        [Display(Name = "Número de Motor")]
        [StringLength(50)]
        public string? NumeroMotor { get; set; }

        [Display(Name = "Número de Chasis")]
        [StringLength(50)]
        public string? NumeroChasis { get; set; }

        [Display(Name = "Marca")]
        [Required(ErrorMessage = "La marca es obligatoria")]
        public int MarcaId { get; set; }

        [Display(Name = "Modelo")]
        [Required(ErrorMessage = "El modelo es obligatorio")]
        public int ModeloId { get; set; }

        [Display(Name = "Versión")]
        public int? VersionId { get; set; }

        [Display(Name = "Año")]
        [Required(ErrorMessage = "El año es obligatorio")]
        [Range(1900, 2100)]
        public int Anio { get; set; }

        [Display(Name = "Color")]
        [StringLength(50)]
        public string? Color { get; set; }

        [Display(Name = "Tipo de Combustible")]
        public int TipoCombustible { get; set; }

        [Display(Name = "Transmisión")]
        public int? Transmision { get; set; }

        [Display(Name = "Estado")]
        public int Estado { get; set; }

        [Display(Name = "Sucursal")]
        public int? SucursalId { get; set; }

        [Display(Name = "Procedencia")]
        public int Procedencia { get; set; }

        [Display(Name = "Número de Importación")]
        [StringLength(50)]
        public string? NumeroImportacion { get; set; }

        [Display(Name = "Número de Póliza")]
        [StringLength(50)]
        public string? NumeroPoliza { get; set; }

        [Display(Name = "Fecha Ingreso al País")]
        public DateTime? FechaIngresoPais { get; set; }

        [Display(Name = "Fecha Recepción")]
        public DateTime? FechaRecepcion { get; set; }

        [Display(Name = "Costo de Importación")]
        public decimal? CostoImportacion { get; set; }

        [Display(Name = "Cliente")]
        public int? ClienteId { get; set; }

        [Display(Name = "Precio de Lista")]
        public decimal? PrecioLista { get; set; }

        [Display(Name = "Kilometraje Actual")]
        [Range(0, int.MaxValue)]
        public int KilometrajeActual { get; set; }

        [Display(Name = "Fecha Primera Matrícula")]
        public DateTime? FechaPrimeraMatricula { get; set; }

        [Display(Name = "Garantía Hasta")]
        public DateTime? GarantiaHasta { get; set; }

        [Display(Name = "Observaciones")]
        [StringLength(500)]
        public string? Observaciones { get; set; }

        [Display(Name = "Precio de Venta")]
        public decimal? PrecioVenta { get; set; }

        [Display(Name = "Fecha de Venta")]
        public DateTime? FechaVenta { get; set; }

        [Display(Name = "Fecha de Entrega")]
        public DateTime? FechaEntrega { get; set; }

        [Display(Name = "Vendedor")]
        public string? VendedorId { get; set; }
    }

    public class ProcesarVentaViewModel
    {
        public int VehiculoId { get; set; }

        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "El cliente es obligatorio")]
        public int ClienteId { get; set; }

        [Display(Name = "Precio de Venta")]
        [Required(ErrorMessage = "El precio de venta es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal PrecioVenta { get; set; }

        [Display(Name = "Fecha de Venta")]
        [Required(ErrorMessage = "La fecha de venta es obligatoria")]
        public DateTime FechaVenta { get; set; } = DateTime.Now;
    }

    public class ProcesarEntregaViewModel
    {
        public int VehiculoId { get; set; }

        [Display(Name = "Fecha de Entrega")]
        [Required(ErrorMessage = "La fecha de entrega es obligatoria")]
        public DateTime FechaEntrega { get; set; } = DateTime.Now;
    }

    public class CambiarEstadoRequest
    {
        public int VehiculoId { get; set; }
        public int NuevoEstado { get; set; }
    }

    public class ActualizarKilometrajeRequest
    {
        public int VehiculoId { get; set; }
        public int NuevoKilometraje { get; set; }
    }
}
