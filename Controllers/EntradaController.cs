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



    }
}
