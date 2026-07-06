using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProjectDersQuery.Models;
using System.Collections.Generic;

namespace ProjectDersQuery.Controllers
{
    [Authorize]
    public class KursController : Controller
    {
        private readonly string _connectionString;

        public KursController(IConfiguration configuration)
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
            var liste = new List<Kurs>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Kurslar";
                if (!string.IsNullOrEmpty(search))
                {
                    query += " WHERE KursAdi LIKE @search";
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
                            liste.Add(new Kurs
                            {
                                KursId = Convert.ToInt32(reader["KursId"]),
                                KursAdi = reader["KursAdi"].ToString(),
                                Ucret = Convert.ToInt32(reader["Ucret"])
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
            Kurs kayit = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Kurslar WHERE KursId = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            kayit = new Kurs
                            {
                                KursId = Convert.ToInt32(reader["KursId"]),
                                KursAdi = reader["KursAdi"].ToString(),
                                Ucret = Convert.ToInt32(reader["Ucret"])
                            };
                        }
                    }
                }
            }
            return Json(kayit);
        }

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

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Kurslar (KursAdi, Ucret) VALUES (@KursAdi, @Ucret)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KursAdi", kurs.KursAdi);
                    command.Parameters.AddWithValue("@Ucret", kurs.Ucret);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return Json(new { success = true, message = "Kurs başarıyla eklendi." });
        }

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

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE Kurslar SET KursAdi = @KursAdi, Ucret = @Ucret WHERE KursId = @KursId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@KursAdi", kurs.KursAdi);
                    command.Parameters.AddWithValue("@Ucret", kurs.Ucret);
                    command.Parameters.AddWithValue("@KursId", kurs.KursId);
                    connection.Open();
                    int affected = command.ExecuteNonQuery();

                    if (affected == 0)
                    {
                        return Json(new { success = false, message = "Kurs bulunamadı." });
                    }
                }
            }

            return Json(new { success = true, message = "Kurs başarıyla güncellendi." });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Kurslar WHERE KursId = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    int affected = command.ExecuteNonQuery();

                    if (affected == 0)
                    {
                        return Json(new { success = false, message = "Kurs bulunamadı." });
                    }
                }
            }

            return Json(new { success = true, message = "Kurs başarıyla silindi." });
        }
    }
}
