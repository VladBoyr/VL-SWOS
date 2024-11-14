using Swos.Domain.Models;

namespace Swos.Database.Models;

public sealed class DbSwosPlayer
{
    public int Id { get; set; }
    public byte Number { get; set; }
    public string Name { get; set; } = "";
    public SwosCountry Country { get; set; }
    public SwosFace Face { get; set; }
    public SwosPosition Position { get; set; }
    public ICollection<DbSwosSkill> Skills { get; set; } = [];
    public byte Rating { get; set; }
    public ICollection<DbSwosTeamPlayer> Teams { get; set; } = [];
}
