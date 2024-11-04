using Common.Database;
using CsvHelper;
using Swos.Database.Repositories;
using Swos.Database.Sqlite;
using Swos.Domain;
using Swos.Domain.Models;
using System.Globalization;
using Swos.Database.Models;

namespace Swos.Database.Importer;

internal class Program
{
    private static ISwosService swosService = null!;
    private static IUnitOfWork unitOfWork = null!;
    private static ITeamDatabaseRepository teamDatabaseRepository = null!;
    private static ITeamKitRepository teamKitRepository = null!;
    private static IGlobalTeamRepository globalTeamRepository = null!;

    static async Task Main()
    {
        var context = new SwosDbContextSqlite();
        await context.Backup();
        await context.Migrate();
        unitOfWork = context;
        teamDatabaseRepository = new TeamDatabaseRepository(context);
        teamKitRepository = new TeamKitRepository(context);
        globalTeamRepository = new GlobalTeamRepository(context);
        swosService = new SwosService();
        
        Console.WriteLine("Swos 2020 path:");
        var swos2020path = Console.ReadLine();
        swos2020path += "/dlc/teamdb/";
        var teamDbInfoFile = $"{swos2020path}teamdb-info.csv";

        if (!File.Exists(teamDbInfoFile))
        {
            Console.WriteLine($"File '{teamDbInfoFile}' not found!");

            await GlobalTeamStats();
            await GlobalTeamLinksAutomate();
            await GlobalTeamLinks();

            return;
        }

        using var reader = new StreamReader(teamDbInfoFile);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var teamDatabases = csv.GetRecords<TeamDatabaseCsv>();
        foreach (var teamDatabaseCsv in teamDatabases)
        {
            await ImportDatabase(swos2020path, teamDatabaseCsv);
        }
    }

    private async static Task ImportDatabase(string swos2020path, TeamDatabaseCsv teamDatabaseCsv)
    {
        swosService.SetSwosPath($"{swos2020path}/{teamDatabaseCsv.Name}/data");

        var teams = new List<DbSwosTeam>();

        foreach (var (_, swosFile) in SwosFileType.AnyTeam.GetSwosFiles())
        {
            var teamsCount = await swosService.OpenSwosFile(swosFile);
            for (var teamId = 0; teamId < teamsCount; teamId++)
            {
                var team = await swosService.ReadTeam(teamId: teamId, includePlayers: true);
                var teamDb = await ToDbSwosTeam(team);
                teams.Add(teamDb);
            }

            swosService.CloseSwosFile();
        }

        var teamDatabase = new TeamDatabase
        {
            Title = teamDatabaseCsv.Name,
            Author = teamDatabaseCsv.Author,
            Version = teamDatabaseCsv.Version,
            Teams = teams
        };

        teamDatabaseRepository.Add(teamDatabase);
        await unitOfWork.SaveChangesAsync();
    }

    private static async Task<DbSwosTeam> ToDbSwosTeam(SwosTeam team)
    {
        var homeKit = await GetOrAddDbSwosKit(team.HomeKit);
        var awayKit = await GetOrAddDbSwosKit(team.AwayKit);

        var teamPlayers = new List<DbSwosTeamPlayer>();
        for (byte posIndex = 0; posIndex < team.PlayerPositions.Length; posIndex++)
        {
            var teamPlayer = ToDbSwosTeamPlayer(posIndex, team.Players[team.PlayerPositions[posIndex]]);
            teamPlayers.Add(teamPlayer);
        }

        return new DbSwosTeam
        {
            GlobalId = team.GlobalId,
            LocalId = team.LocalId,
            FileName = team.FileName,
            Name = team.Name,
            Country = team.Country,
            Division = team.Division,
            HomeKit = homeKit,
            HomeKitId = homeKit.Id,
            AwayKit = awayKit,
            AwayKitId = awayKit.Id,
            CoachName = team.CoachName,
            Tactic = team.Tactic,
            Players = teamPlayers
        };
    }

    private static async Task<DbSwosKit> GetOrAddDbSwosKit(SwosKit teamKit)
    {
        var teamKitDb = await teamKitRepository.FindTeamKit(
            teamKit.KitType,
            teamKit.ShirtMainColor,
            teamKit.ShirtExtraColor,
            teamKit.ShortsColor,
            teamKit.SocksColor);

        if (teamKitDb == null)
        {
            teamKitDb = new DbSwosKit
            {
                KitType = teamKit.KitType,
                ShirtMainColor = teamKit.ShirtMainColor,
                ShirtExtraColor = teamKit.ShirtExtraColor,
                ShortsColor = teamKit.ShortsColor,
                SocksColor = teamKit.SocksColor
            };

            teamKitRepository.Add(teamKitDb);
            await unitOfWork.SaveChangesAsync();
        }

        return teamKitDb;
    }

    private static DbSwosTeamPlayer ToDbSwosTeamPlayer(byte positionIndex, SwosPlayer player)
    {
        var skills = player.Skills
            .Select(x => ToDbSwosSkill(x.Key, x.Value))
            .ToArray();

        return new DbSwosTeamPlayer
        {
            Player = new DbSwosPlayer
            {
                Number = player.Number,
                Name = player.Name,
                Country = player.Country,
                Face = player.Face,
                Position = player.Position,
                Skills = skills,
                Rating = player.Rating
            },
            PlayerPositionIndex = positionIndex
        };
    }

    private static DbSwosSkill ToDbSwosSkill(SwosSkill skill, SwosSkillValue skillValue)
    {
        return new DbSwosSkill
        {
            Skill = skill,
            SkillValue = skillValue.SkillValue,
            PrimarySkill = skillValue.PrimarySkill
        };
    }

    private static async Task GlobalTeamStats()
    {
        var globalTeams = (await globalTeamRepository.GetAllGlobalTeamsSwos()).Select(x => x.SwosTeamId).ToHashSet();
        var teamDatabases = await teamDatabaseRepository.GetTeamDatabases();

        var totalGlobalTeamsCount = 0;
        var totalTeamsCount = 0;

        foreach (var teamDatabase in teamDatabases)
        {
            Console.WriteLine($"Team Database: {teamDatabase.Title}");

            var globalTeamsCount = teamDatabase.Teams.Count(x => globalTeams.Contains(x.Id));
            totalGlobalTeamsCount += globalTeamsCount;
            totalTeamsCount += teamDatabase.Teams.Count;

            Console.WriteLine($"Teams in Global: {globalTeamsCount}");
            Console.WriteLine($"Teams: {teamDatabase.Teams.Count}");
        }

        Console.WriteLine("TOTAL");
        Console.WriteLine($"Total Teams in Global: {totalGlobalTeamsCount}");
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

        var teamDatabases = await teamDatabaseRepository.GetTeamDatabases();

        foreach (var teamDatabase in teamDatabases)
        {
            Console.WriteLine($"Team Database: {teamDatabase.Title}");

            foreach (var team in teamDatabase.Teams)
            {
                var globalTeamSwos = await globalTeamRepository.FindGlobalTeamSwos(team);
                if (globalTeamSwos == null)
                {
                    var globalTeams = await globalTeamRepository.FindGlobalTeamsExactly(team.Name, team.Country);
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
    }

    private static async Task GlobalTeamLinks()
    {
        var teamDatabases = await teamDatabaseRepository.GetTeamDatabases();

        foreach (var teamDatabase in teamDatabases)
        {
            Console.WriteLine($"Team Database: {teamDatabase.Title}");

            foreach (var team in teamDatabase.Teams)
            {
                var globalTeamSwos = await globalTeamRepository.FindGlobalTeamSwos(team);
                if (globalTeamSwos == null)
                {
                    Console.WriteLine($"{team.Id}. {team.Name} ({team.Country})");

                    var globalTeams = await globalTeamRepository.FindGlobalTeams(team.Name, team.Country);
                    GlobalTeamsShow(globalTeams);

                    var inputStr = Console.ReadLine();
                    var inputId = -1;
                    while (!int.TryParse(inputStr, out inputId))
                    {
                        globalTeams = await globalTeamRepository.FindGlobalTeamsByText(inputStr!);
                        GlobalTeamsShow(globalTeams);
                        inputStr = Console.ReadLine();
                    }

                    if (inputId >= 0)
                    {
                        GlobalTeam? globalTeam;

                        if (inputId == 0)
                        {
                            globalTeam = new GlobalTeam();
                            globalTeamRepository.Add(globalTeam);
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
}
