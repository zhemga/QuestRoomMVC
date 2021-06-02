using DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace DAL
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext() : base("name=ApplicationContext")
        {
            Database.SetInitializer(new RoomInitializer());
        }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }

        public DbSet<QuestRoom> QuestRooms { get; set; }
        public DbSet<DecorationType> Types { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderContainer> OrderContainers { get; set; }
    }
}
