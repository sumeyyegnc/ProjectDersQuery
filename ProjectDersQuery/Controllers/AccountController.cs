using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ProjectDersQuery.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectDersQuery.Controllers
{
    public class AccountController : Controller
    {
        private readonly string _connectionString;

        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
            EnsureKullaniciTableExists();
        }

        private void EnsureKullaniciTableExists()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Kullanicilar' and xtype='U')
                    CREATE TABLE Kullanicilar (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Username NVARCHAR(50) NOT NULL UNIQUE,
                        Password NVARCHAR(50) NOT NULL
                    )";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Kullanici model)
        {
            if (ModelState.IsValid)
            {
                bool isValidUser = false;
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = "SELECT COUNT(1) FROM Kullanicilar WHERE Username = @username AND Password = @password";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", model.Username);
                        command.Parameters.AddWithValue("@password", model.Password); // Note: Simple plain text for demo purposes

                        connection.Open();
                        var result = (int)command.ExecuteScalar();
                        isValidUser = result > 0;
                    }
                }

                if (isValidUser)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username)
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Kullanici model)
        {
            if (ModelState.IsValid)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var checkQuery = "SELECT COUNT(1) FROM Kullanicilar WHERE Username = @username";
                    using (var checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@username", model.Username);
                        connection.Open();
                        var count = (int)checkCommand.ExecuteScalar();
                        if (count > 0)
                        {
                            ModelState.AddModelError("Username", "Bu kullanıcı adı zaten alınmış.");
                            return View(model);
                        }
                    }

                    var insertQuery = "INSERT INTO Kullanicilar (Username, Password) VALUES (@username, @password)";
                    using (var insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@username", model.Username);
                        insertCommand.Parameters.AddWithValue("@password", model.Password);
                        insertCommand.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
