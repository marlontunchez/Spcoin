using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace SPCOIN.Controllers
{
    public class PermisosController : Controller
    {
        private readonly Context _context;
        public PermisosController(Context context)
        {
            _context = context;
        }
        public ActionResult Permisos(int codigoasignacionpermisos)
        {
            HttpContext.Session.SetInt32("CODIGOASIGNACIONPERMISOS", codigoasignacionpermisos);
             //HttpContext.Session.SetInt32("CODIGOASIGNACIONPERMISOS", codigoasignacionpermisos);
            //HttpContext.Items["CodigoA"] = codigoasignacionpermisos;

            TempData["CODIGOASIGNACIONPERMISOS"] = codigoasignacionpermisos;        
            return RedirectToAction("index", "home");
            // Código adicional
        }
    }
}
