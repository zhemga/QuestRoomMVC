using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
        [Required]
        [MaxLength(128)]
        [Phone]
        public string Phone { get; set; }
    }
}
