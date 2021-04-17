using DAL.Entities;
using System.Data.Entity;

namespace DAL
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("name=ApplicationContext")
        {
            Database.SetInitializer(new RoomInitializer());
        }

        public DbSet<QuestRoom> QuestRooms { get; set; }
        public DbSet<DecorationType> Types { get; set; }
        public DbSet<Company> Companies { get; set; }
    }
}
