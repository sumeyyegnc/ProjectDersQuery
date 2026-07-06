using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProjectDersQuery.Models;
using System.Collections.Generic;

namespace ProjectDersQuery.Controllers
{
    [Authorize]
    public class OgrenciController : Controller
    {
        private readonly string _connectionString;

        public OgrenciController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAll(string search = null)
        {
            var liste = new List<Ogrenci>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Ogrenciler";
                if (!string.IsNullOrEmpty(search))
                {
                    query += " WHERE AdSoyad LIKE @search OR Email LIKE @search";
                }

                using (var command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(search))
                    {
                        command.Parameters.AddWithValue("@search", "%" + search + "%");
                    }

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            liste.Add(new Ogrenci
                            {
                                OgrenciId = Convert.ToInt32(reader["OgrenciId"]),
                                AdSoyad = reader["AdSoyad"].ToString(),
                                Email = reader["Email"].ToString()
                            });
                        }
                    }
                }
            }
            return Json(liste);
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            Ogrenci kayit = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Ogrenciler WHERE OgrenciId = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            kayit = new Ogrenci
                            {
                                OgrenciId = Convert.ToInt32(reader["OgrenciId"]),
                                AdSoyad = reader["AdSoyad"].ToString(),
                                Email = reader["Email"].ToString()
                            };
                        }
                    }
                }
            }
            return Json(kayit);
        }

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

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Ogrenciler (AdSoyad, Email) VALUES (@AdSoyad, @Email)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AdSoyad", ogrenci.AdSoyad);
                    command.Parameters.AddWithValue("@Email", ogrenci.Email);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return Json(new { success = true, message = "Öğrenci başarıyla eklendi." });
        }

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

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE Ogrenciler SET AdSoyad = @AdSoyad, Email = @Email WHERE OgrenciId = @OgrenciId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AdSoyad", ogrenci.AdSoyad);
                    command.Parameters.AddWithValue("@Email", ogrenci.Email);
                    command.Parameters.AddWithValue("@OgrenciId", ogrenci.OgrenciId);
                    connection.Open();
                    int affected = command.ExecuteNonQuery();

                    if (affected == 0)
                    {
                        return Json(new { success = false, message = "Öğrenci bulunamadı." });
                    }
                }
            }

            return Json(new { success = true, message = "Öğrenci başarıyla güncellendi." });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Ogrenciler WHERE OgrenciId = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    int affected = command.ExecuteNonQuery();

                    if (affected == 0)
                    {
                        return Json(new { success = false, message = "Öğrenci bulunamadı." });
                    }
                }
            }

            return Json(new { success = true, message = "Öğrenci başarıyla silindi." });
        }
    }
}
