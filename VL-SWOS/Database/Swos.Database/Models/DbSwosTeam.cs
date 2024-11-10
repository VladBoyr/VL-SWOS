using Swos.Domain.Models;

namespace Swos.Database.Models;

public sealed class DbSwosTeam
{
    public int Id { get; set; }
    public int TeamDatabaseId { get; set; }
    public TeamDatabase TeamDatabase { get; set; } = null!;
    public int GlobalId { get; set; }
    public int LocalId { get; set; }
    public string FileName { get; set; } = "";
    public string Name { get; set; } = "";
    public SwosCountry Country { get; set; }
    public byte Division { get; set; }
    public int HomeKitId { get; set; }
    public DbSwosKit HomeKit { get; set; } = new();
    public int AwayKitId { get; set; }
    public DbSwosKit AwayKit { get; set; } = new();
    public string CoachName { get; set; } = "";
    public SwosTactic Tactic { get; set; }
    public ICollection<DbSwosTeamPlayer> Players { get; set; } = [];
}
