using Microsoft.AspNetCore.Mvc;
using Skilled_Force_VS_22.Manager;
using Skilled_Force_VS_22.Models;
using Skilled_Force_VS_22.Models.DB;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

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

        public async Task<IActionResult> Index(string keywords, string location, string jobType, int? pageNumber)
        {
            if (HttpContext.Session.IsAvailable && HttpContext.Session.Keys.Contains("UserId") && HttpContext.Session.GetString("UserId") != null)
            {
                loadMetaInfo();
                // ViewBag.jobs = GetListAsync(keywords, location, jobType, 5);
                return await GetListAsync(keywords, location, jobType, pageNumber).ConfigureAwait(false);
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
        public async Task<IActionResult> GetListAsync(string keywords, string location, string jobtype, int? pageNumber)
        {
            string userId = HttpContext.Session.GetString("UserId").ToString();
            string roleId = HttpContext.Session.GetString("RoleId").ToString();
            IQueryable<Job>? sqlQuery = skilledForceDB.Job;
            if (roleId.Equals("1")) //Job Seeker
            {
                ViewBag.AppliedJobIds = skilledForceDB.JobApplication.Where(u => u.ApplicantUserId == userId).Select(jApp => jApp.JobId).ToList();
            }
            else if (roleId.Equals("2"))
            {
                sqlQuery = skilledForceDB.Job.Where(j => j.CreatedByUserId == userId);
            }
            else if (roleId.Equals("3"))
            {
                sqlQuery = skilledForceDB.Job.Where(j => j.CompanyId == HttpContext.Session.GetString("CompanyId").ToString());
            }

            if (keywords != null && keywords != "")
                sqlQuery = sqlQuery.Where(j => j.Title.Contains(keywords) || j.Description.Contains(keywords) || j.Salary.Contains(keywords) ||
                            j.EmploymentType.Contains(keywords) || j.Location.Contains(keywords) || j.JobType.Contains(keywords));
            if (location != null && location != "")
                sqlQuery = sqlQuery.Where(j => j.Location.Contains(location));
            if (jobtype != null && jobtype != "")
                sqlQuery = sqlQuery.Where(j => j.JobType.Contains(jobtype));

            sqlQuery = sqlQuery.OrderByDescending(j => j.UpdatedAt).OrderByDescending(j => j.UpdatedAt);

            return View(await PaginatedList<Job>.CreateAsync(source: sqlQuery.AsNoTracking(), pageIndex: pageNumber ?? 1, pageSize: 3));
        }

        private void loadMetaInfo()
        {

            List<SelectListItem> Locations = skilledForceDB.Job.GroupBy(J => J.Location).Select(entry => new SelectListItem { Value = entry.Key, Text = entry.Key }).ToList();
            Locations.Insert(0, new SelectListItem { Value = "", Text = "Select Location" });
            ViewBag.Locations = Locations;

            List<SelectListItem> JobTypes = skilledForceDB.Job.GroupBy(J => J.JobType).Select(entry => new SelectListItem { Value = entry.Key, Text = entry.Key }).ToList();
            JobTypes.Insert(0, new SelectListItem { Value = "", Text = "Select JobType" });
            ViewBag.jobTypes = JobTypes;
        }
    }
}