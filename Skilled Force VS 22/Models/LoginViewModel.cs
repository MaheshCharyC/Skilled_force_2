using System;
using System.ComponentModel.DataAnnotations;

namespace Skilled_Force_VS_22.Models
{
    public class LoginViewModel {

        [Required(ErrorMessage = "Please enter Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}
