using Swos.Domain.Models;

namespace Swos.CareerMod.Database.Models;

public class TeamKit
{
    public int Id { get; set; }
    public SwosKitType KitType { get; set; }
    public SwosColor ShirtMainColor { get; set; }
    public SwosColor ShirtExtraColor { get; set; }
    public SwosColor ShortsColor { get; set; }
    public SwosColor SocksColor { get; set; }
}