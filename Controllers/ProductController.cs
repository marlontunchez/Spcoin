using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SPCOIN.Models;
using System.Data.SqlClient;

namespace SPCOIN.Controllers
{
    public class ProductController : Controller
    {

        private readonly Context _context;
        public ProductController(Context context)
        {
            _context = context;
        }
        // GET: ProductController
        public async Task<IActionResult> ObtenerProducto(string codigo)
        {
            try
            {
                Producto producto = null;
                using (SqlConnection con = new SqlConnection(_context.Conexion))
                {
                    using (SqlCommand cmd = new SqlCommand("SBUSCAPRODUCTO", con))
                    {
                      
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CODIGOPRODUCTO", System.Data.SqlDbType.VarChar).Value = codigo;
                        cmd.Parameters.Add("@CODIGOASIGNACIONPERMISOS", System.Data.SqlDbType.BigInt).Value = HttpContext.Session.GetInt32("CODIGOASIGNACIONPERMISOS") ?? 0;
                        con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                producto = new Producto()
                                {
                                    CodigoProducto = Convert.ToString(reader["CODIGOPRODUCTO"]),
                                    Nombre = Convert.ToString(reader["NOMBRE"]),
                                    Unidades = Convert.ToInt32(reader["UNIDADES"]),
                                    Precio = Convert.ToDouble(reader["PRECIO"]),
                                    Existencia = Convert.ToInt32(reader["EXISTENCIA"]),
                                    VenderSinExistencia = Convert.ToBoolean(reader["VENDERSINEXISTENCIA"])
                                };
                            }
                        }
                    }
                }

                return Json(producto);
            }
            catch (Exception e)
            {
                return Json(new { error = e.Message });
            }
        }



        public async Task<JsonResult> ConsultaProducto(string busqueda)
        {
            try
            {
                List<Producto> productos = new List<Producto>();
                using (SqlConnection con = new SqlConnection(_context.Conexion))
                {
                    using (SqlCommand cmd = new SqlCommand("SConsultaProductos", con))
                    {
                        Console.WriteLine(busqueda);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@BUSCAR", System.Data.SqlDbType.VarChar).Value = busqueda;
                        cmd.Parameters.Add("@CODIGOSUCURSAL", System.Data.SqlDbType.BigInt).Value = 1;
                        cmd.Parameters.Add("@COLUMNA", System.Data.SqlDbType.VarChar).Value = "P.NOMBRE";
                        cmd.Parameters.Add("@FILTRO", System.Data.SqlDbType.VarChar).Value = "";
                        cmd.Parameters.Add("@DESCRIPCION", System.Data.SqlDbType.VarChar).Value = "";

                        con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                Producto producto = new Producto()
                                {
                                    CodigoProducto = Convert.ToString(reader["CODIGO"]),
                                    Descripcion = Convert.ToString(reader["DESCRIPCION"]),
                                    Existencia = Convert.ToInt32(reader["EXISTENCIA"]),
                                    Nombre = Convert.ToString(reader["NOMBRE"]),
                                    Precio = Convert.ToDouble(reader["PRECIO1"])
                                 
                                };
                                productos.Add(producto);
                            }
                        }
                    }
                }
                var response = new
                {
                    status = true,
                    data = productos
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




