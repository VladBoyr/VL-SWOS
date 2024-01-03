using Common.OptionalExtension;

namespace Swos.Domain.Models;

public class SwosFindTeamQuery
{
    public SwosFileType[] FileTypes { get; set; } = [];
    public Optional<int[]> GlobalIds { get; set; }
    public Optional<int> MinGlobalId { get; set; }
    public Optional<int> MaxGlobalId { get; set; }
    public Optional<int[]> LocalIds { get; set; }
    public Optional<int> MinLocalId { get; set; }
    public Optional<int> MaxLocalId { get; set; }
    public Optional<string> NameLike { get; set; }
    public Optional<SwosCountry[]> Countries { get; set; }
    public Optional<byte[]> Divisions { get; set; }
    public Optional<SwosKitType[]> KitTypes { get; set; }
    public Optional<SwosColor[]> ShirtMainColors { get; set; }
    public Optional<SwosColor[]> ShirtExtraColors { get; set; }
    public Optional<SwosColor[]> ShortsColors { get; set; }
    public Optional<SwosColor[]> SocksColors { get; set; }
    public Optional<string> CoachNameLike { get; set; }
    public Optional<SwosTactic[]> Tactics { get; set; }
}