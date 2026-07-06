using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProjectDersQuery.Models;
using System.Collections.Generic;

namespace ProjectDersQuery.Controllers
{
    [Authorize]
    public class EgitmenController : Controller
    {
        private readonly string _connectionString;

        public EgitmenController(IConfiguration configuration)
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
            var liste = new List<Egitmen>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Egitmenler";
                if (!string.IsNullOrEmpty(search))
                {
                    query += " WHERE EgitmenAdi LIKE @search OR Brans LIKE @search";
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
                            liste.Add(new Egitmen
                            {
                                EgitmenId = Convert.ToInt32(reader["EgitmenId"]),
                                EgitmenAdi = reader["EgitmenAdi"].ToString(),
                                Brans = reader["Brans"].ToString()
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
            Egitmen kayit = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Egitmenler WHERE EgitmenId = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            kayit = new Egitmen
                            {
                                EgitmenId = Convert.ToInt32(reader["EgitmenId"]),
                                EgitmenAdi = reader["EgitmenAdi"].ToString(),
                                Brans = reader["Brans"].ToString()
                            };
                        }
                    }
                }
            }
            return Json(kayit);
        }

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

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Egitmenler (EgitmenAdi, Brans) VALUES (@EgitmenAdi, @Brans)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EgitmenAdi", egitmen.EgitmenAdi);
                    command.Parameters.AddWithValue("@Brans", egitmen.Brans);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return Json(new { success = true, message = "Eğitmen başarıyla eklendi." });
        }

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

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE Egitmenler SET EgitmenAdi = @EgitmenAdi, Brans = @Brans WHERE EgitmenId = @EgitmenId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EgitmenAdi", egitmen.EgitmenAdi);
                    command.Parameters.AddWithValue("@Brans", egitmen.Brans);
                    command.Parameters.AddWithValue("@EgitmenId", egitmen.EgitmenId);
                    connection.Open();
                    int affected = command.ExecuteNonQuery();

                    if (affected == 0)
                    {
                        return Json(new { success = false, message = "Eğitmen bulunamadı." });
                    }
                }
            }

            return Json(new { success = true, message = "Eğitmen başarıyla güncellendi." });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Egitmenler WHERE EgitmenId = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    int affected = command.ExecuteNonQuery();

                    if (affected == 0)
                    {
                        return Json(new { success = false, message = "Eğitmen bulunamadı." });
                    }
                }
            }

            return Json(new { success = true, message = "Eğitmen başarıyla silindi." });
        }
    }
}
