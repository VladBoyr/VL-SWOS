namespace Swos.Domain.Models;

public class SwosTeam
{
    public const int HeaderSize = 2 * SwosKit.SwosSize + MaxNameLength + MaxCoachNameLength + PlayersCount + 8;
    public const int SwosSize = HeaderSize + PlayersCount * SwosPlayer.SwosSize;
    public const int PlayersCount = 16;
    public const int MaxNameLength = 18;
    public const int MaxCoachNameLength = 24;

    public int GlobalId { get; set; }
    public int LocalId { get; set; }
    public string Name { get; set; } = "";
    public SwosCountry Country { get; set; }
    public byte Division { get; set; }
    public SwosKit HomeKit { get; set; } = new();
    public SwosKit AwayKit { get; set; } = new();
    public string CoachName { get; set; } = "";
    public SwosTactic Tactic { get; set; }
    public byte[] PlayerPositions { get; set; } = new byte[PlayersCount];
    public SwosPlayer[] Players { get; set; } = new SwosPlayer[PlayersCount];
}