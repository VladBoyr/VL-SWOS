using Common.Logging;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Common.Database.Sqlite;

public class SqliteDbStrategy
{
    private readonly string databaseFileName;
    private readonly ILogger logger;

    private static string AppPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".";
    private static string DatabasePath => $"{AppPath}/Database/";
    private static string BackupPath => $"{DatabasePath}/Backup/";
    private static int MaxBackupCount => 100;

    public SqliteDbStrategy(string databaseFileName)
    {
        this.databaseFileName = databaseFileName;
        logger = LogProvider.Create(GetType());
    }

    public string ConnectionString => $"Data Source={DatabasePath}{databaseFileName};";

    public Task Backup()
    {
        try
        {
            logger.LogInformation("Starting of database backup...");

            if (!Directory.Exists(BackupPath))
                Directory.CreateDirectory(BackupPath);

            if (!File.Exists($"{DatabasePath}{databaseFileName}"))
            {
                logger.LogWarning($"Database file '{DatabasePath}{databaseFileName}' not found.");
                return Task.CompletedTask;
            }

            var timestamp = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss_UTC_");
            File.Copy($"{DatabasePath}{databaseFileName}", $"{BackupPath}{timestamp}{databaseFileName}");

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
