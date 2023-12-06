using Microsoft.AspNetCore.Mvc;

namespace SPCOIN.Controllers
{
    public class DocumentoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
