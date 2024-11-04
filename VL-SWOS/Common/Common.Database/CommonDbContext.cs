using Common.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Common.Database;

public abstract class CommonDbContext : DbContext, IDbContext, IUnitOfWork
{
    protected readonly ILogger logger;

    public CommonDbContext(DbContextOptions options) : base(options)
    {
        logger = LogProvider.Create(GetType());
    }

    public abstract Task Backup();

    public async Task Migrate()
    {
        try
        {
            logger.LogInformation("Starting of database migration...");
            await Database.MigrateAsync();
            logger.LogInformation("Database migration successfully finished.");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error during database migration.");
            throw;
        }
    }
}