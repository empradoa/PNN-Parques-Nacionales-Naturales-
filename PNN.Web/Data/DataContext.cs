using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PNN.web.Data.Entities;
using PNN.Web.Data.Entities;

namespace PNN.web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        //modelo mapeado de owner pero en plural owners
        public DbSet<Owner> Owners { get; set; }
        public DbSet<ZoneType> ZoneTypes { get; set; }
        public DbSet<ContentType> ContentTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Park> Parks { get; set; }
        public DbSet<Manager> Managers { get; set; }

    }
}
