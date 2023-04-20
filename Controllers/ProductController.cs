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


        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
      
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
