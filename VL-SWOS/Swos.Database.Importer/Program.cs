using Swos.Database.Importer.GlobalPlayers;
using Swos.Database.Importer.GlobalTeams;
using Swos.Database.Importer.ImportFromSwos;
using Swos.Database.Models;
using Swos.Database.Repositories;
using Swos.Database.Sqlite;
using Swos.Domain;
using Swos.Domain.Models;

namespace Swos.Database.Importer;

internal static class Program
{
    private static IImporterFromSwos importerFromSwos = null!;
    private static IGlobalTeamLinker globalTeamLinker = null!;
    private static IGlobalPlayerLinker globalPlayerLinker = null!;

    private static async Task Main()
    {
        var context = new SwosDbContextSqlite();
        await context.Backup();
        await context.Migrate();

        importerFromSwos = new ImporterFromSwos(
            new SwosService(),
            new TeamDatabaseRepository(context),
            new TeamKitRepository(context),
            context);

        globalTeamLinker = new GlobalTeamLinker(
            new GlobalTeamRepository(context),
            new TeamDatabaseRepository(context),
            context);

        globalPlayerLinker = new GlobalPlayerLinker(
            new GlobalPlayerRepository(context),
            new TeamDatabaseRepository(context),
            context);

        Console.WriteLine("Swos 2020 path:");
        var swos2020Path = Console.ReadLine();
        await importerFromSwos.ImportFromSwos2020(swos2020Path ?? string.Empty);

        await GlobalTeamStats();
        await GlobalTeamLinksAutomate();
        await GlobalTeamLinks();

        await GlobalPlayerStats();
        await GlobalPlayerLinksAutomate();
    }

    private static async Task GlobalTeamStats()
    {
        var teamDatabasesStats = await globalTeamLinker.GlobalTeamStats();

        var totalGlobalTeamsCount = 0;
        var totalTeamsCount = 0;

        foreach (var (teamDatabase, globalTeamsCount) in teamDatabasesStats)
        {
            Console.WriteLine($"Team Database: {teamDatabase.Title}");

            totalGlobalTeamsCount += globalTeamsCount;
            totalTeamsCount += teamDatabase.Teams.Count;
            var globalPercent = Math.Round(100M * globalTeamsCount / teamDatabase.Teams.Count, 2, MidpointRounding.AwayFromZero);

            Console.WriteLine($"Teams in Global: {globalTeamsCount} ({globalPercent} %)");
            Console.WriteLine($"Teams: {teamDatabase.Teams.Count}");
        }

        var totalPercent = Math.Round(100M * totalGlobalTeamsCount / totalTeamsCount, 2, MidpointRounding.AwayFromZero);
        Console.WriteLine("TOTAL");
        Console.WriteLine($"Total Teams in Global: {totalGlobalTeamsCount} ({totalPercent} %)");
        Console.WriteLine($"Total Teams: {totalTeamsCount}");
        Console.ReadLine();
    }

    private static async Task GlobalTeamLinksAutomate()
    {
        Console.WriteLine("Auto SWOS Teams to Global Teams. Y/N?");
        var inputKey = Console.ReadKey();
        Console.WriteLine();
        if (inputKey.KeyChar != 'Y' && inputKey.KeyChar != 'y')
            return;

        await globalTeamLinker.LinksAutomate();
    }

    private static async Task GlobalTeamLinks()
    {
        var teams = await globalTeamLinker.TeamsToLink();

        foreach (var team in teams)
        {
            Console.WriteLine($"Team Database: {team.TeamDatabase.Title}");
            Console.WriteLine($"{team.Id}. {team.Name} ({team.Country})");

            var globalTeams = await globalTeamLinker.FindGlobalTeams(team.Name, team.Country);
            GlobalTeamsShow(globalTeams);
            var inputStr = Console.ReadLine();
            await globalTeamLinker.MergeGlobalTeams(globalTeams, inputStr);

            int inputId;
            while (!int.TryParse(inputStr, out inputId))
            {
                globalTeams = await globalTeamLinker.FindGlobalTeamsByText(inputStr!);
                GlobalTeamsShow(globalTeams);

                inputStr = Console.ReadLine();
                await globalTeamLinker.MergeGlobalTeams(globalTeams, inputStr);
            }

            if (inputId >= 0)
            {
                if (!await globalTeamLinker.LinksTeam(team, globalTeams, inputId))
                    break;
            }
        }
    }

    private static void GlobalTeamsShow(GlobalTeam[] globalTeams)
    {
        if (globalTeams.Length == 0)
        {
            Console.WriteLine("Global Teams not found.");
        }
        else
        {
            Console.WriteLine("Global Teams found:");
            foreach (var globalTeam in globalTeams
                .SelectMany(x => x.SwosTeams.Select(t =>
                    new
                    {
                        GlobalTeamId = x.Id,
                        TeamName = t.SwosTeam.Name,
                        TeamCountry = t.SwosTeam.Country
                    }))
                .Distinct())
            {
                Console.WriteLine($"{globalTeam.GlobalTeamId}. {globalTeam.TeamName} ({globalTeam.TeamCountry})");
            }
        }
    }

    private static async Task GlobalPlayerStats()
    {
        var playerDatabasesStats = await globalPlayerLinker.GlobalPlayerStats();

        var totalGlobalPlayersCount = 0;
        var totalPlayersCount = 0;

        foreach (var (teamDatabase, globalPlayersCount) in playerDatabasesStats)
        {
            Console.WriteLine($"Team Database: {teamDatabase.Title}");

            var playersCount = teamDatabase.Teams.Count * SwosTeam.PlayersCount;
            totalGlobalPlayersCount += globalPlayersCount;
            totalPlayersCount += playersCount;
            var globalPercent = Math.Round(100M * globalPlayersCount / playersCount, 2, MidpointRounding.AwayFromZero);

            Console.WriteLine($"Players in Global: {globalPlayersCount} ({globalPercent} %)");
            Console.WriteLine($"Players: {playersCount}");
        }

        var totalPercent = Math.Round(100M * totalGlobalPlayersCount / totalPlayersCount, 2, MidpointRounding.AwayFromZero);
        Console.WriteLine("TOTAL");
        Console.WriteLine($"Total Players in Global: {totalGlobalPlayersCount} ({totalPercent} %)");
        Console.WriteLine($"Total Players: {totalPlayersCount}");
        Console.ReadLine();
    }

    private static async Task GlobalPlayerLinksAutomate()
    {
        Console.WriteLine("Auto SWOS Players to Global Players. Y/N?");
        var inputKey = Console.ReadKey();
        Console.WriteLine();
        if (inputKey.KeyChar != 'Y' && inputKey.KeyChar != 'y')
            return;

        await globalPlayerLinker.LinksAutomate();
    }
}
