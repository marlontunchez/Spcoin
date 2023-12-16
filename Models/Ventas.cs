namespace SPCOIN.Models
{
    public class Ventas
    {
        public Int64 CodigoVenta { get; set; }
        public Int64 Correlativo { get; set; }
        public DateTime Fecha { get; set; }
        public Int64 CodigoCliente { get; set; }
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public string? Nit { get; set; }
        public string? FormaDePago { get; set; }
        public string? Documento { get; set; }
        public decimal Total { get; set; }
        public  string? Estado { get; set; }
        public string? UuId { get; set; }
        public string? Numero { get; set; }
        public DateTime FechaCertificacion { get; set; }
        public Int64 CodigoDocumento { get; set; }
        public string? Vendedor { get; set; }
        public string? Comentario { get; set; }
        public  decimal Totalventas { get; set; }
        public decimal Margen { get; set; }
    }
}
