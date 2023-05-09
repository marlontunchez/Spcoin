namespace SPCOIN.Models
{
    public class DetalleEntrada
    {

        public int CodigoEntrada { get; set; }
        public int CodigoDetalleEntrada { get; set; }
        public string? CodigoProducto { get; set; }
        public string? Nombre { get; set; }
        public int Unidades { get; set; }
        public decimal Costo { get; set; }
        public decimal Total { get; set; }
        public int CodigoAsignacionPermisos { get; set; }
    
    }
}
