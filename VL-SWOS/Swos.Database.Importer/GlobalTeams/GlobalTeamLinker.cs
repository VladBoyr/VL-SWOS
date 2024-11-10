using Common.Database;
using Common.Logging;
using Microsoft.Extensions.Logging;
using Swos.Database.Models;
using Swos.Database.Repositories;
using Swos.Domain.Models;

namespace Swos.Database.Importer;

public interface IGlobalTeamLinker
{
    Task<(TeamDatabase, int)[]> GlobalTeamStats();
    Task LinksAutomate();
    Task LinksTeam(DbSwosTeam team, GlobalTeam[] globalTeams, int globalTeamId);
    Task<DbSwosTeam[]> TeamsToLink();
    Task<GlobalTeam[]> FindGlobalTeams(string teamName, SwosCountry teamCountry);
    Task<GlobalTeam[]> FindGlobalTeamsByText(string text);
    Task MergeGlobalTeams(GlobalTeam[] globalTeams, string? mergeQuery);
}

public sealed class GlobalTeamLinker(
    IGlobalTeamRepository globalTeamRepository,
    ITeamDatabaseRepository teamDatabaseRepository,
    IUnitOfWork unitOfWork) : IGlobalTeamLinker
{
    private readonly ILogger<GlobalTeamLinker> logger = LogProvider.Create<GlobalTeamLinker>();
    private readonly IGlobalTeamRepository globalTeamRepository = globalTeamRepository;
    private readonly ITeamDatabaseRepository teamDatabaseRepository = teamDatabaseRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<(TeamDatabase, int)[]> GlobalTeamStats()
    {
        var existGlobalTeams = (await globalTeamRepository.GetAllGlobalTeamsSwos()).Select(x => x.SwosTeamId).ToHashSet();
        var teamDatabases = await teamDatabaseRepository.GetTeamDatabases();

        var result = new List<(TeamDatabase, int)>();
        foreach (var teamDatabase in teamDatabases)
        {
            var globalTeamsCount = teamDatabase.Teams.Count(x => existGlobalTeams.Contains(x.Id));
            result.Add((teamDatabase, globalTeamsCount));
        }

        return [.. result];
    }

    public async Task LinksAutomate()
    {
        var existGlobalTeams = (await globalTeamRepository.GetAllGlobalTeamsSwos()).Select(x => x.SwosTeamId).ToHashSet();
        var teamDatabases = await teamDatabaseRepository.GetTeamDatabases();

        foreach (var teamDatabase in teamDatabases)
        {
            logger.LogInformation($"Team Database: {teamDatabase.Title}");

            foreach (var team in teamDatabase.Teams.Where(x => !existGlobalTeams.Contains(x.Id)))
            {
                var globalTeams = team.Name == "EMPTY"
                    ? await globalTeamRepository.FindGlobalTeams(team.Name, team.Country)
                    : await globalTeamRepository.FindGlobalTeamsExactly(team.Name, team.Country);

                if (globalTeams.Length > 0 && globalTeams.Select(x => x.Id).Distinct().Count() == 1)
                {
                    var globalTeam = globalTeams.First();

                    logger.LogInformation($"{team.Id}. {team.Name} ({team.Country}) => {globalTeam.Id}");

                    globalTeam.SwosTeams.Add(
                        new GlobalTeamSwos
                        {
                            SwosTeamId = team.Id,
                            SwosTeam = team
                        });
                    await unitOfWork.SaveChangesAsync();
                }
            }
        }
    }

    public async Task LinksTeam(DbSwosTeam team, GlobalTeam[] globalTeams, int globalTeamId)
    {
        GlobalTeam? globalTeam;

        if (globalTeamId == 0)
        {
            globalTeam = await globalTeamRepository.FindEmpty();
            if (globalTeam == null)
            {
                globalTeam = new GlobalTeam();
                globalTeamRepository.Add(globalTeam);
            }
        }
        else
        {
            globalTeam = globalTeams.SingleOrDefault(x => x.Id == globalTeamId);
            if (globalTeam == null)
                return;
        }

        globalTeam.SwosTeams.Add(
            new GlobalTeamSwos
            {
                SwosTeamId = team.Id,
                SwosTeam = team
            });
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<DbSwosTeam[]> TeamsToLink()
    {
        var existGlobalTeams = (await globalTeamRepository.GetAllGlobalTeamsSwos()).Select(x => x.SwosTeamId).ToHashSet();
        var teamDatabases = await teamDatabaseRepository.GetTeamDatabases();
        var teams = new List<DbSwosTeam>();

        foreach (var teamDatabase in teamDatabases.OrderByDescending(x => x.Teams.Count(t => !existGlobalTeams.Contains(t.Id))))
        {
            foreach (var team in teamDatabase.Teams.Where(x => !existGlobalTeams.Contains(x.Id)))
            {
                teams.Add(team);
            }
        }

        return [.. teams];
    }

    public Task<GlobalTeam[]> FindGlobalTeams(string teamName, SwosCountry teamCountry)
    {
        return globalTeamRepository.FindGlobalTeams(teamName, teamCountry);
    }

    public Task<GlobalTeam[]> FindGlobalTeamsByText(string text)
    {
        return globalTeamRepository.FindGlobalTeamsByText(text);
    }

    public async Task MergeGlobalTeams(GlobalTeam[] globalTeams, string? mergeQuery)
    {
        var mergeIds = mergeQuery?.Split(' ');
        if (mergeIds?.Length == 2 &&
            int.TryParse(mergeIds[0], out var fromMergeId) &&
            int.TryParse(mergeIds[1], out var toMergeId))
        {
            var fromGlobalTeam = globalTeams.SingleOrDefault(x => x.Id == fromMergeId);
            var toGlobalTeam = globalTeams.SingleOrDefault(x => x.Id == toMergeId);

            if (fromGlobalTeam == null || toGlobalTeam == null)
                return;

            foreach (var fromTeam in fromGlobalTeam.SwosTeams)
            {
                toGlobalTeam.SwosTeams.Add(
                    new GlobalTeamSwos
                    {
                        SwosTeamId = fromTeam.SwosTeam.Id,
                        SwosTeam = fromTeam.SwosTeam
                    });
            }
            fromGlobalTeam.SwosTeams.Clear();
            await unitOfWork.SaveChangesAsync();
        }
    }
}
