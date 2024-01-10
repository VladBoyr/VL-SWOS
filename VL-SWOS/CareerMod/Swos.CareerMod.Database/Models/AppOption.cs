namespace Swos.CareerMod.Database.Models;

public class AppOption
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Value { get; set; } = "";

    public static class Names
    {
        public const string SwosPath = "SwosPath";
    }
}