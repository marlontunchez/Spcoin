using Microsoft.AspNetCore.Mvc;

namespace SPCOIN.Controllers
{
    public class ClienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
