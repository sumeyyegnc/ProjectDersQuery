using Microsoft.AspNetCore.Mvc;
using ProjectDersQuery.Data;
using ProjectDersQuery.Models;
using System.Linq;



namespace ProjectDersQuery.Controllers
{
    public class EgitmenController : Controller
    {
        private readonly EgitimDbContext context;
        public EgitmenController(EgitimDbContext context)
        {
            this.context = context;
        }
        // GET: Egitmen
        // GET: Egitmen
        public IActionResult Index()
        {
            return View();
        }

        // GET: Egitmen/GetAll
        [HttpGet]
        public JsonResult GetAll()
        {
            var liste = context.Egitmenler.ToList();
            return Json(liste);
        }

        // GET: Egitmen/GetById?id=5
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var kayit = context.Egitmenler.Find(id);
            return Json(kayit);
        }

        // POST: Egitmen/Create
        [HttpPost]
        public JsonResult Create(Egitmen egitmen)
        {
            if (!ModelState.IsValid)
            {
                var hatalar = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, message = string.Join(" ", hatalar) });
            }

            context.Egitmenler.Add(egitmen);
            context.SaveChanges();

            return Json(new { success = true, message = "Eğitmen başarıyla eklendi." });
        }

        // POST: Egitmen/Update
        [HttpPost]
        public JsonResult Update(Egitmen egitmen)
        {
            if (!ModelState.IsValid)
            {
                var hatalar = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, message = string.Join(" ", hatalar) });
            }

            var mevcut = context.Egitmenler.Find(egitmen.EgitmenId);
            if (mevcut == null)
            {
                return Json(new { success = false, message = "Eğitmen bulunamadı." });
            }

            mevcut.EgitmenAdi = egitmen.EgitmenAdi;
            mevcut.Brans = egitmen.Brans;
            context.SaveChanges();

            return Json(new { success = true, message = "Eğitmen başarıyla güncellendi." });
        }

        // POST: Egitmen/Delete
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var kayit = context.Egitmenler.Find(id);
            if (kayit == null)  
            {
                return Json(new { success = false, message = "Eğitmen bulunamadı." });
            }

            context.Egitmenler.Remove(kayit);
            context.SaveChanges();

            return Json(new { success = true, message = "Eğitmen başarıyla silindi." });
        }
    }
}
