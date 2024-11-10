using Common.Database;
using Microsoft.EntityFrameworkCore;
using Swos.Database.Models;

namespace Swos.Database;

public interface ISwosDbContext : IDbContext
{
    DbSet<TeamDatabase> TeamDatabases { get; set; }
    DbSet<DbSwosTeam> Teams { get; set; }
    DbSet<DbSwosKit> TeamKits { get; set; }
    DbSet<DbSwosTeamPlayer> TeamPlayers { get; set; }
    DbSet<DbSwosPlayer> Players { get; set; }
    DbSet<DbSwosSkill> Skills { get; set; }
    DbSet<GlobalTeam> GlobalTeams { get; set; }
    DbSet<GlobalTeamSwos> GlobalTeamSwos { get; set; }
    DbSet<GlobalPlayer> GlobalPlayers { get; set; }
    DbSet<GlobalPlayerSwos> GlobalPlayerSwos { get; set; }
}

public abstract class SwosDbContext(DbContextOptions options) : CommonDbContext(options), ISwosDbContext
{
    public DbSet<TeamDatabase> TeamDatabases { get; set; }
    public DbSet<DbSwosTeam> Teams { get; set; }
    public DbSet<DbSwosKit> TeamKits { get; set; }
    public DbSet<DbSwosTeamPlayer> TeamPlayers { get; set; }
    public DbSet<DbSwosPlayer> Players { get; set; }
    public DbSet<DbSwosSkill> Skills { get; set; }
    public DbSet<GlobalTeam> GlobalTeams { get; set; }
    public DbSet<GlobalTeamSwos> GlobalTeamSwos { get; set; }
    public DbSet<GlobalPlayer> GlobalPlayers { get; set; }
    public DbSet<GlobalPlayerSwos> GlobalPlayerSwos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TeamDatabase>()
            .HasMany(x => x.Teams)
            .WithOne(x => x.TeamDatabase)
            .HasForeignKey(x => x.TeamDatabaseId);

        modelBuilder.Entity<DbSwosTeam>()
            .HasOne(x => x.HomeKit)
            .WithMany()
            .HasForeignKey(x => x.HomeKitId);

        modelBuilder.Entity<DbSwosTeam>()
            .HasOne(x => x.AwayKit)
            .WithMany()
            .HasForeignKey(x => x.AwayKitId);

        modelBuilder.Entity<DbSwosTeam>()
            .HasMany(x => x.Players)
            .WithOne()
            .HasForeignKey(x => x.TeamId);

        modelBuilder.Entity<DbSwosTeamPlayer>()
            .HasOne(x => x.Player)
            .WithMany()
            .HasForeignKey(x => x.PlayerId);

        modelBuilder.Entity<DbSwosPlayer>()
            .HasMany(x => x.Skills)
            .WithOne()
            .HasForeignKey(x => x.PlayerId);

        modelBuilder.Entity<GlobalTeam>()
            .HasMany(x => x.SwosTeams)
            .WithOne()
            .HasForeignKey(x => x.GlobalTeamId);

        modelBuilder.Entity<GlobalTeamSwos>()
            .HasOne(x => x.SwosTeam)
            .WithMany()
            .HasForeignKey(x => x.SwosTeamId);

        modelBuilder.Entity<GlobalPlayer>()
            .HasMany(x => x.SwosPlayers)
            .WithOne()
            .HasForeignKey(x => x.GlobalPlayerId);

        modelBuilder.Entity<GlobalPlayerSwos>()
            .HasOne(x => x.SwosPlayer)
            .WithMany()
            .HasForeignKey(x => x.SwosPlayerId);

        base.OnModelCreating(modelBuilder);
    }
}
