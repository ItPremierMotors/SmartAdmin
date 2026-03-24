namespace SmartAdmin.Models.Crm
{
    // ══════════════════════════════════════════════════════════════
    //  ViewModels para Dashboard CRM — alineados con DTOs del backend
    // ══════════════════════════════════════════════════════════════

    public class DashboardLeadsVM
    {
        public int LeadsNuevos { get; set; }
        public int LeadsConvertidos { get; set; }
        public decimal TasaConversion { get; set; }
        public double TiempoPromedioRespuestaHoras { get; set; }
        public int LeadsSinContactar { get; set; }
        public int LeadsDescartados {get; set;}
        public List<KpiAgrupadoVM> LeadsPorOrigen { get; set; } = new();
    }

    public class DashboardPipelineVM
    {
        public int OportunidadesAbiertas { get; set; }
        public decimal ValorTotalPipeline { get; set; }
        public decimal WinRate { get; set; }
        public decimal ValorPromedioOportunidad { get; set; }
        public List<KpiAgrupadoVM> OportunidadesPorEtapa { get; set; } = new();
    }

    public class DashboardEquipoVM
    {
        public List<KpiVendedorVM> Vendedores { get; set; } = new();
    }

    public class KpiVendedorVM
    {
        public string VendedorId { get; set; } = null!;
        public string VendedorNombre { get; set; } = null!;
        public int LeadsAsignados { get; set; }
        public int OportunidadesGanadas { get; set; }
        public decimal ValorVendido { get; set; }
        public int CargaActual { get; set; }
    }

    public class DashboardRetencionVM
    {
        public int LeadsDescartados { get; set; }
        public int OportunidadesPerdidas { get; set; }
        public int OportunidadesCanceladas { get; set; }
        public List<KpiAgrupadoVM> MotivoDescarte { get; set; } = new();
        public List<KpiAgrupadoVM> MotivoPerdida { get; set; } = new();
    }

    public class DashboardActividadVM
    {
        public int TotalActividades { get; set; }
        public int Completadas { get; set; }
        public int Vencidas { get; set; }
        public decimal TasaCumplimiento { get; set; }
        public List<KpiAgrupadoVM> ActividadesPorTipo { get; set; } = new();
        public List<KpiVendedorActividadVM> ActividadesPorVendedor { get; set; } = new();
    }

    public class KpiVendedorActividadVM
    {
        public string VendedorId { get; set; } = null!;
        public string VendedorNombre { get; set; } = null!;
        public int Realizadas { get; set; }
        public int Vencidas { get; set; }
        public decimal TasaCumplimiento { get; set; }
    }

    public class DashboardOrigenVM
    {
        public List<KpiOrigenVM> Origenes { get; set; } = new();
    }

    public class KpiOrigenVM
    {
        public string Origen { get; set; } = null!;
        public int TotalLeads { get; set; }
        public int Convertidos { get; set; }
        public decimal TasaConversion { get; set; }
        public int OportunidadesGanadas { get; set; }
        public decimal WinRate { get; set; }
    }

    public class DashboardTiempoVM
    {
        public double TiempoPromedioRespuestaHoras { get; set; }
        public double CicloVentaPromedioDias { get; set; }
    }

    public class KpiAgrupadoVM
    {
        public string Nombre { get; set; } = null!;
        public int Cantidad { get; set; }
    }

    // ViewModel para alertas de leads fríos
    public class AlertasLeadsFriosVM
    {
        public List<LeadViewModel> SinContactar { get; set; } = new();
        public List<LeadViewModel> Frios { get; set; } = new();
        public List<LeadViewModel> MuyFrios { get; set; } = new();
        public List<LeadViewModel> CandidatosDescarte { get; set; } = new();
    }

    // ViewModel para VendedorSucursal
    public class VendedorSucursalVM
    {
        public int VendedorSucursalId { get; set; }
        public string VendedorId { get; set; } = null!;
        public string? VendedorNombre { get; set; }
        public int SucursalId { get; set; }
        public string? SucursalNombre { get; set; }
        public bool EstaActivo { get; set; }
    }

    public class AsignarVendedorSucursalVM
    {
        public string VendedorId { get; set; } = null!;
        public int SucursalId { get; set; }
    }
}
