namespace Swos.Database.Models;

public sealed class TeamDatabase
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Version { get; set; }
    public ICollection<DbSwosTeam> Teams { get; set; } = [];
}
