using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skilled_Force_VS_22.Manager;
using Skilled_Force_VS_22.Models.DB;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Skilled_Force_VS_22.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SkilledForceDB skilledForceDB;

        public CompanyController(ILogger<HomeController> logger, SkilledForceDB skilledForceDB)
        {
            _logger = logger;
            this.skilledForceDB = skilledForceDB;
        }


        [HttpGet]
        public IActionResult GetCompanyDetails(String CompanyId)
        {
            Company? company;
            if (CompanyId != null)
            {
                company = skilledForceDB.Company
                    .Include(c => c.CompanyReviews)
                    .ThenInclude(c => c.User)
                    .Where(c => c.CompanyId == CompanyId).FirstOrDefault();
            }
            else
            {
                company = skilledForceDB.Company
                   .Include(c => c.CompanyReviews)
                   .ThenInclude(c => c.User)
                   .Where(c => c.UserId.Equals(TempData.Peek("UserId"))).FirstOrDefault();
            }


            return View("CompanyDetails", company);
        }

        [HttpGet]
        public IActionResult EditCompanyDetails(String companyId)
        {
            Company? company = skilledForceDB.Company
                .Where(c => c.CompanyId == companyId).FirstOrDefault();

            return View("CompanyDetailsEdit", company);
        }

        [HttpPost]
        public IActionResult EditCompanyDetails(Company company)
        {
            Company? existingCompany = skilledForceDB.Company
                                            .Where(c => c.CompanyId == company.CompanyId).FirstOrDefault();

            existingCompany.Description = company.Description;
            existingCompany.Name = company.Name;

            skilledForceDB.Company.Update(existingCompany);
            skilledForceDB.SaveChanges();


            return RedirectToAction("GetCompanyDetails", "Company", new { company.CompanyId });
        }




    }
}
