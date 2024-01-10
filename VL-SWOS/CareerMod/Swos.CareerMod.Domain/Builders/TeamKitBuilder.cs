using Swos.CareerMod.Database.Models;
using Swos.Domain.Models;

namespace Swos.CareerMod.Domain.Builders;

public interface ITeamKitBuilder
{
    TeamKit BuildFromOriginal(SwosKit originalKit);
}

public class TeamKitBuilder : ITeamKitBuilder
{
    public TeamKit BuildFromOriginal(SwosKit originalKit)
    {
        return new TeamKit
        {
            KitType = originalKit.KitType,
            ShirtMainColor = originalKit.ShirtMainColor,
            ShirtExtraColor = originalKit.ShirtExtraColor,
            ShortsColor = originalKit.ShortsColor,
            SocksColor = originalKit.SocksColor
        };
    }
}