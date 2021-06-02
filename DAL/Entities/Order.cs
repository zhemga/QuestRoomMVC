using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Order
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int QuestRoomId { get; set; }
        [Required]
        public virtual QuestRoom QuestRoom { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public int OrderContainerId { get; set; }
        [Required]
        public virtual OrderContainer OrderContainer { get; set; }
    }
}
