using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class OrderContainer
    {
        [Key]
        public int Id { get; set; }
        public string Phone { get; set; }
        [Required]
        public bool NotRegisteredUser { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime DateTime { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
