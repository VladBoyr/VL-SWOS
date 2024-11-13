using Microsoft.EntityFrameworkCore;
using Swos.Database.Models;
using Swos.Domain.Models;

namespace Swos.Database.Repositories;

[Flags]
public enum GlobalTeamDlo
{
    None = 0,
    SwosTeam = 1,
}

public interface IGlobalTeamRepository
{
    void Add(GlobalTeam team);
    Task<GlobalTeam?> FindEmpty();
    Task<GlobalTeam[]> FindGlobalTeamsExactly(string teamName, SwosCountry teamCountry);
    Task<GlobalTeam[]> FindGlobalTeams(string teamName, SwosCountry teamCountry);
    Task<GlobalTeam[]> FindGlobalTeamsByText(string text);
    Task<GlobalTeam[]> GetAllGlobalTeams(GlobalTeamDlo includeData);
    Task<HashSet<int>> GetAllGlobalTeamsSwosIds();
}

public sealed class GlobalTeamRepository(ISwosDbContext context) : IGlobalTeamRepository
{
    public void Add(GlobalTeam team)
    {
        context.GlobalTeams.Add(team);
    }

    public Task<GlobalTeam?> FindEmpty()
    {
        return context
            .GlobalTeams
            .Where(x => x.SwosTeams.Count == 0)
            .FirstOrDefaultAsync();
    }

    public Task<GlobalTeam[]> FindGlobalTeamsExactly(string teamName, SwosCountry teamCountry)
    {
        return context.GlobalTeams
            .Include(x => x.SwosTeams)
            .ThenInclude(x => x.SwosTeam)
            .Where(x => x
                .SwosTeams
                .Any(t => t.SwosTeam.Name == teamName &&
                          t.SwosTeam.Country == teamCountry))
            .ToArrayAsync();
    }

    public async Task<GlobalTeam[]> FindGlobalTeams(string teamName, SwosCountry teamCountry)
    {
        var firstFound = await context
            .GlobalTeams
            .Include(x => x.SwosTeams)
            .ThenInclude(x => x.SwosTeam)
            .Where(x => x
                .SwosTeams
                .Any(t => t.SwosTeam.Name == teamName &&
                          t.SwosTeam.Country == teamCountry))
            .ToArrayAsync();

        if (firstFound.Length > 0)
            return firstFound;

        var secondFound = await context
            .GlobalTeams
            .Include(x => x.SwosTeams)
            .ThenInclude(x => x.SwosTeam)
            .Where(x => x
                .SwosTeams
                .Any(t => t.SwosTeam.Name == teamName))
            .ToArrayAsync();

        if (secondFound.Length > 0)
            return secondFound;

        var thirdResult = new List<GlobalTeam>();
        var teamNames = teamName.Replace('.',' ').Split(' ');
        foreach (var partName in teamNames.Where(x => x.Length >= 3))
        {
            var thirdFound = await context
                .GlobalTeams
                .Include(x => x.SwosTeams)
                .ThenInclude(x => x.SwosTeam)
                .Where(x => x.SwosTeams.Any(t => t.SwosTeam.Name.Contains(partName)) &&
                            !thirdResult.Select(exist => exist.Id).Contains(x.Id))
                .ToArrayAsync();
            thirdResult.AddRange(thirdFound);
        }

        return [.. thirdResult];
    }

    public async Task<GlobalTeam[]> FindGlobalTeamsByText(string text)
    {
        var findText = text.ToUpper();

        var result = await context
            .GlobalTeams
            .Include(x => x.SwosTeams)
            .ThenInclude(x => x.SwosTeam)
            .Where(x => x
                .SwosTeams
                .Any(t => t.SwosTeam.Name.Contains(findText)))
            .ToArrayAsync();

        if (Enum.TryParse<SwosCountry>(findText, out var findCountry))
        {
            var countryFound = await context
                .GlobalTeams
                .Include(x => x.SwosTeams)
                .ThenInclude(x => x.SwosTeam)
                .Where(x => x
                    .SwosTeams
                    .Any(t => t.SwosTeam.Country == findCountry) &&
                    !result.Select(gt => gt.Id).Contains(x.Id))
                .ToArrayAsync();

            return [.. result, .. countryFound];
        }

        return [.. result];
    }

    public async Task<GlobalTeam[]> GetAllGlobalTeams(GlobalTeamDlo includeData)
    {
        var query = context.GlobalTeams.AsQueryable();

        if (includeData.HasFlag(GlobalTeamDlo.SwosTeam))
            query = query
                .Include(x => x.SwosTeams)
                .ThenInclude(x => x.SwosTeam);

        return await query.ToArrayAsync();
    }

    public async Task<HashSet<int>> GetAllGlobalTeamsSwosIds()
    {
        return (await context.GlobalTeamSwos
                .Select(x => x.SwosTeamId)
                .ToArrayAsync())
            .ToHashSet();
    }
}
