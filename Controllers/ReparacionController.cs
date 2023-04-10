using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPCOIN.Models;
using System.Data.SqlClient;

namespace SPCOIN.Controllers
{
    public class ReparacionController : Controller
    {
        private readonly Context _context;
        public ReparacionController(Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
               ;

                List<Reparacion> reparacionesrealizadas = new List<Reparacion>();
                using (SqlConnection con = new SqlConnection(_context.Conexion))
                {
                    using (SqlCommand cmd = new SqlCommand("SREPARACIONESREALIZADAS", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@BUSCAR", System.Data.SqlDbType.VarChar).Value = "";
                        cmd.Parameters.Add("@CODIGOASIGNACIONPERMISOS", System.Data.SqlDbType.BigInt).Value = Convert.ToInt32(TempData["CODIGOASIGNACIONPERMISOS"]);
                        //cmd.Parameters.Add("@CODIGOASIGNACIONPERMISOS", System.Data.SqlDbType.BigInt).Value = Convert.ToInt32(HttpContext.Items["CODIGOASIGNACIONPERMISOS"]);
                        con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Reparacion reparacion = new Reparacion()
                                {
                                    CodigoReparacion = Convert.ToInt32(reader["CODIGOREPARACION"]),
                                    Fecha = Convert.ToDateTime(reader["FECHA"]),
                                    Nit  = Convert.ToString(reader["NIT"]),
                                    Nombre = Convert.ToString(reader["NOMBRE"]),
                                    Motocicleta = Convert.ToString(reader["MOTOCICLETA"]),
                                    Mecanico = Convert.ToString(reader["MECANICO"]),
                                    ConvertidoAVenta = Convert.ToBoolean(reader["CONVERTIDOAVENTA"])
                                };
                                reparacionesrealizadas.Add(reparacion);
                            }
                        }
                    }
                }

                return View(reparacionesrealizadas);
            }
            catch (Exception e)
        {
                ViewBag.Error = e.Message;
            return View();
        }
        }


        [AllowAnonymous]
                public async Task<IActionResult> Create(Reparacion u)
        {
            try
            {
                using (SqlConnection con = new(_context.Conexion))
                {
                    using (SqlCommand cmd = new("IREPARACION", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CODIGOCLIENTE", System.Data.SqlDbType.BigInt).Value = 1;
                        cmd.Parameters.Add("@CODIGOASIGNACIONPERMISOS", System.Data.SqlDbType.BigInt).Value = 1;

                        cmd.Parameters.Add("@MOTOCICLETA", System.Data.SqlDbType.VarChar).Value = u.Motocicleta ;
                        cmd.Parameters.Add("@MECANICO", System.Data.SqlDbType.VarChar).Value = u.Mecanico;
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





    }
}