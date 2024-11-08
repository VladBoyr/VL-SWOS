﻿using Microsoft.EntityFrameworkCore;
using Swos.Database.Models;
using Swos.Domain.Models;

namespace Swos.Database.Repositories;

public interface IGlobalTeamRepository
{
    void Add(GlobalTeam team);
    Task<GlobalTeam?> FindEmpty();
    Task<GlobalTeam[]> FindGlobalTeamsExactly(string teamName, SwosCountry teamCountry);
    Task<GlobalTeam[]> FindGlobalTeams(string teamName, SwosCountry teamCountry);
    Task<GlobalTeam[]> FindGlobalTeamsByText(string text);
    Task<GlobalTeamSwos?> FindGlobalTeamSwos(DbSwosTeam team);
    Task<GlobalTeamSwos[]> GetAllGlobalTeamsSwos();
}

public sealed class GlobalTeamRepository(ISwosDbContext context) : IGlobalTeamRepository
{
    private readonly ISwosDbContext context = context;

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
                    .Any(t => t.SwosTeam.Country == findCountry))
                .ToArrayAsync();

            return [.. result, .. countryFound];
        }

        return [.. result];
    }

    public Task<GlobalTeamSwos?> FindGlobalTeamSwos(DbSwosTeam team)
    {
        return context
            .GlobalTeamSwos
            .Where(x => x.SwosTeamId == team.Id)
            .SingleOrDefaultAsync();
    }

    public Task<GlobalTeamSwos[]> GetAllGlobalTeamsSwos()
    {
        return context
            .GlobalTeamSwos
            .ToArrayAsync();
    }
}
