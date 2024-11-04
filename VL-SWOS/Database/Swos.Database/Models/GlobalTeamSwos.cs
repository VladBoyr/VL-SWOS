namespace Swos.Database.Models;

public sealed class GlobalTeamSwos
{
    public int Id { get; set; }
    public int GlobalTeamId { get; set; }
    public int SwosTeamId { get; set; }
    public DbSwosTeam SwosTeam { get; set; } = new();
}
