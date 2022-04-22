using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Skilled_Force_VS_22.Manager;
using Skilled_Force_VS_22.Models;
using Skilled_Force_VS_22.Models.DB;
using Skilled_Force_VS_22.Util;
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
            ViewBag.edit = false;
            return View("JobPostForm");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult JobPostForm(JobDTO jobDto)
        {
            Job job;
            if (jobDto.JobId != null)
            {
                job = skilledForceDB.Job.Where(j => j.JobId == jobDto.JobId).FirstOrDefault();
            }
            else
            {
                job = new Job();
            }

            job.JobType = jobDto.JobType;
            job.EmploymentType = jobDto.EmploymentType;
            job.Title = jobDto.Title;
            job.Description = jobDto.Description;
            job.Location = jobDto.Location;
            job.Salary = jobDto.Salary;
            job.CompanyId = jobDto.CompanyId;

            ModelState.Remove("JobId");
            ModelState.Remove("Users");
            ModelState.Remove("CreatedAt");
            ModelState.Remove("CreatedBy");
            ModelState.Remove("UpdatedBy");
            ModelState.Remove("CreatedByUserId");
            ModelState.Remove("UpdatedByUserId");
            ModelState.Remove("Company");
            ModelState.Remove("JobApplications");
            if (ModelState.IsValid)
            {
                job.CompanyId = HttpContext.Session.GetString("CompanyId").ToString();
                if (job.JobId == null)
                {
                    job.CreatedByUserId = HttpContext.Session.GetString("UserId").ToString();
                    job.CreatedAt = DateTime.Now;
                    job.UpdatedByUserId = HttpContext.Session.GetString("UserId").ToString();
                    job.UpdatedAt = DateTime.Now;
                    TempData["Success"] = "Created Job post successfully!";
                    skilledForceDB.Job.Add(job);
                }
                else
                {
                    job.UpdatedByUserId = HttpContext.Session.GetString("UserId").ToString();
                    job.UpdatedAt = DateTime.Now;
                    TempData["Success"] = "Update Job post successfully!";
                    skilledForceDB.Job.Update(job);
                }
                skilledForceDB.SaveChanges();
            }
            else
            {
                TempData["Error"] = "Error posting job";
                if (job.JobId == null)
                    return JobPostForm();
                else
                    return JobEditForm(job.JobId);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult JobEditForm(string jobId)
        {
            Job job = skilledForceDB.Job.Where(j => j.JobId == jobId).FirstOrDefault();
            JobDTO jobDTO = new JobDTO();

            jobDTO.JobType = job.JobType;
            jobDTO.EmploymentType = job.EmploymentType;
            jobDTO.Title = job.Title;
            jobDTO.Description = job.Description;
            jobDTO.Location = job.Location;
            jobDTO.Salary = job.Salary;
            jobDTO.CompanyId = job.CompanyId;


            ViewBag.edit = true;
            return View("JobPostForm", jobDTO);
        }

        public IActionResult JobApply(string jobId)
        {
            if (HttpContext.Session.GetString("UserId").Equals("0"))
                return RedirectToAction("LoginForm", "Account");
            string userId = HttpContext.Session.GetString("UserId").ToString();
            if (!skilledForceDB.JobApplication.Where(j => j.JobId == jobId && j.ApplicantUserId == userId).Any())
            {
                JobApplication jobApplication = new JobApplication();
                jobApplication.ApplicantUserId = userId;
                jobApplication.JobId = jobId;
                jobApplication.Status = JobApplicationStatusEnum.PENDING;
                skilledForceDB.JobApplication.Add(jobApplication);
                skilledForceDB.SaveChanges();

            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult JobCancel(string jobId)
        {
            string userId = HttpContext.Session.GetString("UserId").ToString();
            JobApplication? existingApplication = skilledForceDB.JobApplication.Where(job => job.ApplicantUserId == userId && job.JobId == jobId).FirstOrDefault();
            if (existingApplication != null)
            {
                skilledForceDB.JobApplication.Remove(existingApplication);
                skilledForceDB.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult JobDelete(string jobId)
        {
            skilledForceDB.Job.Remove(skilledForceDB.Job.Where(job => job.JobId.Equals(jobId)).FirstOrDefault());
            skilledForceDB.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ViewJob(string jobId)
        {
            string userId = HttpContext.Session.GetString("UserId").ToString();
            Job job = skilledForceDB.Job.Include(j => j.CreatedBy).Where(j => j.JobId == jobId).FirstOrDefault();
            ViewBag.IsApplied = skilledForceDB.JobApplication.Where(job => job.ApplicantUserId == userId && job.JobId == jobId).Any();

            Company company = skilledForceDB.Company.Where(c => c.CompanyId.Equals(job.CompanyId)).FirstOrDefault();
            ViewBag.companyName = company.Name;
            ViewBag.companyDesc = company.Description;
            return View("JobDetails", job);
        }

        private User GetUser(string userId)
        {
            return skilledForceDB.User.Where(u => u.UserId.Equals(userId)).FirstOrDefault();
        }
    }
}
