using System.Globalization;
using Common.Database;
using Common.Logging;
using CsvHelper;
using Microsoft.Extensions.Logging;
using Swos.Database.Models;
using Swos.Database.Repositories;
using Swos.Domain;
using Swos.Domain.Models;

namespace Swos.Database.Importer.ImportFromSwos;

public interface IImporterFromSwos
{
    Task ImportFromSwos2020(string swos2020Path);
}

public sealed class ImporterFromSwos(
    ISwosService swosService,
    ITeamDatabaseRepository teamDatabaseRepository,
    ITeamKitRepository teamKitRepository,
    IUnitOfWork unitOfWork) : IImporterFromSwos
{
    private readonly ILogger<ImporterFromSwos> logger = LogProvider.Create<ImporterFromSwos>();
    
    public async Task ImportFromSwos2020(string swos2020Path)
    {
        var dlcTeamDbPath = $"{swos2020Path}/dlc/teamdb/";
        var teamDbInfoFile = $"{dlcTeamDbPath}teamdb-info.csv";

        if (!File.Exists(teamDbInfoFile))
        {
            logger.LogError($"File '{teamDbInfoFile}' not found!");
            return;
        }
        
        using var reader = new StreamReader(teamDbInfoFile);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var teamDatabases = csv.GetRecords<TeamDatabaseCsv>();
        foreach (var teamDatabaseCsv in teamDatabases)
        {
            await ImportDatabase(swos2020Path, teamDatabaseCsv);
        }
    }

    private async Task ImportDatabase(string swos2020Path, TeamDatabaseCsv teamDatabaseCsv)
    {
        swosService.SetSwosPath($"{swos2020Path}/{teamDatabaseCsv.Name}/data");

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

    private async Task<DbSwosTeam> ToDbSwosTeam(SwosTeam team)
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

    private async Task<DbSwosKit> GetOrAddDbSwosKit(SwosKit teamKit)
    {
        var teamKitDb = await teamKitRepository.FindTeamKit(
            teamKit.KitType,
            teamKit.ShirtMainColor,
            teamKit.ShirtExtraColor,
            teamKit.ShortsColor,
            teamKit.SocksColor);

        if (teamKitDb != null)
            return teamKitDb;

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
}
