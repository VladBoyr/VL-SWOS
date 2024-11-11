namespace Swos.Database.Models;

public sealed class DbSwosTeamPlayer
{
    public int Id { get; set; }
    public int TeamId { get; set; }
    public DbSwosTeam Team { get; set; } = new();
    public int PlayerId { get; set; }
    public DbSwosPlayer Player { get; set; } = new();
    public byte PlayerPositionIndex { get; set; }
}
