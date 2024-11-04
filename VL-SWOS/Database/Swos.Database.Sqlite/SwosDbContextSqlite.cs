using Common.Database.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Swos.Database.Sqlite;

public class SwosDbContextSqlite : SwosDbContext
{
    private static readonly SqliteDbStrategy dbStrategy = new("swos.db");

    public SwosDbContextSqlite() : base(Options(dbStrategy.ConnectionString))
    {
    }

    public SwosDbContextSqlite(DbContextOptions options) : base(options)
    {
    }

    public override Task Backup()
    {
        return dbStrategy.Backup();
    }

    private static DbContextOptions Options(string connectionString)
    {
        return new DbContextOptionsBuilder()
            .UseSqlite(connectionString)
            .Options;
    }
}
