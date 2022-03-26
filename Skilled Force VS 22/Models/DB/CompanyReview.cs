using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Skilled_Force_VS_22.Models.DB
{
    public class CompanyReview
    {

        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CompanyRatingId { get; set; }


        [Required]
        public string CompanyId { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public string comment { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }


    }
}
