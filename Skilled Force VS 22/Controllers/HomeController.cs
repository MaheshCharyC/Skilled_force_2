using Microsoft.AspNetCore.Mvc;
using Skilled_Force_VS_22.Manager;
using Skilled_Force_VS_22.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Skilled_Force_VS_22.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SkilledForceDB skilledForceDB;

        public HomeController(ILogger<HomeController> logger, SkilledForceDB skilledForceDB)
        {
            _logger = logger;
            this.skilledForceDB = skilledForceDB;
        }

        public IActionResult Index()
        {
            if (TempData.Peek("UserId") != null)
            {
                ViewBag.jobs = GetList();
                return View();
            }
            else
                return RedirectToAction("LoginForm", "Account");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public List<Job> GetList()
        {
            if (TempData.Peek("RoleId").Equals("2"))
            {
                return skilledForceDB.Job.Include(job => job.Users).Where(j => j.CreatedBy == TempData.Peek("UserId").ToString()).OrderByDescending(j => j.UpdatedAt).ToList();
            }
            else if (TempData.Peek("RoleId").Equals("1"))
            {
                User user = skilledForceDB.User.Where(u => u.UserId.Equals(TempData.Peek("UserId").ToString())).FirstOrDefault(); ;
                List<Job> allJobs = skilledForceDB.Job.Include(job => job.Users).ToList();
                allJobs.ForEach(job => job.IsApplied = job.Users.Contains(user));
                return allJobs;
            }
            return skilledForceDB.Job.Include(job => job.Users).ToList();
        }
    }
}