namespace Common.Database;

public interface IDbContext
{
    Task Migrate();
}