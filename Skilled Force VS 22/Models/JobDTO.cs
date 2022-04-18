using System.ComponentModel.DataAnnotations;

namespace Skilled_Force_VS_22.Models
{
    public class JobDTO
    {
        public string? JobId { get; set; }

        [Required]
        [Display(Name = "Title")]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Location")]
        [StringLength(50)]
        public string Location { get; set; }

        [Required]
        [Display(Name = "JobType")]
        [StringLength(50)]
        public string JobType { get; set; }

        [Required]
        [Display(Name = "Salary")]
        [StringLength(50)]
        public string Salary { get; set; }

        [Required]
        [Display(Name = "EmploymentType [Part Time / Full Time]")]
        [StringLength(50)]
        public string EmploymentType { get; set; }

        public string CompanyId { get; set; }
    }
}
