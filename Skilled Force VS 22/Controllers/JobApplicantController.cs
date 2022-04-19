using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skilled_Force_VS_22.Manager;
using Skilled_Force_VS_22.Models.DB;
using Skilled_Force_VS_22.Util;

namespace Skilled_Force_VS_22.Controllers
{
    public class JobApplicantController : Controller
    {
        private readonly ILogger<JobApplicantController> _logger;
        private readonly SkilledForceDB skilledForceDB;

        public JobApplicantController(ILogger<JobApplicantController> logger, SkilledForceDB skilledForceDB)
        {
            _logger = logger;
            this.skilledForceDB = skilledForceDB;
        }

        [HttpGet]
        public async Task<IActionResult> JobApplicantsList(String JobId, int? pageNumber)
        {
            IQueryable<JobApplication> sqlQuery = skilledForceDB.JobApplication.Include(jobApp => jobApp.ApplicantUser).Where(jobApp => jobApp.JobId == JobId);
            return View(viewName: "JobApplicantsList", await PaginatedList<JobApplication>.CreateAsync(source: sqlQuery.AsNoTracking(), pageIndex: pageNumber ?? 1, pageSize: 3));

        }

        [HttpGet]
        public async Task<IActionResult> UpdateApplicatntStatus(String JobApplicationId, JobApplicationStatusEnum status)
        {
            JobApplication SelectedJobApplication = skilledForceDB.JobApplication.Where(jobApp => jobApp.JobApplicationId == JobApplicationId).FirstOrDefault();
            SelectedJobApplication.Status = status;
            skilledForceDB.JobApplication.Update(SelectedJobApplication);
            skilledForceDB.SaveChanges();
            return RedirectToAction("JobApplicantsList", "JobApplicant", new { JobId = SelectedJobApplication.JobId });

        }


        [HttpGet]
        public IActionResult UserJobApplicationsList()
        {
            string userId = HttpContext.Session.GetString("UserId").ToString();
            List<JobApplication> jobApplications = skilledForceDB.JobApplication.Include(app=>app.Job).Where(app => app.ApplicantUserId == userId).ToList();
            return View("UserJobApplicationsList", jobApplications);

        }
    }
}
