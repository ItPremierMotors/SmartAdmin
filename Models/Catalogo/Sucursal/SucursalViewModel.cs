namespace SmartAdmin.Models.Catalogo.Sucursal
{
    public class SucursalViewModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public string? Direccion { get; set; }
        public bool Activa { get; set; }
    }
}
