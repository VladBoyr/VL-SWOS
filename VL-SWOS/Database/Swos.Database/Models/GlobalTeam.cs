namespace Swos.Database.Models;

public sealed class GlobalTeam
{
    public int Id { get; set; }
    public ICollection<GlobalTeamSwos> SwosTeams { get; set; } = [];
}
