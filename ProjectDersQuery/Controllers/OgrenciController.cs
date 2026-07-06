using Microsoft.AspNetCore.Mvc;
using ProjectDersQuery.Data;
using ProjectDersQuery.Models;

namespace ProjectDersQuery.Controllers
{
    public class OgrenciController : Controller
    {
        private readonly EgitimDbContext context;
        public OgrenciController(EgitimDbContext context)
        {
            this.context = context;
        }
        // GET: Ogrenci
        // GET: Ogrenci
        public IActionResult Index()
        {
            return View();
        }

        // GET: Ogrenci/GetAll
        [HttpGet]
        public JsonResult GetAll()
        {
            var liste = context.Ogrenciler.ToList();
            return Json(liste);
        }

        // GET: Ogrenci/GetById?id=5
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var kayit = context.Ogrenciler.Find(id);
            return Json(kayit);
        }

        // POST: Ogrenci/Create
        [HttpPost]
        public JsonResult Create(Ogrenci ogrenci)
        {
            if (!ModelState.IsValid)
            {
                var hatalar = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, message = string.Join(" ", hatalar) });
            }

            context.Ogrenciler.Add(ogrenci);
            context.SaveChanges();

            return Json(new { success = true, message = "Öğrenci başarıyla eklendi." });
        }

        // POST: Ogrenci/Update
        [HttpPost]
        public JsonResult Update(Ogrenci ogrenci)
        {
            if (!ModelState.IsValid)
            {
                var hatalar = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, message = string.Join(" ", hatalar) });
            }

            var mevcut = context.Ogrenciler.Find(ogrenci.OgrenciId);
            if (mevcut == null)
            {
                return Json(new { success = false, message = "Öğrenci bulunamadı." });
            }

            mevcut.AdSoyad = ogrenci.AdSoyad;
            mevcut.Email = ogrenci.Email;
            context.SaveChanges();

            return Json(new { success = true, message = "Öğrenci başarıyla güncellendi." });
        }

        // POST: Ogrenci/Delete
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var kayit = context.Ogrenciler.Find(id);
            if (kayit == null)
            {
                return Json(new { success = false, message = "Öğrenci bulunamadı." });
            }

            context.Ogrenciler.Remove(kayit);
                context.SaveChanges();

            return Json(new { success = true, message = "Öğrenci başarıyla silindi." });
        }
    }
}
