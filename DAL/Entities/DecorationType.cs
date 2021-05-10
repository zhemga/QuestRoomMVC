using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class DecorationType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(64)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public ICollection<QuestRoom> QuestRoom { get; set; }
    }
}
