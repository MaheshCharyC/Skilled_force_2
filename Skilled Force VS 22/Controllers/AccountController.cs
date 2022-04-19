using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Skilled_Force_VS_22.Manager;
using Skilled_Force_VS_22.Models.DB;
using Skilled_Force_VS_22.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
                UpdateSession(exisitngUser);
                ViewBag.success = true;
                if(exisitngUser.RoleId.Equals("3"))
                {
                    return RedirectToAction("GetRecruiters", "Account");
                }
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


        [HttpGet]
        public async Task<IActionResult> RecruiterRegister()
        {
            LoadMetaData();
            ViewBag.edit = false;
            return View("RecruiterRegistrationForm");
        }

        [HttpGet]
        public IActionResult UpdateRecruiterProfile()
        {
            LoadMetaData();
            ViewBag.edit = false;
            return View("RecruiterRegistrationForm");
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            SetModel();
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("UserId") != null)
                {
                    user.UserId = HttpContext.Session.GetString("UserId").ToString();
                    skilledForceDB.User.Update(user);
                    skilledForceDB.SaveChanges();
                    UpdateSession(user);
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
        public Task<IActionResult> RecruiterRegister(User user)
        {            
            SetModel();
            if (ModelState.IsValid)
            {
                if (user.UserId != null)
                {
                    skilledForceDB.User.Update(user);
                    ViewBag.success = true;
                    skilledForceDB.SaveChanges();
                    return GetRecruiters(1);
                }
                int userExists = skilledForceDB.User.Where(u => u.Email == user.Email).Count();
                if (userExists == 0)
                {
                    skilledForceDB.User.Add(user);
                    ViewBag.success = true;
                    skilledForceDB.SaveChanges();
                    return GetRecruiters(1);
                }
                ViewBag.Error = "User Email exists";
            } else
            {
                if (user.UserId != null)
                    return EditRecruiterDetails(user.UserId);
            }
            return RecruiterRegister();

        }


        [HttpPost]
        public IActionResult UpdateRecruiterProfile(User user)
        {
            SetModel();
            if (ModelState.IsValid)
            {
                if (user.UserId != null)
                {
                    skilledForceDB.User.Update(user);
                    ViewBag.success = true;
                    skilledForceDB.SaveChanges();
                    UpdateSession(user);
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Error = "Unable to update";
            }
            LoadMetaData();
            ViewBag.edit = true;
            return View("RecruiterRegistrationForm", user);
        }


        [HttpPost]
        public IActionResult CompanyRegister(User user)
        {
            ModelState.Remove("Jobs");
            ModelState.Remove("Role");
            ModelState.Remove("Company.UserId");
            ModelState.Remove("Company.CompanyId");
            ModelState.Remove("Company.User");
            ModelState.Remove("Company.CompanyReviews");
            ModelState.Remove("CompanyReviews");
            ModelState.Remove("Company");
            ModelState.Remove("CompanyId");
            ModelState.Remove("UserId");
            ModelState.Remove("CreatedJobs");
            ModelState.Remove("SentChats");
            ModelState.Remove("SentMessages");
            ModelState.Remove("ReceivedChats");
            ModelState.Remove("JobApplications");
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("UserId") != null)
                {
                    user.UserId = HttpContext.Session.GetString("UserId").ToString();
                    skilledForceDB.User.Update(user);
                    skilledForceDB.SaveChanges();
                    UpdateSession(user);
                    ViewBag.success = true;
                    return RedirectToAction("Index", "Home");
                }
                int userExists = skilledForceDB.User.Where(u => u.Email == user.Email).Count();
                if (userExists == 0)
                {
                    skilledForceDB.User.Add(user);
                    user.CompanyId = user.Company.CompanyId.ToString();
                    skilledForceDB.SaveChanges();
                    ViewBag.SuccessMessage = "Saved User Successfully";
                    return LoginForm();
                }
                ViewBag.Error = "User Email exists";
            }
            return CompanyRegister();
        }

        [HttpGet]
        public IActionResult GetAccountDetails(string? userId)
        {
            if (userId==null)
            {
                userId = HttpContext.Session.GetString("UserId");
            }
            User user = skilledForceDB.User.Where(u => u.UserId == userId).First();

            return View("GetAccountDetails", user);
        }


        private void SetModel()
        {
            ModelState.Remove("Jobs");
            ModelState.Remove("Role");
            ModelState.Remove("Company.UserId");
            ModelState.Remove("Company.CompanyId");
            ModelState.Remove("Company.Name");
            ModelState.Remove("Company.User");
            ModelState.Remove("Company.Description");
            ModelState.Remove("Company.CompanyReviews");
            ModelState.Remove("CompanyReviews");
            ModelState.Remove("Company");
            ModelState.Remove("CompanyId");
            ModelState.Remove("UserId");
            ModelState.Remove("CreatedJobs");
            ModelState.Remove("SentChats");
            ModelState.Remove("SentMessages");
            ModelState.Remove("ReceivedChats");
            ModelState.Remove("JobApplications");
        }

        private User getUserIfExists(string Email, string Password)
        {
            return skilledForceDB.User.Where(u => u.Email == Email && u.Password == Password).FirstOrDefault();
        }

        public List<Role> GetRoles() => skilledForceDB.Role.ToList();

        public IActionResult EditDetails()
        {            
            ViewBag.edit = true;
            LoadMetaData();
            if (HttpContext.Session.GetString("RoleId").Equals("3"))
            {
                User user = skilledForceDB.User.Include(u => u.Company).Where(u => u.UserId.Equals(HttpContext.Session.GetString("UserId"))).FirstOrDefault();
                return View("CompanyRegistrationForm", user);
            } else if (HttpContext.Session.GetString("RoleId").Equals("2"))
            {
                User user = skilledForceDB.User.Include(u => u.Company).Where(u => u.UserId.Equals(HttpContext.Session.GetString("UserId"))).FirstOrDefault();
                return View("RecruiterRegistrationForm", user);
            } else
            {
                User user = skilledForceDB.User.Where(u => u.UserId.Equals(HttpContext.Session.GetString("UserId"))).FirstOrDefault();
                return View("RegistrationForm", user);
            }            
        }

        public async Task<IActionResult> EditRecruiterDetails(string userId)
        {
            User user = skilledForceDB.User.Where(u => u.UserId.Equals(userId)).FirstOrDefault();
            ViewBag.edit = true;
            LoadMetaData();
            return View("RecruiterRegistrationForm", user);
        }

        public void LoadMetaData()
        {
            ViewBag.Gender = new List<SelectListItem>() {
                new SelectListItem { Value = "", Text = "Select Gender"},
                new SelectListItem { Value = "Male", Text = "Male"},
                new SelectListItem { Value = "Female", Text = "Female"},
                new SelectListItem { Value = "Other", Text = "Other"},
            };
        }

        public void UpdateSession(User user)
        {
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("FirstName", user.FirstName);
            HttpContext.Session.SetString("RoleId", user.RoleId);
            if(user.CompanyId != null)
            {
                HttpContext.Session.SetString("CompanyId", user.CompanyId);
            }
        }

        public async Task<IActionResult> GetRecruiters(int? pageNumber)
        {
            IQueryable<User> sqlQuery = skilledForceDB.User.Where(u => u.RoleId.Equals("2") && u.CompanyId.Equals(HttpContext.Session.GetString("CompanyId")));
            return View(viewName: "GetRecruiters", await PaginatedList<User>.CreateAsync(source: sqlQuery.AsNoTracking(), pageIndex: pageNumber ?? 1, pageSize: 3));
            /*ViewBag.users = users;
            return View("GetRecruiters");*/
        }

        public async Task<IActionResult> DeleteRecruiter(string userId)
        {
            User user = skilledForceDB.User.Where(u => u.UserId.Equals(userId)).FirstOrDefault();
            skilledForceDB.User.Remove(user);
            skilledForceDB.SaveChanges();
            return await GetRecruiters(1);
        }

    }
}
