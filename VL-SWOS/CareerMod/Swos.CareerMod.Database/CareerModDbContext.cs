using Common.Database;
using Microsoft.EntityFrameworkCore;
using Swos.CareerMod.Database.Models;

namespace Swos.CareerMod.Database;

public interface ICareerModDbContext : IDbContext
{
    DbSet<Player> Players { get; set; }
    DbSet<PlayerSkill> PlayerSkills { get; set; }
}

public class CareerModDbContext : CommonDbContext, ICareerModDbContext
{
    public CareerModDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }
    public DbSet<PlayerSkill> PlayerSkills { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<Player>()
            .HasMany(x => x.Skills)
            .WithOne()
            .HasForeignKey(x => x.PlayerId);
    }
}