using Microsoft.EntityFrameworkCore;
using Swos.CareerMod.Database;

namespace SwsCareer.Database.Sqlite;

public class SqliteCareerModDbContext : CareerModDbContext
{
    private const string ConnectionString = @"Data Source=Database\careerMod.db;";

    public SqliteCareerModDbContext() : base(Options(ConnectionString))
    {
    }

    public SqliteCareerModDbContext(DbContextOptions options) : base(options)
    {
    }

    private static DbContextOptions Options(string connectionString)
    {
        return new DbContextOptionsBuilder()
            .UseSqlite(connectionString)
            .Options;
    }
}