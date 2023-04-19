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
                        cmd.Parameters.Add("@CODIGOASIGNACIONPERMISOS", System.Data.SqlDbType.BigInt).Value = HttpContext.Session.GetInt32("CODIGOASIGNACIONPERMISOS");
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


        [HttpPost]
                public async Task<IActionResult> Create(Reparacion r)
        {
            try
            {
                using (SqlConnection con = new(_context.Conexion))
                {
                    using (SqlCommand cmd = new("IREPARACION", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CODIGOCLIENTE", System.Data.SqlDbType.BigInt).Value = 1;
                        cmd.Parameters.Add("@CODIGOASIGNACIONPERMISOS", System.Data.SqlDbType.BigInt).Value = HttpContext.Session.GetInt32("CODIGOASIGNACIONPERMISOS");

                        cmd.Parameters.Add("@MOTOCICLETA", System.Data.SqlDbType.VarChar).Value = r.Motocicleta ;
                        cmd.Parameters.Add("@MECANICO", System.Data.SqlDbType.VarChar).Value = r.Mecanico;
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




        [HttpPost]
        public async Task<IActionResult> Convertir(Reparacion r)
        {
            try
            {
                using (SqlConnection con = new(_context.Conexion))
                {
                    using (SqlCommand cmd = new("FCONVIERTEREPARACIONAVENTA", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CODIGOREPARACION", System.Data.SqlDbType.BigInt).Value = r.CodigoReparacion;
                       con.Open();
                        cmd.ExecuteNonQuery(); // Ejecutar el comando
                                               // 
                        Console.WriteLine(r.CodigoReparacion);
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



        [HttpDelete]
        public async Task<IActionResult> Delete(int codigoReparacion)
        {
            try
            {
                // Validar que el código de reparación es un número válido
                if (!int.TryParse(codigoReparacion.ToString(), out _))
                {
                    return Json(new { success = false, message = "El código de reparación no es válido" });
                }

                using (SqlConnection con = new(_context.Conexion))
                {
                    using (SqlCommand cmd = new("DREPARACION", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CODIGOREPARACION", System.Data.SqlDbType.BigInt).Value = codigoReparacion;
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
                return Json(new { success = false, message = e.Message });
            }
        }

        public async Task<JsonResult> Detalle(int codigoReparacion)
        {
            try
            {
                List<DetalleReparacion> detallesReparacion = new List<DetalleReparacion>();
                using (SqlConnection con = new SqlConnection(_context.Conexion))
                {
                    using (SqlCommand cmd = new SqlCommand("SDETALLEREPARACION", con))
                    {
                        Console.WriteLine(codigoReparacion);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CODIGOVENTA", System.Data.SqlDbType.Int).Value = codigoReparacion;
                        con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                DetalleReparacion detalleReparacion = new DetalleReparacion()
                                {
                                    CodigoDetalleReparacion = Convert.ToInt32(reader["CODIGODETALLEREPARACION"]),
                                    CodigoProducto = Convert.ToString(reader["CODIGO"]),
                                    Nombre = Convert.ToString(reader["NOMBRE"]),
                                    Unidades = Convert.ToInt32(reader["UNIDADES"]),
                                    Precio = Convert.ToDecimal(reader["PRECIO"]),
                                    Total = Convert.ToDecimal(reader["TOTAL"]),
                                };
                                detallesReparacion.Add(detalleReparacion);
                            }
                        }
                    }
                }
                var response = new
                {
                    status = true,
                    data = detallesReparacion
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
        public async Task<IActionResult> Createdetail(DetalleReparacion D)
        {
            try
            {
                using (SqlConnection con = new(_context.Conexion))
                {
                    using (SqlCommand cmd = new("IDETALLEREPARACION", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CODIGOREPARACION", System.Data.SqlDbType.BigInt).Value = D.CodigoReparacion;
                        cmd.Parameters.Add("@CODIGOPRODUCTO", System.Data.SqlDbType.VarChar).Value = D.CodigoProducto;

                        cmd.Parameters.Add("@UNIDADES", System.Data.SqlDbType.Real).Value = D.Unidades;
                        cmd.Parameters.Add("@PRECIO", System.Data.SqlDbType.Real).Value = D.Precio;
                        cmd.Parameters.Add("@TOTAL", System.Data.SqlDbType.Real).Value = D.Precio*D.Unidades;
                        cmd.Parameters.Add("@CODIGOASIGNACIONPERMISOS", System.Data.SqlDbType.BigInt).Value = D.CodigoAsignacionPermisos;
                        cmd.Parameters.Add("@DESCUENTO", System.Data.SqlDbType.Real).Value = 0;
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



        [HttpDelete]
        public async Task<IActionResult> DeleteDetail(int codigoDetalleReparacion)
        {
            try
            {
                // Validar que el código de reparación es un número válido
                if (!int.TryParse(codigoDetalleReparacion.ToString(), out _))
                {
                    return Json(new { success = false, message = "El código de detalle reparación no es válido" });
                }

                using (SqlConnection con = new(_context.Conexion))
                {
                    using (SqlCommand cmd = new("DDETALLEREPARACION", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CODIGODETALLEREPARACION", System.Data.SqlDbType.BigInt).Value = codigoDetalleReparacion;
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
                return Json(new { success = false, message = e.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> insertDetail(DetalleReparacion dr)
        {
            try
            {
                using (SqlConnection con = new(_context.Conexion))
                {
                    using (SqlCommand cmd = new("IDETALLEREPARACION", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CODIGOREPARACION", System.Data.SqlDbType.BigInt).Value = dr.CodigoReparacion;
                        cmd.Parameters.Add("@CODIGOPRODUCTO", System.Data.SqlDbType.VarChar).Value = dr.CodigoProducto;
                        cmd.Parameters.Add("@UNIDADES", System.Data.SqlDbType.BigInt).Value =dr.Unidades;
                        cmd.Parameters.Add("@PRECIO", System.Data.SqlDbType.Real).Value = dr.Precio;
                        cmd.Parameters.Add("@TOTAL", System.Data.SqlDbType.Real).Value = dr.Unidades*dr.Precio;
                        cmd.Parameters.Add("@CODIGOASIGNACIONPERMISOS", System.Data.SqlDbType.BigInt).Value = HttpContext.Session.GetInt32("CODIGOASIGNACIONPERMISOS");
                        cmd.Parameters.Add("@DESCUENTO", System.Data.SqlDbType.Real).Value = 0;

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