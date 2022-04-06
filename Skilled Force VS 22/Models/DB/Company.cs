using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Skilled_Force_VS_22.Models.DB
{
    public class Company
    {

        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CompanyId { get; set; }


        [Required]
        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual IList<CompanyReview> CompanyReviews { get; set; }

        //public virtual IList<Job> Jobs { get; set; }



    }
}
