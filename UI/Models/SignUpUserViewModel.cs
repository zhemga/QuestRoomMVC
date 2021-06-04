using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class SignUpUserViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [MaxLength(128)]
        [CustomPhone]
        public string Phone { get; set; }
    }
}