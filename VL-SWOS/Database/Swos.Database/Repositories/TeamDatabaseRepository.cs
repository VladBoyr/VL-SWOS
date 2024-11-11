﻿using Microsoft.EntityFrameworkCore;
using Swos.Database.Models;

namespace Swos.Database.Repositories;

[Flags]
public enum TeamDatabaseDlo
{
    None = 0,
    Teams = 1,
    Players = 2
}

public interface ITeamDatabaseRepository
{
    void Add(TeamDatabase teamDb);
    Task<TeamDatabase[]> GetTeamDatabases(TeamDatabaseDlo includeData);
}

public sealed class TeamDatabaseRepository(ISwosDbContext context) : ITeamDatabaseRepository
{
    private readonly ISwosDbContext context = context;

    public void Add(TeamDatabase teamDb)
    {
        context.TeamDatabases.Add(teamDb);
    }

    public async Task<TeamDatabase[]> GetTeamDatabases(TeamDatabaseDlo includeData)
    {
        var query = context.TeamDatabases.AsQueryable();

        if (includeData.HasFlag(TeamDatabaseDlo.Teams))
            query = query
                .Include(x => x.Teams);

        if (includeData.HasFlag(TeamDatabaseDlo.Players))
            query = query
                .Include(x => x.Teams)
                .ThenInclude(x => x.Players);

        return await query.ToArrayAsync();
    }
}
