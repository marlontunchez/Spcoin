using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SPCOIN.Models;
using System.Data.SqlClient;

namespace SPCOIN.Controllers
{
    public class VentasController : Controller
    {
        private readonly Context _context;
        public VentasController(Context context)
        {
            _context = context;
        }
        // GET: VentasController
        public ActionResult Index()
        {
            return View();
        }


        public async Task<JsonResult> obtenerVentas(DateTime fechaInicial, DateTime fechaFinal, string busqueda)
        {
            try
            {
                List<Ventas> ventas = new List<Ventas>();
                using (SqlConnection con = new SqlConnection(_context.Conexion))
                {
                    using (SqlCommand cmd = new SqlCommand("SVENTASREALIZADAS2", con))
                    {
                        if (busqueda == null)
                        {
                            busqueda = "";
                        }
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@BUSCAR", System.Data.SqlDbType.VarChar).Value = busqueda;
                        cmd.Parameters.Add("@CODIGOASIGNACIONPERMISOS", System.Data.SqlDbType.BigInt).Value = HttpContext.Session.GetInt32("CODIGOASIGNACIONPERMISOS");
                        cmd.Parameters.Add("@COLUMNA", System.Data.SqlDbType.VarChar).Value ="C.NOMBRE";
                        cmd.Parameters.Add("@FECHAINICIAL", System.Data.SqlDbType.DateTime).Value = fechaInicial;
                        cmd.Parameters.Add("@FECHAFINAL", System.Data.SqlDbType.DateTime).Value = fechaFinal;
                           con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Ventas Venta = new Ventas()
                                {
                                    CodigoVenta = Convert.ToInt64(reader["CODIGOVENTA"]),
                                    Correlativo = Convert.ToInt64(reader["CORRELATIVO"]),
                                    Fecha = Convert.ToDateTime(reader["FECHA"]),
                                    CodigoCliente = Convert.ToInt64(reader["CODCLIENTE"]),
                                    Nombre = Convert.ToString(reader["NOMBRE"]),
                                    Direccion = Convert.ToString(reader["DIRECCION"]),
                                    Nit = Convert.ToString(reader["NIT"]),
                                    FormaDePago = Convert.ToString(reader["FORMADEPAGO"]),
                                    Documento = Convert.ToString(reader["DOCUMENTO"]),
                                    Total = Convert.ToDecimal(reader["TOTAL"]),
                                    Estado = Convert.ToString(reader["ESTADO"]),
                                    UuId = Convert.ToString(reader["UUID"]),
                                    Numero = Convert.ToString(reader["NUMERO"]),
                                    //FechaCertificacion = Convert.ToDateTime(reader["FECHACERTIFICACION"]),
                                    CodigoDocumento = Convert.ToInt64(reader["CODIGODOCUMENTO"]),
                                    Vendedor = Convert.ToString(reader["VENDEDOR"]),
                                    Comentario = Convert.ToString(reader["COMENTARIO"])
                                };
                                ventas.Add(Venta);
                            }
                        }
                    }
                }
                var response = new
                {
                    status = true,
                    data = ventas
                };
                return Json(response);
            }
            catch (Exception e)
            {
                var response = new
                {
                    status = false,
                    message = e.Message
                };
                return Json(response);
            }
        }



    }
}

