namespace SPCOIN.Models
{
    public class DetalleReparacion
    {
        public int CodigoReparacion { get; set; }
        public int CodigoDetalleReparacion { get; set; }  
        public string CodigoProducto { get; set; }  
        public string Nombre { get; set; }
        public int Unidades { get; set; }   
        public decimal precio { get; set; } 
        public decimal total    { get; set; }
        public int CodigoAsignacionPermisos { get; set; }   
        public decimal Descuento { get; set; }      


    }
}
