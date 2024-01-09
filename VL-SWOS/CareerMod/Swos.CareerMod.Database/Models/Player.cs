using Swos.Domain.Models;

namespace Swos.CareerMod.Database.Models;

public class Player
{
    public int Id { get; set; }
    public byte Number { get; set; }
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public byte Age { get; set; }
    public SwosCountry Country { get; set; }
    public SwosFace Face { get; set; }
    public SwosPosition Position { get; set; }
    public ICollection<PlayerSkill> Skills { get; set; } = [];
    public byte YouthRating { get; set; }
    public byte Rating { get; set; }
    public int Price => Rating.ToPrice();
    public int BonusRating { get; set; }
    public Dictionary<SwosSkill, PlayerSkill> SkillsDictionary => Skills
        .GroupBy(x => x.Skill)
        .ToDictionary(x => x.Key, x => x.Single());
}