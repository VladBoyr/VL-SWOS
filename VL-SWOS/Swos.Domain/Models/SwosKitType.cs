namespace Swos.Domain.Models;

public enum SwosKitType : byte
{
    Solid = 0,
    Sleeves = 1,
    VerticalStripes = 2,
    HorizontalStripes = 3
}

public static class SwosKitTypeExtension
{
    public static string ToRussianString(this SwosKitType kitType)
    {
        return kitType switch
        {
            SwosKitType.Solid => "Сплошная форма",
            SwosKitType.Sleeves => "Рукава",
            SwosKitType.VerticalStripes => "Вертикальные полосы",
            SwosKitType.HorizontalStripes => "Горизонтальные полосы",
            _ => throw new ArgumentOutOfRangeException(nameof(kitType), kitType, null)
        };
    }
}