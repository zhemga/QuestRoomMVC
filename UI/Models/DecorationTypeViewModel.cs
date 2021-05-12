using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class DecorationTypeViewModel
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
    }
}