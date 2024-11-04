namespace Swos.Database.Models;

public sealed class DbSwosTeamPlayer
{
    public int Id { get; set; }
    public int TeamId { get; set; }
    public int PlayerId { get; set; }
    public DbSwosPlayer Player { get; set; } = new();
    public byte PlayerPositionIndex { get; set; }
}
