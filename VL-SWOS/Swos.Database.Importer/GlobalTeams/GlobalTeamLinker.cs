using Common.Database;
using Common.Logging;
using Microsoft.Extensions.Logging;
using Swos.Database.Models;
using Swos.Database.Repositories;
using Swos.Domain.Models;

namespace Swos.Database.Importer.GlobalTeams;

public interface IGlobalTeamLinker
{
    Task<(TeamDatabase, int)[]> GlobalTeamStats();
    Task LinksAutomate();
    Task<bool> LinksTeam(DbSwosTeam team, GlobalTeam[] globalTeams, int globalTeamId);
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

    public async Task<(TeamDatabase, int)[]> GlobalTeamStats()
    {
        var existGlobalTeamIds = await globalTeamRepository.GetAllGlobalTeamsSwosIds();
        var teamDatabases = await teamDatabaseRepository.GetTeamDatabases(TeamDatabaseDlo.Teams);

        var result = new List<(TeamDatabase, int)>();
        foreach (var teamDatabase in teamDatabases)
        {
            var globalTeamsCount = teamDatabase.Teams.Count(x => existGlobalTeamIds.Contains(x.Id));
            result.Add((teamDatabase, globalTeamsCount));
        }

        return [.. result];
    }

    public async Task LinksAutomate()
    {
        var existGlobalTeamIds = await globalTeamRepository.GetAllGlobalTeamsSwosIds();
        var teamDatabases = await teamDatabaseRepository.GetTeamDatabases(TeamDatabaseDlo.Teams);

        var existGlobalTeams = (await globalTeamRepository.GetAllGlobalTeams())
            .SelectMany(x => x.SwosTeams.Select(swos => new { GlobalTeam = x, TeamName = swos.SwosTeam.Name }))
            .Distinct()
            .GroupBy(x => x.TeamName)
            .ToDictionary(g => g.Key,
                g => g.Select(x => x.GlobalTeam)
                    .ToArray());

        foreach (var teamDatabase in teamDatabases)
        {
            logger.LogInformation($"Team Database: {teamDatabase.Title}");

            foreach (var team in teamDatabase.Teams.Where(x => !existGlobalTeamIds.Contains(x.Id)))
            {
                var globalTeam = FindGlobalTeamExactly(existGlobalTeams, team.Name, team.Country);
                if (globalTeam == null)
                    continue;

                logger.LogInformation($"{team.Id}. {team.Name} ({team.Country}) => {globalTeam.Id}");

                globalTeam.SwosTeams.Add(
                    new GlobalTeamSwos
                    {
                        SwosTeamId = team.Id,
                        SwosTeam = team
                    });
            }
            
            await unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<bool> LinksTeam(DbSwosTeam team, GlobalTeam[] globalTeams, int globalTeamId)
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
                return false;
        }

        globalTeam.SwosTeams.Add(
            new GlobalTeamSwos
            {
                SwosTeamId = team.Id,
                SwosTeam = team
            });
        await unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<DbSwosTeam[]> TeamsToLink()
    {
        var existGlobalTeams = await globalTeamRepository.GetAllGlobalTeamsSwosIds();
        var teamDatabases = await teamDatabaseRepository.GetTeamDatabases(TeamDatabaseDlo.Teams);

        var teams = new List<DbSwosTeam>();

        foreach (var teamDatabase in teamDatabases.OrderByDescending(x =>
                     x.Teams.Count(t => !existGlobalTeams.Contains(t.Id))))
        {
            teams.AddRange(teamDatabase.Teams.Where(x => !existGlobalTeams.Contains(x.Id)));
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

    private static GlobalTeam? FindGlobalTeamExactly(
        Dictionary<string, GlobalTeam[]> existGlobalTeams,
        string teamName,
        SwosCountry teamCountry)
    {
        GlobalTeam[] globalTeams;
        if (existGlobalTeams.TryGetValue(teamName, out var foundGlobalTeams))
        {
            globalTeams = foundGlobalTeams
                .Where(x => teamName == "EMPTY" ||
                            x.SwosTeams.Any(t => t.SwosTeam.Name == teamName &&
                                                 t.SwosTeam.Country == teamCountry))
                .ToArray();
        }
        else
        {
            return null;
        }

        if (globalTeams.Length == 0 || globalTeams.Select(x => x.Id).Distinct().Count() != 1)
            return null;

        return globalTeams.First();
    }
}
