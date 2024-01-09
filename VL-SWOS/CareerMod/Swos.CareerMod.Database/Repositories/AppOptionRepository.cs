using Microsoft.EntityFrameworkCore;
using Swos.CareerMod.Database.Models;

namespace Swos.CareerMod.Database.Repositories;

public interface IAppOptionRepository
{
    Task<AppOption?> Find(string name);
    void Add(AppOption item);
}

public class AppOptionRepository : IAppOptionRepository
{
    private readonly ICareerModDbContext context;

    public AppOptionRepository(ICareerModDbContext context)
    {
        this.context = context;
    }

    public Task<AppOption?> Find(string name)
    {
        return context.AppOptions
            .Where(x => x.Name == name)
            .SingleOrDefaultAsync();
    }

    public void Add(AppOption item)
    {
        context.AppOptions.Add(item);
    }
}