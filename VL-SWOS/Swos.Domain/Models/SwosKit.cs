namespace Swos.Domain.Models;

public class SwosKit
{
    public const int SwosSize = 5;

    public SwosKitType KitType { get; set; }
    public SwosColor ShirtMainColor { get; set; }
    public SwosColor ShirtExtraColor { get; set; }
    public SwosColor ShortsColor { get; set; }
    public SwosColor SocksColor { get; set; }

    public override string ToString()
    {
        return $"KitType: {KitType}, Shirt: {ShirtMainColor} / {ShirtExtraColor}, Shorts: {ShortsColor}, Socks: {SocksColor}";
    }
}