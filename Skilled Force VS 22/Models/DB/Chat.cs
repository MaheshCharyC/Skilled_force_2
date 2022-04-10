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
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public virtual User FromUser { get; set; }

        public virtual User ToUser { get; set; }
    }
}
