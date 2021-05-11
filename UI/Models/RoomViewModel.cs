using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
        [Required]
        [MaxLength(2048)]
        public string Description { get; set; }
        [Required]
        public string PassingTime { get; set; }
        [Required]
        [Range(1, 10)]
        public int MinPlayers { get; set; }
        [Required]
        [Range(1, 100)]
        public int MinAge { get; set; }
        [Required]
        public string DifficultyLevel { get; set; }
        [Required]
        public string HorrorLevel { get; set; }
        [Required]
        [MaxLength(512)]
        public string Address { get; set; }
        [Required]
        [MaxLength(256)]
        public string CompanyName { get; set; }
        [Required]
        [Range(1, 9999)]
        public int Price { get; set; }
        public string[] ImagesUrl { get; set; }
        [Required]
        public string ImagesUrlForForm { get; set; }
        [Required]
        [Range(0, 5)]
        public double Rating { get; set; }
        [Required]
        public string DecorationType { get; set; }
    }

    public enum DifficultyLevel
    {
        Easy,
        Beginner,
        Normal,
        Hard,
        Professional,
    };

    public enum HorrorLevel
    {
        Peaceful,
        Strange,
        Normal,
        Scary,
        Horror,
    };
}