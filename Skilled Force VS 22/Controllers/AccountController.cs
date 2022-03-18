using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Skilled_Force_VS_22.Manager;
using Skilled_Force_VS_22.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Skilled_Force_VS_22.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SkilledForceDB skilledForceDB;

        public AccountController(ILogger<AccountController> logger, SkilledForceDB skilledForceDB)
        {
            _logger = logger;
            this.skilledForceDB = skilledForceDB;
        }

        [HttpGet]
        public IActionResult LoginForm()
        {
            return View("LoginForm");
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            User exisitngUser = getUserIfExists(login.Email, login.Password);
            if (exisitngUser != null)
            {
                UpdateUserData(exisitngUser);
                ViewData["success"] = true;
                return RedirectToAction("Index", "Home");
            }
            ViewData["success"] = false;
            return LoginForm();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (TempData.Peek("UserId") != null)
                TempData.Clear();
            return LoginForm();
        }

        [HttpGet]
        public IActionResult Register()
        {
            LoadMetaData();
            ViewBag.edit = false;
            return View("RegistrationForm");
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            ModelState.Remove("Jobs");
            ModelState.Remove("Role");
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            {
                if (TempData.ContainsKey("UserId"))
                {
                    user.UserId = TempData.Peek("UserId").ToString();
                    skilledForceDB.User.Update(user);
                    skilledForceDB.SaveChanges();
                    UpdateUserData(user);
                    ViewBag.success = true;
                    return RedirectToAction("Index", "Home");
                }
                User exisitngUser = getUserIfExists(user.Email, user.Password);
                if (exisitngUser == null)
                {
                    skilledForceDB.User.Add(user);
                    skilledForceDB.SaveChanges();
                    TempData["SuccessMessage"] = "Saved User Successfully";
                }
                else
                {
                    TempData["Error"] = "User Email exists";
                    return Register();
                }
            }
            else
            {
                return Register();
            }
            return LoginForm();
        }

        private User getUserIfExists(string Email, string Password)
        {
            return skilledForceDB.User.Where(u => u.Email == Email && u.Password == Password).FirstOrDefault();
        }

        public List<Role> GetRoles() => skilledForceDB.Role.ToList();

        public IActionResult EditDetails()
        {
            User user = skilledForceDB.User.Where(u => u.UserId.Equals(TempData.Peek("UserId").ToString())).FirstOrDefault();
            ViewBag.edit = true;
            LoadMetaData();
            return View("RegistrationForm", user);
        }

        public void LoadMetaData()
        {
            List<SelectListItem> roles = new List<SelectListItem>();
            foreach (Role role in GetRoles())
            {
                roles.Add(new SelectListItem { Value = role.RoleId, Text = role.Name });
            }
            ViewBag.roles = roles;
            ViewBag.Gender = new List<SelectListItem>() {
                new SelectListItem { Value = "", Text = ""},
                new SelectListItem { Value = "Male", Text = "Male"},
                new SelectListItem { Value = "Female", Text = "Female"},
                new SelectListItem { Value = "Other", Text = "Other"},
            };
        }

        public void UpdateUserData(User user)
        {
            TempData["UserId"] = user.UserId.ToString();
            TempData["Email"] = user.Email;
            TempData["FirstName"] = user.FirstName;
            TempData["RoleId"] = user.RoleId;
        }
    }
}
