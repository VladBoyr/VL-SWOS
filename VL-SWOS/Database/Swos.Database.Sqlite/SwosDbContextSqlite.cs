using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Swos.Database.Sqlite;

public class SwosDbContextSqlite : SwosDbContext
{
    private static string AppPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".";
    private static string ConnectionString => $"Data Source={DatabasePath}{DatabaseFileName};";
    private static string DatabaseFileName => $"swos.db";
    private static string DatabasePath => $"{AppPath}/Database/";
    private static string BackupPath => $"{DatabasePath}/Backup/";
    private static int MaxBackupCount => 100;

    public SwosDbContextSqlite() : base(Options(ConnectionString))
    {
    }

    public SwosDbContextSqlite(DbContextOptions options) : base(options)
    {
    }

    public override Task Backup()
    {
        try
        {
            logger.LogInformation("Starting of database backup...");

            if (!Directory.Exists(BackupPath))
                Directory.CreateDirectory(BackupPath);

            if (!File.Exists($"{DatabasePath}{DatabaseFileName}"))
            {
                logger.LogWarning($"Database file '{DatabasePath}{DatabaseFileName}' not found.");
                return Task.CompletedTask;
            }

            var timestamp = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss_UTC_");
            File.Copy($"{DatabasePath}{DatabaseFileName}", $"{BackupPath}{timestamp}{DatabaseFileName}");

            logger.LogInformation("Database backup successfully finished.");

            DeleteOldBackups();

            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error during database backup.");
            throw;
        }
    }

    private static DbContextOptions Options(string connectionString)
    {
        return new DbContextOptionsBuilder()
            .UseSqlite(connectionString)
            .Options;
    }

    private void DeleteOldBackups()
    {
        logger.LogInformation("Deleting old database backups...");

        var files = Directory.GetFiles(BackupPath)
            .Select(file => new FileInfo(file))
            .OrderByDescending(file => file.LastWriteTime);

        foreach (var file in files.Skip(MaxBackupCount))
        {
            try
            {
                file.Delete();
                logger.LogInformation($"Delete old database backup '{file.Name}' complete.");
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error during delete old database backup '{file.Name}'.");
                throw;
            }
        }

        logger.LogInformation("Delete old database backups finished.");
    }
}
