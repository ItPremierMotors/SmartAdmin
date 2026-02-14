namespace SmartAdmin.Models.Catalogo.EstadoOs
{
    public class EstadoOsViewModel
    {
        public int EstadoId { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int OrdenSecuencial { get; set; }
        public bool EsEstadoFinal { get; set; }
        public bool PermiteModificacion { get; set; }
    }
}
