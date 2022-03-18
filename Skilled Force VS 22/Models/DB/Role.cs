using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Skilled_Force_VS_22.Models
{
    public class Role
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string RoleId { get; set; }

        [Required]
        [Column("Name")]
        [Display(Name = "Name")]
        [StringLength(50)]
        public string Name { get; set; }

        [Column("Description")]
        [Display(Name = "Description")]
        [StringLength(500)]
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }

    }
}
