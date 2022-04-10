using Skilled_Force_VS_22.Models.DB;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Skilled_Force_VS_22.Models.DB
{
    public class Chat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ChatId { get; set; }

        [Column("CreatedAt")]
        [Display(Name = "Created At")]
        public DateTime UpdatedTime { get; set; }
        public bool IsRead { get; set; }

        [Required]
        public string FromUserId { get; set; }
        [Required]
        public string ToUserId { get; set; }

        [ForeignKey("FromUserId")]
        public virtual User FromUser { get; set; }

        [ForeignKey("ToUserId")]
        public virtual User ToUser { get; set; }

        public virtual IList<Message> Messages { get; set; }

    }
}
