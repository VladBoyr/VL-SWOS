using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Common.Logging;

public static class LogProvider
{
    private const string LogPath = @"Logs\log.txt";
    private static readonly ILoggerFactory Factory = LoggerFactory.Create(ConfigureFactory);

    public static ILogger<T> Create<T>()
    {
        return Factory.CreateLogger<T>();
    }

    public static ILogger Create(Type type)
    {
        return Factory.CreateLogger(type);
    }

    private static void ConfigureFactory(ILoggingBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(LogPath, rollingInterval: RollingInterval.Month)
            .CreateLogger();

        builder.AddSerilog(dispose: true);
    }
}