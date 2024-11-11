using Common.Database;
using Common.Logging;
using Microsoft.Extensions.Logging;
using Swos.Database.Models;
using Swos.Database.Repositories;

namespace Swos.Database.Importer;

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
    private readonly IGlobalPlayerRepository globalPlayerRepository = globalPlayerRepository;
    private readonly ITeamDatabaseRepository teamDatabaseRepository = teamDatabaseRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<(TeamDatabase, int)[]> GlobalPlayerStats()
    {
        var existGlobalPlayers = (await globalPlayerRepository.GetAllGlobalPlayersSwos()).Select(x => x.SwosPlayerId).ToHashSet();
        var teamDatabases = await teamDatabaseRepository.GetTeamDatabases(TeamDatabaseDlo.TeamPlayers);

        var result = new List<(TeamDatabase, int)>();
        foreach (var teamDatabase in teamDatabases)
        {
            var globalPlayersCount = teamDatabase.Teams.SelectMany(x => x.Players.Where(p => existGlobalPlayers.Contains(p.Id))).Count();
            result.Add((teamDatabase, globalPlayersCount));
        }

        return [.. result];
    }

    public async Task LinksAutomate()
    {
        var existGlobalPlayers = (await globalPlayerRepository.GetAllGlobalPlayersSwos()).Select(x => x.SwosPlayerId).ToHashSet();
        var teamDatabases = await teamDatabaseRepository.GetTeamDatabases(TeamDatabaseDlo.Players);

        foreach (var teamDatabase in teamDatabases)
        {
            logger.LogInformation($"Team Database: {teamDatabase.Title}");

            foreach (var player in teamDatabase.Teams.SelectMany(x => x.Players).Where(x => !existGlobalPlayers.Contains(x.Id)))
            {
                var globalPlayers = await globalPlayerRepository.FindGlobalPlayersExactly(player.Player.Name, player.Team);

                if (globalPlayers.Length > 0 && globalPlayers.Select(x => x.Id).Distinct().Count() == 1)
                {
                    var globalPlayer = globalPlayers.First();

                    logger.LogInformation($"{player.Id}. {player.Player.Name} ({player.Player.Country}), {player.Team.Name} ({player.Team.Country}) => {globalPlayer.Id}");

                    globalPlayer.SwosPlayers.Add(
                        new GlobalPlayerSwos
                        {
                            SwosPlayerId = player.Player.Id,
                            SwosPlayer = player.Player
                        });
                    await unitOfWork.SaveChangesAsync();
                }
            }
        }
    }
}
