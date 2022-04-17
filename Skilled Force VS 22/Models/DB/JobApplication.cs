using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Skilled_Force_VS_22.Models.DB
{
    public class JobApplication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string JobApplicationId { get; set; }


        [Column("ApplicantUserId")]
        public string ApplicantUserId { get; set; }


        [Column("JobId")]
        public string JobId { get; set; }


        [ForeignKey("ApplicantUserId")]
        public virtual User ApplicantUser { get; set; }

        [ForeignKey("JobId")]
        public virtual Job Job { get; set; }



    }
}
