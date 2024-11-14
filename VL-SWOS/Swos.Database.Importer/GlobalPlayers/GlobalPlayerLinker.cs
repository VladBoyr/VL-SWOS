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
    Task<bool> LinksPlayer(DbSwosTeamPlayer player, GlobalPlayer[] globalPlayers, int globalPlayerId);
    Task<DbSwosTeamPlayer[]> PlayersToLink();
    Task<GlobalPlayer[]> FindGlobalPlayers(string playerName, DbSwosTeam playerTeam);
    Task<GlobalPlayer[]> FindGlobalPlayersByText(string text);
    Task MergeGlobalPlayers(GlobalPlayer[] globalPlayers, string? mergeQuery);
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

                logger.LogInformation($"{player.Player.Id}. {player.Player.Name} ({player.Player.Country}), " +
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

    public async Task<bool> LinksPlayer(DbSwosTeamPlayer player, GlobalPlayer[] globalPlayers, int globalPlayerId)
    {
        GlobalPlayer? globalPlayer;

        if (globalPlayerId == 0)
        {
            globalPlayer = await globalPlayerRepository.FindEmpty();
            if (globalPlayer == null)
            {
                globalPlayer = new GlobalPlayer();
                globalPlayerRepository.Add(globalPlayer);
            }
        }
        else
        {
            globalPlayer = globalPlayers.SingleOrDefault(x => x.Id == globalPlayerId);
            if (globalPlayer == null)
                return false;
        }

        globalPlayer.SwosPlayers.Add(
            new GlobalPlayerSwos
            {
                SwosPlayerId = player.Id,
                SwosPlayer = player.Player
            });
        await unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<DbSwosTeamPlayer[]> PlayersToLink()
    {
        var existGlobalPlayers = await globalPlayerRepository.GetAllGlobalPlayersSwosIds();
        var teamDatabases = await teamDatabaseRepository.GetTeamDatabases(TeamDatabaseDlo.Players);

        var teamPlayers = new List<DbSwosTeamPlayer>();

        foreach (var teamDatabase in teamDatabases.OrderByDescending(x =>
                     x.Teams.Count(t => !existGlobalPlayers.Contains(t.Id))))
        {
            teamPlayers.AddRange(teamDatabase.Teams
                .SelectMany(x => x.Players)
                .Where(x => !existGlobalPlayers.Contains(x.PlayerId)));
        }

        return [.. teamPlayers];
    }

    public Task<GlobalPlayer[]> FindGlobalPlayers(string playerName, DbSwosTeam playerTeam)
    {
        return globalPlayerRepository.FindGlobalPlayers(playerName, playerTeam);
    }

    public Task<GlobalPlayer[]> FindGlobalPlayersByText(string text)
    {
        return globalPlayerRepository.FindGlobalPlayersByText(text);
    }

    public async Task MergeGlobalPlayers(GlobalPlayer[] globalPlayers, string? mergeQuery)
    {
        var mergeIds = mergeQuery?.Split(' ');
        if (mergeIds?.Length == 2 &&
            int.TryParse(mergeIds[0], out var fromMergeId) &&
            int.TryParse(mergeIds[1], out var toMergeId))
        {
            var fromGlobalPlayer = globalPlayers.SingleOrDefault(x => x.Id == fromMergeId);
            var toGlobalPlayer = globalPlayers.SingleOrDefault(x => x.Id == toMergeId);

            if (fromGlobalPlayer == null || toGlobalPlayer == null)
                return;

            foreach (var fromPlayer in fromGlobalPlayer.SwosPlayers)
            {
                toGlobalPlayer.SwosPlayers.Add(
                    new GlobalPlayerSwos
                    {
                        SwosPlayerId = fromPlayer.SwosPlayer.Id,
                        SwosPlayer = fromPlayer.SwosPlayer
                    });
            }

            fromGlobalPlayer.SwosPlayers.Clear();
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
