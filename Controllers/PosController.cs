using Microsoft.AspNetCore.Mvc;

namespace SPCOIN.Controllers
{
    public class PosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
