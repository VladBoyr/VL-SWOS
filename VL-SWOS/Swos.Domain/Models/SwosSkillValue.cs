using System.Text;

namespace Swos.Domain.Models;

public class SwosSkillValue
{
    public byte SkillValue { get; set; }
    public bool PrimarySkill { get; set; }

    public SwosSkillValue(byte skillValue, bool primarySkill)
    {
        SkillValue = skillValue;
        PrimarySkill = primarySkill;
    }

    public byte ToByte()
    {
        return (byte)(SkillValue + 8 * (PrimarySkill ? 1 : 0));
    }
}

public static class SwosSkillValueExtension
{
    public static string ToFormatString(this Dictionary<SwosSkill, SwosSkillValue> skills)
    {
        var sb = new StringBuilder();
        foreach (var (skill, skillValue) in skills.OrderBy(x => x.Key))
        {
            sb.Append(skill.ToChar());
            sb.Append('=');
            sb.Append(skillValue.SkillValue);
            sb.Append(skillValue.PrimarySkill ? '+' : ' ');
            sb.Append(' ');
        }
        return sb.ToString();
    }
}
