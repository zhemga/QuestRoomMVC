using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Count { get; set; }
        public int QuestRoomId { get; set; }
        public virtual QuestRoom QuestRoom { get; set; }
        public int OrderContainerId { get; set; }
        public virtual OrderContainer OrderContainer { get; set; }
    }
}
