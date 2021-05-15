using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class QuestRoom
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        [Required]
        [MaxLength(2048)]
        public string Description { get; set; }
        [Required]
        public DateTime PassingTime { get; set; }
        [Required]
        [Range(1, 10)]
        public int MinPlayers { get; set; }
        [Required]
        [Range(1, 100)]
        public int MinAge { get; set; }
        [Required]
        [Range(1,5)]
        public int DifficultyLevel { get; set; }
        [Required]
        [Range(1, 5)]
        public int HorrorLevel { get; set; }
        [Required]
        [Range(1, 9999)]
        public int Price { get; set; }
        [Required]
        [MaxLength(512)]
        public string Address { get; set; }
        [Required]
        public string ImagesUrl { get; set; }
        [Required]
        [Range(0,5)]
        public double Rating { get; set; }

        public int DecorationTypeId { get; set; }
        public virtual DecorationType DecorationType { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
