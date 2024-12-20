﻿using Microsoft.EntityFrameworkCore;
using Swos.Database.Models;
using Swos.Domain.Models;

namespace Swos.Database.Repositories;

public interface IGlobalTeamRepository
{
    void Add(GlobalTeam team);
    Task<GlobalTeam?> FindEmpty();
    Task<GlobalTeam[]> FindGlobalTeams(string teamName, SwosCountry teamCountry);
    Task<GlobalTeam[]> FindGlobalTeamsByText(string text);
    Task<GlobalTeam[]> GetAllGlobalTeams();
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

    public Task<GlobalTeam[]> GetAllGlobalTeams()
    {
        return context.GlobalTeams
            .Include(x => x.SwosTeams)
            .ThenInclude(x => x.SwosTeam)
            .ToArrayAsync();
    }

    public async Task<HashSet<int>> GetAllGlobalTeamsSwosIds()
    {
        return (await context.GlobalTeamSwos
                .Select(x => x.SwosTeamId)
                .ToArrayAsync())
            .ToHashSet();
    }
}
