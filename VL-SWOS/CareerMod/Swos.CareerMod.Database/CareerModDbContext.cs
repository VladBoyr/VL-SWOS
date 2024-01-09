using Common.Database;
using Microsoft.EntityFrameworkCore;
using Swos.CareerMod.Database.Models;

namespace Swos.CareerMod.Database;

public interface ICareerModDbContext : IDbContext
{
    DbSet<Player> Players { get; set; }
    DbSet<PlayerSkill> PlayerSkills { get; set; }
    DbSet<Coach> Coaches { get; set; }
    DbSet<Team> Teams { get; set; }
    DbSet<TeamKit> TeamKits { get; set; }
    DbSet<AppOption> AppOptions { get; set; }
}

public class CareerModDbContext : CommonDbContext, ICareerModDbContext
{
    public CareerModDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }
    public DbSet<PlayerSkill> PlayerSkills { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamKit> TeamKits { get; set; }
    public DbSet<AppOption> AppOptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<Player>()
            .HasMany(x => x.Skills)
            .WithOne()
            .HasForeignKey(x => x.PlayerId);

        modelBuilder
            .Entity<Team>()
            .HasOne(x => x.HomeKit)
            .WithMany()
            .HasForeignKey(x => x.HomeKitId);

        modelBuilder
            .Entity<Team>()
            .HasOne(x => x.AwayKit)
            .WithMany()
            .HasForeignKey(x => x.AwayKitId);

        modelBuilder
            .Entity<Team>()
            .HasOne(x => x.Coach)
            .WithMany()
            .HasForeignKey(x => x.CoachId);

        modelBuilder
            .Entity<Team>()
            .HasMany(x => x.Players)
            .WithOne()
            .HasForeignKey(x => x.TeamId);
    }
}