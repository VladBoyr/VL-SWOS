using Swos.Domain.Models;

namespace Swos.CareerMod.Database.Models;

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public SwosCountry Country { get; set; }
    public int HomeKitId { get; set; }
    public TeamKit HomeKit { get; set; } = new();
    public int AwayKitId { get; set; }
    public TeamKit AwayKit { get; set; } = new();
    public int CoachId { get; set; }
    public Coach Coach { get; set; } = new();
    public ICollection<Player> Players { get; set; } = [];
    public int TeamRating { get; set; }
}