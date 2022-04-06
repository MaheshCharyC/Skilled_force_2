using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Skilled_Force_VS_22.Models.DB
{
    public class Job
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string JobId { get; set; }


        [Required]
        [Column("Title")]
        [Display(Name = "Title")]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [Column("Description")]
        [Display(Name = "Description")]
        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        [Column("Location")]
        [Display(Name = "Location")]
        [StringLength(50)]
        public string Location { get; set; }

        [Required]
        [Column("JobType")]
        [Display(Name = "JobType")]
        [StringLength(50)]
        public string JobType { get; set; }

        [Required]
        [Column("Salary")]
        [Display(Name = "Salary")]
        [StringLength(50)]
        public string Salary { get; set; }

        [Required]
        [Column("EmploymentType")]
        [Display(Name = "EmploymentType [Part Time / Full Time]")]
        [StringLength(50)]
        public string EmploymentType { get; set; }

        [Column("CreatedBy")]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Column("UpdatedBy")]
        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }

        [Column("CreatedAt")]
        //[DisplayFormat(DataFormatString = "{0:d} at {0:t}", ApplyFormatInEditMode = true)]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Column("UpdatedAt")]
        //[DisplayFormat(DataFormatString = "{0:d} at {0:t}", ApplyFormatInEditMode = true)]
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }
        
        [Column("IsApplied")]
        public bool IsApplied { get; set; }
        public virtual IList<User> Users { get; set; }

        public string CompanyId { get; set; }
    }
}
