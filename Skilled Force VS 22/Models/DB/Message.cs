using Skilled_Force_VS_22.Models.DB;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Skilled_Force_VS_22.Models.DB
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string MessageId { get; set; }

        public string UserMessage { get; set; }

        [Column("CreatedAt")]
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
        public virtual User FromUser { get; set; }

        public virtual User ToUser { get; set; }

        public string ChatId { get; set; }
    }
}
