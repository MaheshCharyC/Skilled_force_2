using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skilled_Force_VS_22.Manager;
using Skilled_Force_VS_22.Models.DB;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Skilled_Force_VS_22.Controllers
{
    public class CompanyReviewController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SkilledForceDB skilledForceDB;

        public CompanyReviewController(ILogger<HomeController> logger, SkilledForceDB skilledForceDB)
        {
            _logger = logger;
            this.skilledForceDB = skilledForceDB;
        }

        [HttpPost]
        public IActionResult AddReview(CompanyReview companyReview)
        {
            companyReview.UserId = TempData.Peek("UserId").ToString();
            companyReview.Time = DateTime.Now;
            skilledForceDB.CompanyReview.Add(companyReview);
            skilledForceDB.SaveChanges();


            return RedirectToAction("GetCompanyDetails", "Company", new { companyReview.CompanyId });
        }




    }
}
