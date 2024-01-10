using Common.Database;
using Swos.CareerMod.Database.Models;
using Swos.CareerMod.Database.Repositories;

namespace Swos.CareerMod.Domain;

public interface IAppOptionService
{
    Task<T> Find<T>(string name, T defaultValue);
    Task Save<T>(string name, T value) where T : notnull;
}

public class AppOptionService : IAppOptionService
{
    private readonly IAppOptionRepository repository;
    private readonly IUnitOfWork unitOfWork;

    public AppOptionService(IAppOptionRepository repository, IUnitOfWork unitOfWork)
    {
        this.repository = repository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<T> Find<T>(string name, T defaultValue)
    {
        var appOption = await repository.Find(name);

        if (appOption == null)
            return defaultValue;

        return (T)Convert.ChangeType(appOption.Value, typeof(T));
    }

    public async Task Save<T>(string name, T value) where T : notnull
    {
        var appOption = await repository.Find(name);

        if (appOption == null)
        {
            repository.Add(new AppOption
            {
                Name = name,
                Value = value.ToString()!
            });
        }
        else
        {
            appOption.Value = value.ToString()!;
        }

        await unitOfWork.SaveChangesAsync();
    }
}