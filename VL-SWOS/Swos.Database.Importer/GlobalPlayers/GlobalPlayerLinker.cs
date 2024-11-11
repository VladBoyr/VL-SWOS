using Swos.Database.Models;
using Swos.Database.Repositories;

namespace Swos.Database.Importer;

public interface IGlobalPlayerLinker
{
    Task<(TeamDatabase, int)[]> GlobalPlayerStats();
}

public sealed class GlobalPlayerLinker(
    IGlobalPlayerRepository globalPlayerRepository,
    ITeamDatabaseRepository teamDatabaseRepository) : IGlobalPlayerLinker
{
    private readonly IGlobalPlayerRepository globalPlayerRepository = globalPlayerRepository;
    private readonly ITeamDatabaseRepository teamDatabaseRepository = teamDatabaseRepository;

    public async Task<(TeamDatabase, int)[]> GlobalPlayerStats()
    {
        var existGlobalPlayers = (await globalPlayerRepository.GetAllGlobalPlayersSwos()).Select(x => x.SwosPlayerId).ToHashSet();
        var teamDatabases = await teamDatabaseRepository.GetTeamDatabases(TeamDatabaseDlo.Players);

        var result = new List<(TeamDatabase, int)>();
        foreach (var teamDatabase in teamDatabases)
        {
            var globalPlayersCount = teamDatabase.Teams.SelectMany(x => x.Players.Where(p => existGlobalPlayers.Contains(p.Id))).Count();
            result.Add((teamDatabase, globalPlayersCount));
        }

        return [.. result];
    }
}
