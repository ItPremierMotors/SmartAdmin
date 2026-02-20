using System.ComponentModel.DataAnnotations;
using SmartAdmin.Models.Enums;

namespace SmartAdmin.Models.Catalogo.VersionVehiculo
{
    /// <summary>
    /// Para listar y mostrar detalles de versiones
    /// </summary>
    public class VersionVehiculoViewModel
    {
        public int VersionId { get; set; }
        public int ModeloId { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string? Motor { get; set; }
        public TipoTransmision? Transmision { get; set; }
        public TipoTraccion? Traccion { get; set; }
        public int? NumPuertas { get; set; }
        public int? NumPasajeros { get; set; }
        public TipoCombustible TipoCombustible { get; set; }
        public decimal? Cilindraje { get; set; }
        public int? PotenciaHp { get; set; }
        public int? TorqueNm { get; set; }
        public decimal? PrecioBase { get; set; }
        public int? AnioVersion { get; set; }
        public string? CaracteristicasPrincipales { get; set; }

        // Datos extras para UI
        public string? ModeloNombre { get; set; }
        public string? MarcaNombre { get; set; }
        public string DescripcionCompleta { get; set; } = null!;

        // Propiedades calculadas
        public string TransmisionNombre => Transmision?.ToString() ?? "N/A";
        public string TraccionNombre => Traccion?.ToString() ?? "N/A";
        public string TipoCombustibleNombre => TipoCombustible.ToString();
        public string MarcaModelo => $"{MarcaNombre} {ModeloNombre}";
    }

    /// <summary>
    /// Para crear nueva versión
    /// </summary>
    public class CreateVersionVehiculoViewModel
    {
        [Display(Name = "Modelo")]
        [Required(ErrorMessage = "El modelo es obligatorio")]
        public int ModeloId { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(20, ErrorMessage = "El código no puede exceder 20 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Motor")]
        [StringLength(50, ErrorMessage = "El motor no puede exceder 50 caracteres")]
        public string? Motor { get; set; }

        [Display(Name = "Transmisión")]
        public TipoTransmision? Transmision { get; set; }

        [Display(Name = "Tracción")]
        public TipoTraccion? Traccion { get; set; }

        [Display(Name = "Número de Puertas")]
        [Range(2, 6, ErrorMessage = "El número de puertas debe estar entre 2 y 6")]
        public int? NumPuertas { get; set; }

        [Display(Name = "Número de Pasajeros")]
        [Range(2, 9, ErrorMessage = "El número de pasajeros debe estar entre 2 y 9")]
        public int? NumPasajeros { get; set; }

        [Display(Name = "Tipo de Combustible")]
        [Required(ErrorMessage = "El tipo de combustible es obligatorio")]
        public TipoCombustible TipoCombustible { get; set; } = TipoCombustible.Gasolina;

        [Display(Name = "Cilindraje (L)")]
        [Range(0.1, 10.0, ErrorMessage = "El cilindraje debe estar entre 0.1 y 10.0 litros")]
        public decimal? Cilindraje { get; set; }

        [Display(Name = "Potencia (HP)")]
        [Range(1, 2000, ErrorMessage = "La potencia debe estar entre 1 y 2000 HP")]
        public int? PotenciaHp { get; set; }

        [Display(Name = "Torque (Nm)")]
        [Range(1, 3000, ErrorMessage = "El torque debe estar entre 1 y 3000 Nm")]
        public int? TorqueNm { get; set; }

        [Display(Name = "Precio Base")]
        [Range(0, 99999999, ErrorMessage = "El precio debe ser un valor positivo")]
        public decimal? PrecioBase { get; set; }

        [Display(Name = "Año de Versión")]
        [Range(2000, 2100, ErrorMessage = "El año debe estar entre 2000 y 2100")]
        public int? AnioVersion { get; set; }

        [Display(Name = "Características Principales")]
        [StringLength(1000, ErrorMessage = "Las características no pueden exceder 1000 caracteres")]
        public string? CaracteristicasPrincipales { get; set; }
    }

    /// <summary>
    /// Para editar versión existente
    /// </summary>
    public class EditVersionVehiculoViewModel
    {
        public int VersionId { get; set; }

        [Display(Name = "Modelo")]
        [Required(ErrorMessage = "El modelo es obligatorio")]
        public int ModeloId { get; set; }

        [Display(Name = "Código")]
        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(20, ErrorMessage = "El código no puede exceder 20 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Motor")]
        [StringLength(50, ErrorMessage = "El motor no puede exceder 50 caracteres")]
        public string? Motor { get; set; }

        [Display(Name = "Transmisión")]
        public TipoTransmision? Transmision { get; set; }

        [Display(Name = "Tracción")]
        public TipoTraccion? Traccion { get; set; }

        [Display(Name = "Número de Puertas")]
        [Range(2, 6, ErrorMessage = "El número de puertas debe estar entre 2 y 6")]
        public int? NumPuertas { get; set; }

        [Display(Name = "Número de Pasajeros")]
        [Range(2, 9, ErrorMessage = "El número de pasajeros debe estar entre 2 y 9")]
        public int? NumPasajeros { get; set; }

        [Display(Name = "Tipo de Combustible")]
        [Required(ErrorMessage = "El tipo de combustible es obligatorio")]
        public TipoCombustible TipoCombustible { get; set; }

        [Display(Name = "Cilindraje (L)")]
        [Range(0.1, 10.0, ErrorMessage = "El cilindraje debe estar entre 0.1 y 10.0 litros")]
        public decimal? Cilindraje { get; set; }

        [Display(Name = "Potencia (HP)")]
        [Range(1, 2000, ErrorMessage = "La potencia debe estar entre 1 y 2000 HP")]
        public int? PotenciaHp { get; set; }

        [Display(Name = "Torque (Nm)")]
        [Range(1, 3000, ErrorMessage = "El torque debe estar entre 1 y 3000 Nm")]
        public int? TorqueNm { get; set; }

        [Display(Name = "Precio Base")]
        [Range(0, 99999999, ErrorMessage = "El precio debe ser un valor positivo")]
        public decimal? PrecioBase { get; set; }

        [Display(Name = "Año de Versión")]
        [Range(2000, 2100, ErrorMessage = "El año debe estar entre 2000 y 2100")]
        public int? AnioVersion { get; set; }

        [Display(Name = "Características Principales")]
        [StringLength(1000, ErrorMessage = "Las características no pueden exceder 1000 caracteres")]
        public string? CaracteristicasPrincipales { get; set; }
    }
}
