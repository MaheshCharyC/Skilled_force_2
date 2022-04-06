using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Skilled_Force_VS_22.Manager;
using Skilled_Force_VS_22.Models;
using Skilled_Force_VS_22.Models.DB;
using System.Diagnostics;

namespace Skilled_Force_VS_22.Controllers
{
    public class JobController : Controller
    {
        private readonly ILogger<JobController> _logger;
        private readonly SkilledForceDB skilledForceDB;

        public JobController(ILogger<JobController> logger, SkilledForceDB skilledForceDB)
        {
            _logger = logger;
            this.skilledForceDB = skilledForceDB;
        }
        public IActionResult JobPostForm()
        {
            addMetaDataToViewBag();
            ViewBag.edit = false;
            return View("JobPostForm");
        }

        private void addMetaDataToViewBag()
        {
            ViewBag.JobType = new List<SelectListItem>()
            {
                new SelectListItem { Value = "", Text = "Select"},
                new SelectListItem { Value = "JavaDeveloper", Text = "Java Developer"},
                new SelectListItem { Value = "WebDeveloper", Text = "Web Developer"},
                new SelectListItem { Value = "FrontEndDeveloper", Text = "FrontEnd Developer"},
                new SelectListItem { Value = "Tester", Text = "Tester"},
                new SelectListItem { Value = "Other", Text = "Other"}
            };
            ViewBag.EmploymentType = new List<SelectListItem>()
            {
                new SelectListItem { Value = "", Text = "Select"},
                new SelectListItem { Value = "FullTime", Text = "FullTime"},
                new SelectListItem { Value = "PartTime", Text = "PartTime"},
                new SelectListItem { Value = "FullTime - Remote", Text = "FullTime - Remote"},
                new SelectListItem { Value = "PartTime - Remote", Text = "PartTime - Remote"}
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult PostJob(Job job)
        {
            ModelState.Remove("JobId");
            ModelState.Remove("Users");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("CreatedBy");
            ModelState.Remove("UpdatedBy");
            if (ModelState.IsValid)
            {
                if(job.JobId == null)
                {
                    job.CreatedBy = HttpContext.Session.GetString("UserId").ToString();
                    job.CreatedAt = DateTime.Now;
                    job.UpdatedBy = HttpContext.Session.GetString("UserId").ToString();
                    job.UpdatedAt = DateTime.Now;
                    skilledForceDB.Job.Add(job);
                } else
                {
                    job.UpdatedBy = HttpContext.Session.GetString("UserId").ToString();
                    job.UpdatedAt = DateTime.Now;
                    skilledForceDB.Job.Update(job);
                }
                skilledForceDB.SaveChanges();
            }
            else
            {
                return JobPostForm();
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult JobEditForm(string jobId)
        {
            Job job = skilledForceDB.Job.Where(j => j.JobId == jobId).FirstOrDefault();
            ViewBag.edit = true;
            addMetaDataToViewBag();
            return View("JobPostForm", job);
        }

        public IActionResult JobApply(string jobId)
        {
            User user = skilledForceDB.User.Where(u => u.UserId.Equals(HttpContext.Session.GetString("UserId").ToString())).FirstOrDefault();
            Job job = skilledForceDB.Job.Include(job => job.Users).Where(j => j.JobId == jobId).FirstOrDefault();
            job.Users = new List<User>() { user};
            skilledForceDB.SaveChanges();
            ViewBag.success = true;
            return RedirectToAction("Index", "Home");
        }

        public IActionResult JobCancel(string jobId)
        {
            User user = skilledForceDB.User.Where(u => u.UserId.Equals(HttpContext.Session.GetString("UserId").ToString())).FirstOrDefault();
            Job job = skilledForceDB.Job.Include(job => job.Users).Where(j => j.JobId == jobId).FirstOrDefault();
            job.Users.Remove(user);
            skilledForceDB.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult JobDelete(string jobId)
        {
            skilledForceDB.Job.Remove(skilledForceDB.Job.Where(job => job.JobId.Equals(jobId)).FirstOrDefault());
            skilledForceDB.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
