namespace Swos.Database.Models;

public sealed class GlobalPlayer
{
    public int Id { get; set; }
    public ICollection<GlobalPlayerSwos> SwosPlayers { get; set; } = [];
}
