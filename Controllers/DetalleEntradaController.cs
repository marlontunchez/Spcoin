using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SPCOIN.Models;
using System.Data.SqlClient;

namespace SPCOIN.Controllers
{
    public class DetalleEntradaController : Controller
    {
        private readonly Context _context;
        public DetalleEntradaController(Context context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            return View();
        }

        // GET: DetalleEntradaController/Details/5

        public async Task<JsonResult> Detalle(int codigoEntrada)
        {
            try
            {
                List<DetalleEntrada> detallesEntrada = new List<DetalleEntrada>();
                using (SqlConnection con = new SqlConnection(_context.Conexion))
                {
                    using (SqlCommand cmd = new SqlCommand("SDETALLEENTRADA", con))
                    {
        
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CODIGOENTRADA", System.Data.SqlDbType.Int).Value = codigoEntrada;
                        con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                DetalleEntrada detalleEntrada = new DetalleEntrada()
                                {
                                    CodigoDetalleEntrada = Convert.ToInt32(reader["CODIGODETALLEENTRADA"]),
                                    CodigoProducto = Convert.ToString(reader["CODIGOPRODUCTO"]),
                                    Nombre = Convert.ToString(reader["NOMBRE"]),
                                    Unidades = Convert.ToInt32(reader["UNIDADES"]),
                                    Costo = Convert.ToDecimal(reader["COSTO"]),
                                    Total = Convert.ToDecimal(reader["TOTAL"]),
                                };
                                detallesEntrada.Add(detalleEntrada);
                            }
                        }
                    }
                }
                var response = new
                {
                    status = true,
                    data = detallesEntrada
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
