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

        public string ChatId { get; set; }

        public string FromUserId { get; set; }

        public DateTime Time { get; set; }

        [ForeignKey("ChatId")]
        public virtual Chat Chat { get; set; }

        [ForeignKey("FromUserId")]
        public virtual User FromUser { get; set; }

    }
}
