using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SPCOIN.Models;
using System.Data.SqlClient;

namespace SPCOIN.Controllers
{
    public class EntradaController : Controller
    {

        private readonly Context _context;
        public EntradaController(Context context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            return View();
        }


        public async Task<JsonResult> ObtenerEntradas(DateTime fechaInicial, DateTime fechaFinal, string busqueda)
        {
            try
            {
                List<Entrada> entrada = new List<Entrada>();
                using (SqlConnection con = new SqlConnection(_context.Conexion))
                {
                    using (SqlCommand cmd = new SqlCommand("SENTRADA", con))
                    {
                        if (busqueda == null)
                        {
                            busqueda = "";
                        }
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CODIGOASIGNACIONPERMISOS", System.Data.SqlDbType.BigInt).Value = HttpContext.Session.GetInt32("CODIGOASIGNACIONPERMISOS");
                        cmd.Parameters.Add("@BUSCAR", System.Data.SqlDbType.VarChar).Value = busqueda;
                          cmd.Parameters.Add("@COLUMNA", System.Data.SqlDbType.VarChar).Value = "DESCRIPCION";
                        cmd.Parameters.Add("@FECHAINICIAL", System.Data.SqlDbType.DateTime).Value = fechaInicial;
                        cmd.Parameters.Add("@FECHAFINAL", System.Data.SqlDbType.DateTime).Value = fechaFinal;
                        con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                           
                            while (reader.Read())
                            {
                                Entrada Entradas = new Entrada()
                                {
                                    CodigoEntrada = Convert.ToInt64(reader["CODIGOENTRADA"]),
                                    Correlativo = Convert.ToInt64(reader["CORRELATIVO"]),
                                    Fecha = Convert.ToDateTime(reader["FECHA"]),
                                    Sucursal = Convert.ToString(reader["SUCURSAL"]),
                                    Descripcion = Convert.ToString(reader["DESCRIPCION"]),
                                    Estado = Convert.ToString(reader["ESTADO"]),
                                       };
                                entrada.Add(Entradas);
                            }
                        }
                    }
                }
                var response = new
                {
                    status = true,
                    data = entrada
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


        [HttpPost]
        public async Task<IActionResult> Create(Entrada E)
        {
            try
            {
                using (SqlConnection con = new(_context.Conexion))
                {
                    using (SqlCommand cmd = new("IENTRADA", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FECHA", System.Data.SqlDbType.Date).Value = E.Fecha;
                        cmd.Parameters.Add("@CODIGOSUCURSAL", System.Data.SqlDbType.BigInt).Value = E.CodigoSucursal;
                        cmd.Parameters.Add("@DESCRIPCION", System.Data.SqlDbType.VarChar).Value = E.Descripcion;
                        cmd.Parameters.Add("@CODIGOASIGNACIONPERMISOS", System.Data.SqlDbType.BigInt).Value = HttpContext.Session.GetInt32("CODIGOASIGNACIONPERMISOS");
                        con.Open();
                        cmd.ExecuteNonQuery(); // Ejecutar el comando                        
                    }
                    con.Close();


                }
                return Json(new { success = true }); ;

            }
            catch (System.Exception e)
            {
                ViewBag.Error = e.Message;
                return Json(new { success = false }); ;
            }
        }

        public async Task<IActionResult> ObtenerEntrada(int codigoEntrada)
        {
            try
            {
                Entrada entrada = null;
                using (SqlConnection con = new SqlConnection(_context.Conexion))
                {
                    using (SqlCommand cmd = new SqlCommand("SBUSCAENTRADA", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CODIGOENTRADA", System.Data.SqlDbType.Int).Value = codigoEntrada;
                        con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                entrada = new Entrada()
                                {
                                    CodigoEntrada = Convert.ToInt32(reader["CODIGOENTRADA"]),
                                    Fecha = Convert.ToDateTime(reader["FECHA"]),
                                    Sucursal = Convert.ToString(reader["SUCURSAL"]),
                                    Descripcion = Convert.ToString(reader["DESCRIPCION"])
                                };
                            }
                        }
                    }
                }

                return Json(entrada);
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message });
            }
        }





    }
}
