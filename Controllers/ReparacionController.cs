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
        public IActionResult Index()
        {
            return View();
        }



    }
}