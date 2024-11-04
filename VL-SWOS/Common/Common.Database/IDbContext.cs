namespace Common.Database;

public interface IDbContext
{
    Task Backup();
    Task Migrate();
}