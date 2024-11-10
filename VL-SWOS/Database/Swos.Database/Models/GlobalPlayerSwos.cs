namespace Swos.Database.Models;

public sealed class GlobalPlayerSwos
{
    public int Id { get; set; }
    public int GlobalPlayerId { get; set; }
    public int SwosPlayerId { get; set; }
    public DbSwosPlayer SwosPlayer { get; set; } = new();
}
