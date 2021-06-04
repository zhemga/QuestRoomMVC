using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(256)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        [Required]
        [MaxLength(128)]
        [Phone]
        public string Phone { get; set; }

        public virtual ICollection<QuestRoom> QuestRoom { get; set; }
    }
}
