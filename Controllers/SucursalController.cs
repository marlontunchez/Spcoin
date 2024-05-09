using Microsoft.AspNetCore.Mvc;
using SPCOIN.Models;
using System.Data.SqlClient;

namespace SPCOIN.Controllers
{
    public class SucursalController : Controller
    {
        private readonly Context _context;

        public SucursalController(Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                int codigoUsuario = Convert.ToInt32(TempData["codigousuario"]);

                List<Sucursal> sucursalesPermitidas = new List<Sucursal>();
                using (SqlConnection con = new SqlConnection(_context.Conexion))
                {
                    using (SqlCommand cmd = new SqlCommand("SUCURSALESPERMITIDASALUSUARIO", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CODIGOUSUARIO", System.Data.SqlDbType.Int).Value = HttpContext.Session.GetInt32("CODIGOUSUARIO");
                        con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Sucursal sucursal = new Sucursal()
                                {
                                    CodigoAsignacionPermisos = Convert.ToInt32(reader["CODIGOASIGNACIONPERMISOS"]),
                                    CodigoSucursal = Convert.ToInt32(reader["CODIGOSUCURSAL"]),
                                    Nombre = Convert.ToString(reader["Nombre"]),
                                    Direccion = Convert.ToString(reader["Direccion"])
                                };
                                sucursalesPermitidas.Add(sucursal);
                            }
                        }
                    }
                }

                return View(sucursalesPermitidas);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }

        public async Task<IActionResult> obtenerSucursales()
        {
            try
            {
                List<Sucursal> sucursalesPermitidas = new List<Sucursal>();
                using (SqlConnection con = new SqlConnection(_context.Conexion))
                {
                    using (SqlCommand cmd = new SqlCommand("SSSUCURSAL", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CODIGOASIGNACIONPERMISOS", System.Data.SqlDbType.Int).Value = HttpContext.Session.GetInt32("CODIGOASIGNACIONPERMISOS");
                        con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Sucursal sucursal = new Sucursal()
                                {
                                    CodigoSucursal = Convert.ToInt32(reader["CODIGOSUCURSAL"]),
                                    Nombre = Convert.ToString(reader["NOMBRE"]),
                                 };
                                sucursalesPermitidas.Add(sucursal);
                            }
                        }
                    }
                }

                return Json(sucursalesPermitidas);
               
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return Json(new { error = e.Message });
            }
        }

    }
}  