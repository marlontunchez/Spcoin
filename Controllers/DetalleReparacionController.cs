using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace SPCOIN.Controllers
{
    public class DetalleReparacionController : Controller
    {
        private readonly Context _context;
        public DetalleReparacionController(Context context)
        {
            _context = context;
        }


        [HttpDelete]
        public async Task<IActionResult> Deletedetalle(int codigoDetalleReparacion)
        {
            try
            {
                // Validar que el código de reparación es un número válido
                if (!int.TryParse(codigoDetalleReparacion.ToString(), out _))
                {
                    return Json(new { success = false, message = "El códigodetallereparación no es válido" });
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

    }
}
