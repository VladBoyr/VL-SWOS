using Swos.Domain.Models;

namespace Swos.Database.Models;

public sealed class DbSwosKit
{
    public int Id { get; set; }
    public SwosKitType KitType { get; set; }
    public SwosColor ShirtMainColor { get; set; }
    public SwosColor ShirtExtraColor { get; set; }
    public SwosColor ShortsColor { get; set; }
    public SwosColor SocksColor { get; set; }
}
