using Swos.Domain.Models;

namespace Swos.CareerMod.Domain.Helpers;

public static class AgeHelper
{
    public static int PositionAgeBonus(SwosPosition playerPosition)
    {
        return playerPosition switch
        {
            SwosPosition.G => 0,
            SwosPosition.RB => -1,
            SwosPosition.LB => -1,
            SwosPosition.D => -1,
            SwosPosition.RW => -2,
            SwosPosition.LW => -2,
            SwosPosition.M => -2,
            SwosPosition.A => -3,
            _ => throw new ArgumentOutOfRangeException(nameof(playerPosition), playerPosition, null)
        };
    }
}