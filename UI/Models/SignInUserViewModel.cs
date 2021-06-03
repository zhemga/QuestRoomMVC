using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class SignInUserViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}