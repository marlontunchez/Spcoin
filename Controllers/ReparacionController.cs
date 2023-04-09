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


      
        public IActionResult Create()
        {
            //try
            //{
            //    using (SqlConnection con = new(_context.Conexion))
            //    {
            //        using (SqlCommand cmd = new("SP_LOGIN", con))
            //        {
            //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //            cmd.Parameters.Add("@USUARIO", System.Data.SqlDbType.VarChar).Value = u.User;
            //            cmd.Parameters.Add("@PASSWORD", System.Data.SqlDbType.VarChar).Value = u.Contraseña;
            //            SqlParameter codigousuarioParam = cmd.Parameters.Add("@CODIGOUSUARIO", System.Data.SqlDbType.Int); // Agregar parámetro de salida
            //            codigousuarioParam.Direction = System.Data.ParameterDirection.Output; // Indicar que es un parámetro de salida
            //            con.Open();
            //            cmd.ExecuteNonQuery(); // Ejecutar el comando                        
            //            int codigousuario = codigousuarioParam.Value == DBNull.Value ? 0 : (int)codigousuarioParam.Value; // Validar si es nulo y asignar cero en su lugar
            //            TempData["codigousuario"] = codigousuario;
            //            if (Convert.ToUInt32(TempData["codigousuario"].ToString()) > 0)
            //            {
            //                List<Claim> c = new List<Claim>();
            //                {
            //                    new Claim(ClaimTypes.NameIdentifier, u.User);
            //                };
            //                ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
            //                AuthenticationProperties p = new();
            //                p.AllowRefresh = true;

            //                p.ExpiresUtc = DateTime.UtcNow.AddHours(1);
            //                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);
            //                return RedirectToAction("Index", "Sucursal");

            //            }
            //            else
            //            {
            //                ViewBag.Error = "Verifique sus credenciales de acceso";
            //            }
            //        }
            //        con.Close();


            //    }
            //    return View();

            //}
            //catch (System.Exception e)
            //{
            //    ViewBag.Error = e.Message;
            //    return View();
            //}
            return View();
        }
       




    }
}