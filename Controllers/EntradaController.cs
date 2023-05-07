using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SPCOIN.Controllers
{
    public class EntradaController : Controller
    {
        // GET: EntradaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: EntradaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EntradaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EntradaController/Create
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

        // GET: EntradaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EntradaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: EntradaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EntradaController/Delete/5
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
