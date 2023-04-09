namespace SPCOIN.Models
{
    public class Reparacion
    {
        public int CodigoReparacion { get; set; }
        public DateTime  Fecha { get; set; }
        public string? Nit { get; set; }
        public string? Nombre { get; set; }
        public string? Motocicleta { get; set; }
        public string? Mecanico { get; set; }
        public bool ConvertidoAVenta { get; set; }
        public int Codigoasignacionpermisos { get; set; }



    }
}
