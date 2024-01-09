using Microsoft.EntityFrameworkCore;
using Swos.CareerMod.Database.Models;

namespace Swos.CareerMod.Database.Repositories;

public interface IAppOptionRepository
{
    Task<T> Find<T>(string name, T defaultValue) where T : notnull;
    void Add<T>(string name, T value) where T : notnull;
}

public class AppOptionRepository : IAppOptionRepository
{
    private readonly ICareerModDbContext context;

    public AppOptionRepository(ICareerModDbContext context)
    {
        this.context = context;
    }

    public async Task<T> Find<T>(string name, T defaultValue) where T : notnull
    {
        var appOption = await context.AppOptions
            .Where(x => x.Name == name)
            .SingleOrDefaultAsync();
        
        if (appOption == null)
            return defaultValue;

        return (T)Convert.ChangeType(appOption.Value, typeof(T));
    }

    public void Add<T>(string name, T value) where T : notnull
    {
        context.AppOptions.Add(new AppOption
        {
            Name = name,
            Value = value.ToString()!
        });
    }
}