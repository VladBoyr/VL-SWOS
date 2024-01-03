using Common.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Common.Database;

public class CommonDbContext : DbContext, IDbContext, IUnitOfWork
{
    private readonly ILogger logger;

    public CommonDbContext(DbContextOptions options) : base(options)
    {
        logger = LogProvider.Create(GetType());
    }

    public async Task Migrate()
    {
        try
        {
            logger.LogInformation("Миграция базы данных стартовала...");
            await Database.MigrateAsync();
            logger.LogInformation("Миграция базы данных завершена.");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Ошибка миграции базы данных");
            throw;
        }
    }
}