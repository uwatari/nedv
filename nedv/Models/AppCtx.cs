using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using nedv.Models.Data;

namespace nedv.Models
{
    public class AppCtx : IdentityDbContext<User>
    {
        public AppCtx(DbContextOptions<AppCtx> options): base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<AdType> AdTypes { get; set; }
        public DbSet<TypeOfConstruction> TypeOfConstructions { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
    }
}
