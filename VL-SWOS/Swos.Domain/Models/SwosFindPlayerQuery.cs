using Common.OptionalExtension;

namespace Swos.Domain.Models;

public class SwosFindPlayerQuery
{
    public SwosFileType[] FileTypes { get; set; } = [];
    public Optional<byte[]> Numbers { get; set; }
    public Optional<byte> MinNumber { get; set; }
    public Optional<byte> MaxNumber { get; set; }
    public Optional<string> NameLike { get; set; }
    public Optional<SwosCountry[]> Countries { get; set; }
    public Optional<SwosFace[]> Faces { get; set; }
    public Optional<SwosPosition[]> Positions { get; set; }
    public Optional<Dictionary<SwosSkill, byte>> MinSkills { get; set; }
    public Optional<Dictionary<SwosSkill, byte>> MaxSkills { get; set; }
    public Optional<SwosSkill[]> PrimarySkills { get; set; }
    public Optional<byte> MinRating { get; set; }
    public Optional<byte> MaxRating { get; set; }
}