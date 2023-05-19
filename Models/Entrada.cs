namespace SPCOIN.Models
{
    public class Entrada
    {
        public Int64 CodigoEntrada { get; set; }
        public Int64 Correlativo { get; set; }
        public DateTime Fecha { get; set; }
        public string? Sucursal { get; set; }
        public int CodigoSucursal { get; set; }
        public string? Descripcion { get; set; }
        public decimal Total { get; set; }
        public string? Estado { get; set; }





    }
}
