using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class User: IdentityUser
    {
        public ICollection<OrderContainer> OrderContainer { get; set; }
    }
}
