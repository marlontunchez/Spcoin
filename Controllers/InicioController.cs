using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace SPCOIN.Controllers
{
    [Authorize]
    public class InicioController : Controller
    {
        private readonly Context _context;
        public InicioController(Context context)
        {
            _context = context;
        }
        // GET: InicioController
        public ActionResult Index()
        {
            return View();
        }
        public class VentasPorMes
        {
            public string labels { get; set; }
            public int data { get; set; }
        }

        public async Task<IActionResult> Ventaspormes()
        {
            try
            {
                var labels = new List<string>();
                var data = new List<int>();
                using (SqlConnection con = new SqlConnection(_context.Conexion))
                {
                    using (SqlCommand cmd = new SqlCommand("RVENTASPORMES", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                labels.Add(Convert.ToString(reader["MES"]));
                                data.Add(Convert.ToInt32(reader["VENTAS"]));
                            }
                        }
                    }
                }

                return Json(new { labels, data });
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return Json(new { Error = e.Message });
            }
        }



        public async Task<IActionResult> Ventasvendedores()
        {
            try
            {
                var labels = new List<string>();
                var data = new List<int>();
                using (SqlConnection con = new SqlConnection(_context.Conexion))
                {
                    using (SqlCommand cmd = new SqlCommand("RVENDEDORESPORMES", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        con.Open();
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (reader.Read())
                            {
                                labels.Add(Convert.ToString(reader["VENDEDOR"]));
                                data.Add(Convert.ToInt32(reader["VENTAS"]));
                            }
                        }
                    }
                }

                return Json(new { labels, data });
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                return Json(new { Error = e.Message });
            }
        }

    }
}
