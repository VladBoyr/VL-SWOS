using Microsoft.EntityFrameworkCore;
using Swos.Database.Models;

namespace Swos.Database.Repositories;

public interface IGlobalPlayerRepository
{
    void Add(GlobalPlayer player);
    Task<GlobalPlayer?> FindEmpty();
    Task<GlobalPlayer[]> FindGlobalPlayers(string playerName, DbSwosTeam playerTeam);
    Task<GlobalPlayer[]> FindGlobalPlayersByText(string text);
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

    public async Task<GlobalPlayer[]> FindGlobalPlayers(string playerName, DbSwosTeam playerTeam)
    {
        var teamIds = (await context.GlobalTeams
                .Include(x => x.SwosTeams)
                .ThenInclude(x => x.SwosTeam)
                .Where(x => x.SwosTeams
                    .Any(t => t.SwosTeamId == playerTeam.Id))
                .ToArrayAsync())
            .SelectMany(x => x.SwosTeams)
            .Select(x => x.SwosTeamId)
            .Distinct()
            .ToArray();

        var firstFound = await context
            .GlobalPlayers
            .Include(x => x.SwosPlayers)
            .ThenInclude(x => x.SwosPlayer)
            .ThenInclude(x => x.Teams)
            .ThenInclude(x => x.Team)
            .ThenInclude(x => x.TeamDatabase)
            .Where(x => x
                .SwosPlayers
                .Any(p => p.SwosPlayer.Name == playerName && 
                          teamIds.Contains(p.SwosPlayer.Id)))
            .ToArrayAsync();

        if (firstFound.Length > 0)
            return firstFound;

        var secondFound = await context
            .GlobalPlayers
            .Include(x => x.SwosPlayers)
            .ThenInclude(x => x.SwosPlayer)
            .ThenInclude(x => x.Teams)
            .ThenInclude(x => x.Team)
            .ThenInclude(x => x.TeamDatabase)
            .Where(x => x
                .SwosPlayers
                .Any(p => p.SwosPlayer.Name == playerName &&
                          p.SwosPlayer.Teams.Any(t => t.Team.Country == playerTeam.Country)))
            .ToArrayAsync();

        if (secondFound.Length > 0)
            return secondFound;

        var thirdResult = new List<GlobalPlayer>();
        var playerNames = playerName.Replace('.', ' ').Split(' ');
        foreach (var partName in playerNames.Where(x => x.Length >= 3))
        {
            var thirdFound = await context
                .GlobalPlayers
                .Include(x => x.SwosPlayers)
                .ThenInclude(x => x.SwosPlayer)
                .ThenInclude(x => x.Teams)
                .ThenInclude(x => x.Team)
                .ThenInclude(x => x.TeamDatabase)
                .Where(x => x
                                .SwosPlayers
                                .Any(p => p.SwosPlayer.Name.Contains(partName)) &&
                            !thirdResult.Select(exist => exist.Id).Contains(x.Id))
                .ToArrayAsync();
            thirdResult.AddRange(thirdFound);
        }

        return [.. thirdResult];
    }

    public async Task<GlobalPlayer[]> FindGlobalPlayersByText(string text)
    {
        var findText = text.ToUpper();

        return await context
            .GlobalPlayers
            .Include(x => x.SwosPlayers)
            .ThenInclude(x => x.SwosPlayer)
            .ThenInclude(x => x.Teams)
            .ThenInclude(x => x.Team)
            .ThenInclude(x => x.TeamDatabase)
            .Where(x => x
                .SwosPlayers
                .Any(t => t.SwosPlayer.Name.Contains(findText)))
            .ToArrayAsync();
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
