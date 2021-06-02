using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class OrderContainer
    {
        [Required]
        public int Id { get; set; }
        public string Phone { get; set; }
        public bool NotRegisteredUser { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public ICollection<Order> Order { get; set; }
    }
}
