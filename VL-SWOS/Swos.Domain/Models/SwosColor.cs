using System.Drawing;

namespace Swos.Domain.Models;

public enum SwosColor : byte
{
    Gray = 0,
    White = 1,
    Black = 2,
    Orange = 3,
    Red = 4,
    Blue = 5,
    Brown = 6,
    LightBlue = 7,
    Green = 8,
    Yellow = 9
}

public static class SwosColorExtension
{
    public static Color ToColor(this SwosColor color)
    {
        return color switch
        {
            SwosColor.Gray => Color.FromArgb(255, 182, 182, 182),
            SwosColor.White => Color.FromArgb(255, 255, 255, 255),
            SwosColor.Black => Color.FromArgb(255, 0, 0, 0),
            SwosColor.Orange => Color.FromArgb(255, 255, 109, 0),
            SwosColor.Red => Color.FromArgb(255, 255, 0, 0),
            SwosColor.Blue => Color.FromArgb(255, 0, 0, 255),
            SwosColor.Brown => Color.FromArgb(255, 109, 0, 36),
            SwosColor.LightBlue => Color.FromArgb(255, 146, 146, 255),
            SwosColor.Green => Color.FromArgb(255, 36, 146, 0),
            SwosColor.Yellow => Color.FromArgb(255, 255, 255, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
    }

    public static string ToRussianString(this SwosColor color)
    {
        return color switch
        {
            SwosColor.Gray => "Серый",
            SwosColor.White => "Белый",
            SwosColor.Black => "Чёрный",
            SwosColor.Orange => "Оранжевый",
            SwosColor.Red => "Красный",
            SwosColor.Blue => "Синий",
            SwosColor.Brown => "Коричневый",
            SwosColor.LightBlue => "Голубой",
            SwosColor.Green => "Зелёный",
            SwosColor.Yellow => "Жёлтый",
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
    }
}