using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SPCOIN.Models;
using System.Data.SqlClient;

namespace SPCOIN.Controllers
{
    public class DetalleVentaController : Controller
    {
        private readonly Context _context;
        public DetalleVentaController(Context context)
        {
            _context = context;
        }
        // GET: DetalleVentaController
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Detalle(int codigoVenta)
        {
            try
            {
                List<DetalleVenta> detallesVenta = new List<DetalleVenta>();
                using (SqlConnection con = new SqlConnection(_context.Conexion))
                {
                    using (SqlCommand cmd = new SqlCommand("SDETALLEVENTA", con))
                    {
                        Console.WriteLine(codigoVenta);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CODIGOVENTA", System.Data.SqlDbType.Int).Value = codigoVenta;
                        con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                DetalleVenta detalleVenta = new DetalleVenta()
                                {
                                    CodigoDetalleVenta = Convert.ToInt32(reader["CODIGODETALLEVENTA"]),
                                    CodigoProducto = Convert.ToString(reader["CODIGO"]),
                                    Nombre = Convert.ToString(reader["NOMBRE"]),
                                    Unidades = Convert.ToInt32(reader["UNIDADES"]),
                                    Precio = Convert.ToDecimal(reader["PRECIO"]),
                                    Total = Convert.ToDecimal(reader["TOTAL"]),
                                };
                                detallesVenta.Add(detalleVenta);
                            }
                        }
                    }
                }
                var response = new
                {
                    status = true,
                    data = detallesVenta
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
