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