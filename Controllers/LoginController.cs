using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SPCOIN.Models;
using System.Data.SqlClient;
using System.Security.Claims;

namespace SPCOIN.Controllers
{
    public class LoginController : Controller
    {
        private readonly Context _context;

        public LoginController(Context context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            ClaimsPrincipal c = HttpContext.User; 
            if (c.Identity != null)
            {
                if (c.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario u)
        {
            try
            {
                using (SqlConnection con = new(_context.Conexion))
                {
                    using (SqlCommand cmd = new("SP_LOGIN", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@USUARIO", System.Data.SqlDbType.VarChar).Value = u.User;
                        cmd.Parameters.Add("@PASSWORD", System.Data.SqlDbType.VarChar).Value = u.Contraseña;
                        SqlParameter codigousuarioParam = cmd.Parameters.Add("@CODIGOUSUARIO", System.Data.SqlDbType.Int); // Agregar parámetro de salida
                        codigousuarioParam.Direction = System.Data.ParameterDirection.Output; // Indicar que es un parámetro de salida
                        con.Open();
                        cmd.ExecuteNonQuery(); // Ejecutar el comando                        
                        int codigousuario = codigousuarioParam.Value == DBNull.Value ? 0 : (int)codigousuarioParam.Value; // Validar si es nulo y asignar cero en su lugar
                        TempData["codigousuario"] = codigousuario;
                        HttpContext.Session.SetInt32("CODIGOUSUARIO", codigousuario);
                        if (codigousuario > 0)
                        {
                            List<Claim> c = new List<Claim>();
                            {
                                new Claim(ClaimTypes.NameIdentifier, u.User);
                            };
                            ClaimsIdentity ci = new(c, CookieAuthenticationDefaults.AuthenticationScheme);
                            AuthenticationProperties p = new();
                            p.AllowRefresh = true;

                            p.ExpiresUtc = DateTime.UtcNow.AddHours(1);
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci), p);
                            return RedirectToAction("Index", "Sucursal");

                        }
                        else
                        {
                            ViewBag.Error = "Verifique sus credenciales de acceso";
                        }
                    }
                    con.Close();


                }
                return View();

            }
            catch (System.Exception e)
            {
                ViewBag.Error = e.Message;
                return View();
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

    }
}
