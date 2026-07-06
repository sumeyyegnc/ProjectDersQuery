using Microsoft.AspNetCore.Mvc;
using ProjectDersQuery.Data;
using ProjectDersQuery.Models;

namespace ProjectDersQuery.Controllers
{
    public class KursController : Controller
    {
        private readonly EgitimDbContext context;
        public KursController(EgitimDbContext context)
        {
            this.context = context;
        }

        // GET: Kurs
        public IActionResult Index()
        {
            return View();
        }

        // GET: Kurs/GetAll
        [HttpGet]
        public JsonResult GetAll()
        {   
            var liste = context.Kurslar.ToList();
            return Json(liste);
        }

        // GET: Kurs/GetById?id=5
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var kayit = context.Kurslar.Find(id);
            return Json(kayit);     
        }   
            
        // POST: Kurs/Create
        [HttpPost]  
        public JsonResult Create(Kurs kurs)
        {
            if (!ModelState.IsValid)
            {
                var hatalar = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, message = string.Join(" ", hatalar) });
            }

            context.Kurslar.Add(kurs);
            context.SaveChanges();

            return Json(new { success = true, message = "Kurs başarıyla eklendi." });
        }

        // POST: Kurs/Update
        [HttpPost]
        public JsonResult Update(Kurs kurs)
        {
            if (!ModelState.IsValid)
            {
                var hatalar = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, message = string.Join(" ", hatalar) });
            }

            var mevcut = context.Kurslar.Find(kurs.KursId);
            if (mevcut == null)
            {
                return Json(new { success = false, message = "Kurs bulunamadı." });
            }

            mevcut.KursAdi = kurs.KursAdi;
            mevcut.Ucret = kurs.Ucret;
            context.SaveChanges();

            return Json(new { success = true, message = "Kurs başarıyla güncellendi." });
        }

        // POST: Kurs/Delete
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var kayit = context.Kurslar.Find(id);
            if (kayit == null)
            {
                return Json(new { success = false, message = "Kurs bulunamadı." });
            }

            context.Kurslar.Remove(kayit);
            context.SaveChanges();

            return Json(new { success = true, message = "Kurs başarıyla silindi." });
        }
    }
}
