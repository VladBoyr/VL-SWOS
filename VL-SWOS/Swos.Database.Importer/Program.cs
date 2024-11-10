using Common.Database;
using Swos.Database.Repositories;
using Swos.Database.Sqlite;
using Swos.Domain;
using Swos.Database.Models;

namespace Swos.Database.Importer;

internal class Program
{
    private static IImporterFromSwos importerFromSwos = null!;
    private static IUnitOfWork unitOfWork = null!;
    private static ITeamDatabaseRepository teamDatabaseRepository = null!;
    private static IGlobalTeamRepository globalTeamRepository = null!;

    static async Task Main()
    {
        var context = new SwosDbContextSqlite();
        await context.Backup();
        await context.Migrate();
        unitOfWork = context;
        teamDatabaseRepository = new TeamDatabaseRepository(context);
        globalTeamRepository = new GlobalTeamRepository(context);

        importerFromSwos = new ImporterFromSwos(
            new SwosService(),
            teamDatabaseRepository,
            new TeamKitRepository(context),
            unitOfWork);

        Console.WriteLine("Swos 2020 path:");
        var swos2020path = Console.ReadLine();
        await importerFromSwos.ImportFromSwos2020(swos2020path ?? string.Empty);

        await GlobalTeamStats();
        await GlobalTeamLinksAutomate();
        await GlobalTeamLinks();
    }

    private static async Task GlobalTeamStats()
    {
        var existGlobalTeams = (await globalTeamRepository.GetAllGlobalTeamsSwos()).Select(x => x.SwosTeamId).ToHashSet();
        var teamDatabases = await teamDatabaseRepository.GetTeamDatabases();

        var totalGlobalTeamsCount = 0;
        var totalTeamsCount = 0;

        foreach (var teamDatabase in teamDatabases)
        {
            Console.WriteLine($"Team Database: {teamDatabase.Title}");

            var globalTeamsCount = teamDatabase.Teams.Count(x => existGlobalTeams.Contains(x.Id));
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

        var existGlobalTeams = (await globalTeamRepository.GetAllGlobalTeamsSwos()).Select(x => x.SwosTeamId).ToHashSet();
        var teamDatabases = await teamDatabaseRepository.GetTeamDatabases();

        foreach (var teamDatabase in teamDatabases)
        {
            Console.WriteLine($"Team Database: {teamDatabase.Title}");

            foreach (var team in teamDatabase.Teams.Where(x => !existGlobalTeams.Contains(x.Id)))
            {
                var globalTeams = team.Name == "EMPTY"
                    ? await globalTeamRepository.FindGlobalTeams(team.Name, team.Country)
                    : await globalTeamRepository.FindGlobalTeamsExactly(team.Name, team.Country);

                if (globalTeams.Length > 0 && globalTeams.Select(x => x.Id).Distinct().Count() == 1)
                {
                    var globalTeam = globalTeams.First();

                    Console.WriteLine($"{team.Id}. {team.Name} ({team.Country}) => {globalTeam.Id}");

                    globalTeam.SwosTeams.Add(
                        new GlobalTeamSwos
                        {
                            SwosTeamId = team.Id,
                            SwosTeam = team
                        });
                    await unitOfWork.SaveChangesAsync();
                }
            }
        }
    }

    private static async Task GlobalTeamLinks()
    {
        var existGlobalTeams = (await globalTeamRepository.GetAllGlobalTeamsSwos()).Select(x => x.SwosTeamId).ToHashSet();
        var teamDatabases = await teamDatabaseRepository.GetTeamDatabases();

        foreach (var teamDatabase in teamDatabases.OrderByDescending(x => x.Teams.Count(t => !existGlobalTeams.Contains(t.Id))))
        {
            Console.WriteLine($"Team Database: {teamDatabase.Title}");

            foreach (var team in teamDatabase.Teams.Where(x => !existGlobalTeams.Contains(x.Id)))
            {
                Console.WriteLine($"{team.Id}. {team.Name} ({team.Country})");

                var globalTeams = await globalTeamRepository.FindGlobalTeams(team.Name, team.Country);
                GlobalTeamsShow(globalTeams);

                var inputId = -1;
                var inputStr = Console.ReadLine();
                await MergeGlobalTeams(globalTeams, inputStr);

                while (!int.TryParse(inputStr, out inputId))
                {
                    globalTeams = await globalTeamRepository.FindGlobalTeamsByText(inputStr!);
                    GlobalTeamsShow(globalTeams);
                    inputStr = Console.ReadLine();
                    await MergeGlobalTeams(globalTeams, inputStr);
                }

                if (inputId >= 0)
                {
                    GlobalTeam? globalTeam;

                    if (inputId == 0)
                    {
                        globalTeam = await globalTeamRepository.FindEmpty();
                        if (globalTeam == null)
                        {
                            globalTeam = new GlobalTeam();
                            globalTeamRepository.Add(globalTeam);
                        }
                    }
                    else
                    {
                        globalTeam = globalTeams.SingleOrDefault(x => x.Id == inputId);
                        if (globalTeam == null)
                            return;
                    }

                    globalTeam.SwosTeams.Add(
                        new GlobalTeamSwos
                        {
                            SwosTeamId = team.Id,
                            SwosTeam = team
                        });
                    await unitOfWork.SaveChangesAsync();
                }
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

    private static async Task MergeGlobalTeams(GlobalTeam[] globalTeams, string? mergeQuery)
    {
        var mergeIds = mergeQuery?.Split(' ');
        if (mergeIds?.Length == 2 &&
            int.TryParse(mergeIds[0], out var fromMergeId) &&
            int.TryParse(mergeIds[1], out var toMergeId))
        {
            var fromGlobalTeam = globalTeams.SingleOrDefault(x => x.Id == fromMergeId);
            var toGlobalTeam = globalTeams.SingleOrDefault(x => x.Id == toMergeId);

            if (fromGlobalTeam == null || toGlobalTeam == null)
                return;

            foreach (var fromTeam in fromGlobalTeam.SwosTeams)
            {
                toGlobalTeam.SwosTeams.Add(
                    new GlobalTeamSwos
                    {
                        SwosTeamId = fromTeam.SwosTeam.Id,
                        SwosTeam = fromTeam.SwosTeam
                    });
            }
            fromGlobalTeam.SwosTeams.Clear();
            await unitOfWork.SaveChangesAsync();
        }
    }
}
