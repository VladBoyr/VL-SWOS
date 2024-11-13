using Common.Database;
using Common.Logging;
using Microsoft.Extensions.Logging;
using Swos.Database.Models;
using Swos.Database.Repositories;

namespace Swos.Database.Importer.GlobalPlayers;

public interface IGlobalPlayerLinker
{
    Task<(TeamDatabase, int)[]> GlobalPlayerStats();
    Task LinksAutomate();
}

public sealed class GlobalPlayerLinker(
    IGlobalPlayerRepository globalPlayerRepository,
    ITeamDatabaseRepository teamDatabaseRepository,
    IUnitOfWork unitOfWork) : IGlobalPlayerLinker
{
    private readonly ILogger<GlobalPlayerLinker> logger = LogProvider.Create<GlobalPlayerLinker>();

    public async Task<(TeamDatabase, int)[]> GlobalPlayerStats()
    {
        var existGlobalPlayers = await globalPlayerRepository.GetAllGlobalPlayersSwosIds();
        var teamDatabases = await teamDatabaseRepository.GetTeamDatabases(TeamDatabaseDlo.TeamPlayers);

        var result = new List<(TeamDatabase, int)>();
        foreach (var teamDatabase in teamDatabases)
        {
            var globalPlayersCount = teamDatabase.Teams
                .SelectMany(x => x.Players
                    .Where(p => existGlobalPlayers.Contains(p.Id)))
                .Count();
            result.Add((teamDatabase, globalPlayersCount));
        }

        return [.. result];
    }

    public async Task LinksAutomate()
    {
        var existGlobalPlayerIds = await globalPlayerRepository.GetAllGlobalPlayersSwosIds();
        var teamDatabases = await teamDatabaseRepository.GetTeamDatabases(TeamDatabaseDlo.Players);

        var teamToPlayerIds = (await globalPlayerRepository.GetAllGlobalTeamPlayers())
            .SelectMany(x => x.SwosTeams.Select(swos => new { GlobalTeam = x, TeamId = swos.SwosTeamId }))
            .Distinct()
            .GroupBy(x => x.TeamId)
            .ToDictionary(g => g.Key,
                g => g.SelectMany(x => x.GlobalTeam.SwosTeams)
                    .SelectMany(x => x.SwosTeam.Players)
                    .Select(x => x.PlayerId)
                    .Distinct()
                    .ToArray());

        var existGlobalPlayers = (await globalPlayerRepository.GetAllGlobalPlayers())
            .SelectMany(x => x.SwosPlayers.Select(swos => new
            {
                GlobalPlayer = x,
                PlayerId = swos.SwosPlayerId,
                PlayerName = swos.SwosPlayer.Name
            }))
            .Distinct()
            .GroupBy(x => x.PlayerId)
            .ToDictionary(g => g.Key,
                g => g.Select(x => (x.PlayerName, x.GlobalPlayer))
                    .ToArray());

        var existGlobalPlayersFinal = teamToPlayerIds
            .ToDictionary(g => g.Key,
                g => g.Value
                    .SelectMany(x => existGlobalPlayers
                        .TryGetValue(x, out var result) ? result : [])
                    .GroupBy(x => x.PlayerName)
                    .ToDictionary(grp => grp.Key,
                        grp => grp.Select(x => x.GlobalPlayer)
                            .Distinct()
                            .ToArray()));
        
        foreach (var teamDatabase in teamDatabases)
        {
            logger.LogInformation($"Team Database: {teamDatabase.Title}");

            foreach (var player in teamDatabase.Teams.SelectMany(x => x.Players)
                         .Where(x => !existGlobalPlayerIds.Contains(x.Id)))
            {
                var globalPlayer = FindGlobalPlayerExactly(
                    existGlobalPlayersFinal,
                    player.Player.Name,
                    player.Team);

                if (globalPlayer == null)
                    continue;

                logger.LogInformation($"{player.Id}. {player.Player.Name} ({player.Player.Country}), " +
                                      $"{player.Team.Name} ({player.Team.Country}) => {globalPlayer.Id}");

                globalPlayer.SwosPlayers.Add(
                    new GlobalPlayerSwos
                    {
                        SwosPlayerId = player.Player.Id,
                        SwosPlayer = player.Player
                    });
            }
            
            await unitOfWork.SaveChangesAsync();
        }
    }

    private static GlobalPlayer? FindGlobalPlayerExactly(
        Dictionary<int, Dictionary<string, GlobalPlayer[]>> existGlobalPlayersFinal,
        string playerName,
        DbSwosTeam swosTeam)
    {
        if (!existGlobalPlayersFinal.TryGetValue(swosTeam.Id, out var swosPlayers))
        {
            return null;
        }

        if (!swosPlayers.TryGetValue(playerName, out var globalPlayers))
        {
            return null;
        }

        if (globalPlayers.Length == 0 || globalPlayers.Select(x => x.Id).Distinct().Count() != 1)
            return null;

        return globalPlayers.First();
    }
}
