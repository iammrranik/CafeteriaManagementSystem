using App.AuthFilters;
using BLL.DTOs;
using DAL.EF;
using DAL.EF.Tables;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace App.Controllers
{
    public class AuthController : Controller
    {
        CafeteriaDbContext db;

        public AuthController(CafeteriaDbContext db)
        {
            this.db = db;
        }

        [Logged]
        public IActionResult Dashboard()
        {
            ViewBag.Uname = HttpContext.Session.GetString("Uname");
            ViewBag.UserTypeId = HttpContext.Session.GetInt32("UType");
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string IdCardNo, string Pass)
        {
            var u = (from user in db.Users
                     where user.IdCardNo.Equals(IdCardNo) &&
                     user.Password.Equals(GetMd5(Pass))
                     select user).SingleOrDefault();
            if (u != null)
            {
                HttpContext.Session.SetString("Uname", u.IdCardNo);
                HttpContext.Session.SetInt32("UType", u.UserTypeId);
                HttpContext.Session.SetInt32("UserId", u.Id);
                if (u.UserTypeId == 1)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else if (u.UserTypeId == 2)
                {
                    return RedirectToAction("Index", "Customer");
                }
                return RedirectToAction("Dashboard");
            }
            TempData["Class"] = "danger";
            TempData["Msg"] = "Invalid Username and Password";
            return View();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View(new UserDTO() { });
        }

        [HttpPost]
        public IActionResult Registration(UserDTO obj)
        {
            ModelState.Remove("UserTypeId");
            if (ModelState.IsValid)
            {
                try
                {
                    // Check duplicate email manually
                    if (db.Users.Any(u => u.Email == obj.Email))
                    {
                        ModelState.AddModelError("Email", "Email address is already registered.");
                        return View(obj);
                    }

                    // Check duplicate ID Card manually
                    if (db.Users.Any(u => u.IdCardNo == obj.IdCardNo))
                    {
                        ModelState.AddModelError("IdCardNo", "ID Card Number is already registered.");
                        return View(obj);
                    }

                    var user = new User()
                    {
                        Name = obj.Name,
                        IdCardNo = obj.IdCardNo,
                        Email = obj.Email,
                        Password = GetMd5(obj.Password),
                        UserTypeId = 2,
                        WalletBalance = 10, // Initial wallet bonus balance for new users
                        ProfileStatus = "New Profile"
                    };

                    db.Users.Add(user);
                    db.SaveChanges();

                    TempData["Class"] = "success";
                    TempData["Msg"] = "Registration Successful";
                    return RedirectToAction("Login");
                }
                catch (System.Exception ex)
                {
                    TempData["Class"] = "danger";
                    TempData["Msg"] = "Database error: " + (ex.InnerException?.Message ?? ex.Message);
                }
            }
            else
            {
                var errors = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                TempData["Class"] = "danger";
                TempData["Msg"] = "Validation errors: " + errors;
            }
            return View(obj);
        }

        private static string GetMd5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
