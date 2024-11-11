using Microsoft.EntityFrameworkCore;
using Swos.Database.Models;
using Swos.Domain.Models;

namespace Swos.Database.Repositories;

public interface IGlobalPlayerRepository
{
    void Add(GlobalPlayer player);
    Task<GlobalPlayer?> FindEmpty();
    Task<GlobalPlayer[]> FindGlobalPlayersExactly(string playerName, DbSwosTeam globalTeam);
    Task<GlobalPlayerSwos[]> GetAllGlobalPlayersSwos();
}

public sealed class GlobalPlayerRepository(ISwosDbContext context) : IGlobalPlayerRepository
{
    private readonly ISwosDbContext context = context;

    public void Add(GlobalPlayer player)
    {
        context.GlobalPlayers.Add(player);
    }

    public Task<GlobalPlayer?> FindEmpty()
    {
        return context
            .GlobalPlayers
            .Where(x => x.SwosPlayers.Count == 0)
            .FirstOrDefaultAsync();
    }

    public async Task<GlobalPlayer[]> FindGlobalPlayersExactly(string playerName, DbSwosTeam swosTeam)
    {
        var playerIds = await context.GlobalTeams
            .Include(x => x.SwosTeams)
            .ThenInclude(x => x.SwosTeam)
            .ThenInclude(x => x.Players)
            .ThenInclude(x => x.Player)
            .Where(x => x
                .SwosTeams
                .Any(t => t.SwosTeam.Id == swosTeam.Id))
            .SelectMany(x => x.SwosTeams)
            .SelectMany(x => x.SwosTeam.Players)
            .Where(x => x.Player.Name == playerName)
            .Select(x => x.PlayerId)
            .ToArrayAsync();

        return await context.GlobalPlayers
            .Include(x => x.SwosPlayers)
            .ThenInclude(x => x.SwosPlayer)
            .Where(x => x
                .SwosPlayers
                .Any(p => playerIds.Contains(p.SwosPlayer.Id)))
            .ToArrayAsync();
    }

    public Task<GlobalPlayerSwos[]> GetAllGlobalPlayersSwos()
    {
        return context
            .GlobalPlayerSwos
            .ToArrayAsync();
    }
}
