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