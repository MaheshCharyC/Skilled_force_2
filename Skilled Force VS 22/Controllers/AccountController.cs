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
                ViewBag.success = true;
                return RedirectToAction("Index", "Home");
            }
            ViewData["success"] = false;
            return LoginForm();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                HttpContext.Session.Clear();
                TempData.Clear();
            }                
            return LoginForm();
        }

        [HttpGet]
        public IActionResult Register()
        {
            LoadMetaData();
            ViewBag.edit = false;
            return View("RegistrationForm");
        }

        [HttpGet]
        public IActionResult CompanyRegister()
        {
            LoadMetaData();
            ViewBag.edit = false;
            return View("CompanyRegistrationForm");
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            ModelState.Remove("Jobs");
            ModelState.Remove("Role");
            ModelState.Remove("Role.RoleId");
            ModelState.Remove("Role.Name");
            ModelState.Remove("Role.Users");
            ModelState.Remove("Role.Description");
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.IsAvailable)
                {
                    user.UserId = HttpContext.Session.GetString("UserId").ToString();
                    skilledForceDB.User.Update(user);
                    skilledForceDB.SaveChanges();
                    UpdateUserData(user);
                    ViewBag.success = true;
                    return RedirectToAction("Index", "Home");
                }
                int userExists = skilledForceDB.User.Where(u => u.Email == user.Email).Count();
                if (userExists == 0)
                {
                    skilledForceDB.User.Add(user);
                    skilledForceDB.SaveChanges();
                    ViewBag.SuccessMessage = "Saved User Successfully";
                    return LoginForm();
                }
                ViewBag.Error = "User Email exists";
            }
            return Register();
        }

        [HttpPost]
        public IActionResult CompanyRegister(User user)
        {
            ModelState.Remove("Jobs");
            ModelState.Remove("Role");
            ModelState.Remove("Role.RoleId");
            ModelState.Remove("Role.Name");
            ModelState.Remove("Role.Users");
            ModelState.Remove("Role.Description");
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.IsAvailable)
                {
                    user.UserId = HttpContext.Session.GetString("UserId").ToString();
                    skilledForceDB.User.Update(user);
                    skilledForceDB.SaveChanges();
                    UpdateUserData(user);
                    ViewBag.success = true;
                    return RedirectToAction("Index", "Home");
                }
                int userExists = skilledForceDB.User.Where(u => u.Email == user.Email).Count();
                if (userExists == 0)
                {
                    skilledForceDB.User.Add(user);
                    skilledForceDB.SaveChanges();
                    ViewBag.SuccessMessage = "Saved User Successfully";
                    return LoginForm();
                }
                ViewBag.Error = "User Email exists";
            }
            return CompanyRegister();
        }

        private User getUserIfExists(string Email, string Password)
        {
            return skilledForceDB.User.Where(u => u.Email == Email && u.Password == Password).FirstOrDefault();
        }

        public List<Role> GetRoles() => skilledForceDB.Role.ToList();

        public IActionResult EditDetails()
        {
            User user = skilledForceDB.User.Where(u => u.UserId.Equals(HttpContext.Session.GetString("UserId"))).FirstOrDefault();
            ViewBag.edit = true;
            LoadMetaData();
            return View("RegistrationForm", user);
        }

        public void LoadMetaData()
        {
            ViewBag.Gender = new List<SelectListItem>() {
                new SelectListItem { Value = "", Text = ""},
                new SelectListItem { Value = "Male", Text = "Male"},
                new SelectListItem { Value = "Female", Text = "Female"},
                new SelectListItem { Value = "Other", Text = "Other"},
            };
        }

        public void UpdateUserData(User user)
        {
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("FirstName", user.FirstName);
            HttpContext.Session.SetString("RoleId", user.RoleId);
            TempData["UserId"] = user.UserId.ToString();
            TempData["Email"] = user.Email;
            TempData["FirstName"] = user.FirstName;
            TempData["RoleId"] = user.RoleId;
        }

        public List<User> Users()
        {
            List<User> users = skilledForceDB.User.Where(u => u.RoleId.Equals(2)).ToList();
            return users;
        }

        

    }
}
