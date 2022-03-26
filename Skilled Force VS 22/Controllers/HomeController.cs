using Microsoft.AspNetCore.Mvc;
using Skilled_Force_VS_22.Manager;
using Skilled_Force_VS_22.Models;
using Skilled_Force_VS_22.Models.DB;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Index(string keywords, string location, string jobType)
        {
            if (TempData.Peek("UserId") != null)
            {
                loadMetaInfo();
                ViewBag.jobs = GetList(keywords, location, jobType);
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
        public List<Job> GetList(string keywords, string location, string jobtype)
        {
            List<Job>? jobs = null;
            IQueryable<Job>? sqlData = null;
            if (TempData.Peek("RoleId").Equals("2"))
                sqlData = skilledForceDB.Job.Include(job => job.Users).Where(j => j.CreatedBy == TempData.Peek("UserId").ToString());
            else 
                sqlData = skilledForceDB.Job.Include(job => job.Users);

            if (keywords != null && keywords != "")
                sqlData = sqlData.Where(j => j.Title.Contains(keywords) || j.Description.Contains(keywords) || j.Salary.Contains(keywords) ||
                            j.EmploymentType.Contains(keywords) || j.Location.Contains(keywords) || j.JobType.Contains(keywords));
            if (location != null && location != "")
                sqlData = sqlData.Where(j => j.Location.Contains(location));
            if (jobtype != null && jobtype  != "")
                sqlData = sqlData.Where(j => j.JobType.Contains(jobtype));

            jobs = sqlData.OrderByDescending(j => j.UpdatedAt).OrderByDescending(j => j.UpdatedAt).ToList();
            if (TempData.Peek("RoleId").Equals("1"))
            {
                User user = skilledForceDB.User.Where(u => u.UserId.Equals(TempData.Peek("UserId").ToString())).FirstOrDefault(); ;
                jobs.ForEach(job => job.IsApplied = job.Users.Contains(user));
            }

            return jobs;
        }

        private void loadMetaInfo()
        {
            ViewBag.Locations = new List<SelectListItem>() {
                    new SelectListItem { Value = "", Text = "Select Location"},
                    new SelectListItem { Value = "Chicago", Text = "Chicago"},
                    new SelectListItem { Value = "Michigan", Text = "Michigan"},
                    new SelectListItem { Value = "New York", Text = "New York"}
                };
            ViewBag.jobTypes = new List<SelectListItem>() {
                    new SelectListItem { Value = "", Text = "Select Job Type"},
                    new SelectListItem { Value = "Front End Developer", Text = "Front End Developer"},
                    new SelectListItem { Value = "Java Developer", Text = "Java Developer"},
                    new SelectListItem { Value = "Web Developer", Text = "Web Developer"}
                };
        }
    }
}