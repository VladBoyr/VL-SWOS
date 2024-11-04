using Swos.Domain.Models;

namespace Swos.Database.Models;

public sealed class DbSwosSkill
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public SwosSkill Skill { get; set; }
    public byte SkillValue { get; set; }
    public bool PrimarySkill { get; set; }
}
