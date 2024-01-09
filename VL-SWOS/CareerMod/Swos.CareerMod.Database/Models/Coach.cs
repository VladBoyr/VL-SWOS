using Swos.Domain.Models;

namespace Swos.CareerMod.Database.Models;

public class Coach
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public byte Age { get; set; }
    public SwosCountry Country { get; set; }
    public byte Rating { get; set; }
    public SwosTactic Tactic { get; set; }
}