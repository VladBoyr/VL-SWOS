using Microsoft.EntityFrameworkCore;
using Swos.Database.Models;
using Swos.Domain.Models;

namespace Swos.Database.Repositories;

public interface ITeamKitRepository
{
    void Add(DbSwosKit teamKit);
    Task<DbSwosKit?> FindTeamKit(
        SwosKitType kitType,
        SwosColor shirtMainColor,
        SwosColor shirtExtraColor,
        SwosColor shortsColor,
        SwosColor socksColor);
}

public sealed class TeamKitRepository(ISwosDbContext context) : ITeamKitRepository
{
    private readonly ISwosDbContext context = context;

    public void Add(DbSwosKit teamKit)
    {
        context.TeamKits.Add(teamKit);
    }

    public Task<DbSwosKit?> FindTeamKit(
        SwosKitType kitType,
        SwosColor shirtMainColor,
        SwosColor shirtExtraColor,
        SwosColor shortsColor,
        SwosColor socksColor)
    {
        return context.TeamKits
            .Where(x => x.KitType == kitType && 
                        x.ShirtMainColor == shirtMainColor &&
                        x.ShirtExtraColor == shirtExtraColor &&
                        x.ShortsColor == shortsColor &&
                        x.SocksColor == socksColor)
            .SingleOrDefaultAsync();
    }
}
