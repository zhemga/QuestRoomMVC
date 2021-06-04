using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class EditUserViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [MaxLength(128)]
        [CustomPhone]
        public string Phone { get; set; }
    }
}