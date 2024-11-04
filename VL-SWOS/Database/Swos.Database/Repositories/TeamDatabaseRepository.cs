using Microsoft.EntityFrameworkCore;
using Swos.Database.Models;

namespace Swos.Database.Repositories;

public interface ITeamDatabaseRepository
{
    void Add(TeamDatabase teamDb);
    Task<TeamDatabase[]> GetTeamDatabases();
}

public sealed class TeamDatabaseRepository(ISwosDbContext context) : ITeamDatabaseRepository
{
    private readonly ISwosDbContext context = context;

    public void Add(TeamDatabase teamDb)
    {
        context.TeamDatabases.Add(teamDb);
    }

    public Task<TeamDatabase[]> GetTeamDatabases()
    {
        return context.TeamDatabases
            .Include(x => x.Teams)
            .ToArrayAsync();
    }
}
