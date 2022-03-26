using Skilled_Force_VS_22.Models.DB;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Skilled_Force_VS_22.Models.DB
{
    public class User
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UserId { get; set; }

        [Column("FirstName")]
        [Required(ErrorMessage = "Please enter First Name")]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last Name")]
        [Column("LastName")]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter Gender")]
        [Column("Gender")]
        [Display(Name = "Gender")]
        [StringLength(50)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please enter Phone")]
        [Column("Phone")]
        [Display(Name = "Phone")]
        [StringLength(50)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        [Column("Email")]
        [Display(Name = "Email")]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        public string RoleId { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [Column("Password")]
        [Display(Name = "Password")]
        [StringLength(50)]
        public string Password { get; set; }


        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return LastName + ", " + FirstName; }
        }

        public virtual Role Role { get; set; }

        public virtual IList<Job> Jobs { get; set; }

        public virtual Company Company { get; set; }

        public virtual IList<CompanyReview> CompanyReviews { get; set; }
    }
}
