using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class DecorationType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
    }
}
