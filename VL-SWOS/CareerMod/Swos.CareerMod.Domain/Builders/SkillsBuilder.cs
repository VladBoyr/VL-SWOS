using Swos.CareerMod.Database.Models;
using Swos.Domain.Models;

namespace Swos.CareerMod.Domain.Builders;

public interface ISkillsBuilder
{
    IEnumerable<PlayerSkill> BuildFromOriginal(Dictionary<SwosSkill, SwosSkillValue> originalSkills);
}

public class SkillsBuilder : ISkillsBuilder
{
    public IEnumerable<PlayerSkill> BuildFromOriginal(Dictionary<SwosSkill, SwosSkillValue> originalSkills)
    {
        return originalSkills.Select(x => new PlayerSkill
        {
            Skill = x.Key,
            SkillValue = x.Value.SkillValue,
            PrimarySkill = x.Value.PrimarySkill
        });
    }
}