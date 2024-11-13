using Microsoft.EntityFrameworkCore;
using Swos.Database.Models;

namespace Swos.Database.Repositories;

public interface IGlobalPlayerRepository
{
    void Add(GlobalPlayer player);
    Task<GlobalPlayer?> FindEmpty();
    Task<GlobalPlayer[]> GetAllGlobalPlayers();
    Task<GlobalTeam[]> GetAllGlobalTeamPlayers();
    Task<HashSet<int>> GetAllGlobalPlayersSwosIds();
}

public sealed class GlobalPlayerRepository(ISwosDbContext context) : IGlobalPlayerRepository
{
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

    public Task<GlobalPlayer[]> GetAllGlobalPlayers()
    {
        return context.GlobalPlayers
            .Include(x => x.SwosPlayers)
            .ThenInclude(x => x.SwosPlayer)
            .ToArrayAsync();
    }

    public Task<GlobalTeam[]> GetAllGlobalTeamPlayers()
    {
        return context.GlobalTeams
            .Include(x => x.SwosTeams)
            .ThenInclude(x => x.SwosTeam)
            .ThenInclude(x => x.Players)
            .ToArrayAsync();
    }

    public async Task<HashSet<int>> GetAllGlobalPlayersSwosIds()
    {
        return (await context
                .GlobalPlayerSwos
                .Select(x => x.SwosPlayerId)
                .ToArrayAsync())
            .ToHashSet();
    }
}
