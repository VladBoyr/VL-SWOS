using Swos.Domain.Models;
using System.Text;

namespace Swos.Domain;

public interface ISwosService : IDisposable
{
    bool SetSwosPath(string path);
    string GetSwosPath();
    Task<int> OpenSwosFile(string fileName);
    void CloseSwosFile();
    string GetSwosFileName();

    Task<int> ReadLocalTeamId(int teamId);
    Task<int> ReadGlobalTeamId(int teamId);
    Task<string> ReadTeamName(int teamId);
    Task<SwosCountry> ReadTeamCountry(int teamId);
    Task<byte> ReadTeamDivision(int teamId);
    Task<SwosKit> ReadTeamKit(int teamId, bool awayKit = false);
    Task<string> ReadTeamCoachName(int teamId);
    Task<SwosTactic> ReadTeamTactic(int teamId);
    Task<byte[]> ReadTeamPlayerPositions(int teamId);
    Task<SwosTeam> ReadTeam(int teamId, bool includePlayers = false);

    Task<byte> ReadPlayerNumber(int teamId, int playerId);
    Task<string> ReadPlayerName(int teamId, int playerId);
    Task<SwosCountry> ReadPlayerCountry(int teamId, int playerId);
    Task<(SwosPosition, SwosFace)> ReadPlayerPositionAndFace(int teamId, int playerId);
    Task<Dictionary<SwosSkill, SwosSkillValue>> ReadPlayerSkills(int teamId, int playerId, SwosPosition playerPosition);
    Task<byte> ReadPlayerRating(int teamId, int playerId);
    Task<SwosPlayer> ReadPlayer(int teamId, int playerId);
    Task<SwosPlayer[]> ReadPlayers(int teamId);

    Task WriteTeamName(int teamId, string teamName);
    Task WriteTeamCountry(int teamId, SwosCountry teamCountry);
    Task WriteTeamDivision(int teamId, byte teamDivision);
    Task WriteTeamKit(int teamId, SwosKit teamKit, bool awayKit = false);
    Task WriteTeamCoachName(int teamId, string coachName);
    Task WriteTeamTactic(int teamId, SwosTactic teamTactic);
    Task WriteTeamPlayerPositions(int teamId, byte[] playerPositions);
    Task WriteTeam(int teamId, SwosTeam team);

    Task WritePlayerNumber(int teamId, int playerId, byte playerNumber);
    Task WritePlayerName(int teamId, int playerId, string playerName);
    Task WritePlayerCountry(int teamId, int playerId, SwosCountry playerCountry);
    Task WritePlayerPositionAndFace(int teamId, int playerId, SwosPosition playerPosition, SwosFace playerFace);
    Task WritePlayerSkills(int teamId, int playerId, SwosPosition playerPosition, Dictionary<SwosSkill, SwosSkillValue> playerSkills);
    Task WritePlayerRating(int teamId, int playerId, byte playerRating);
    Task WritePlayer(int teamId, int playerId, SwosPlayer player);
    Task WritePlayers(int teamId, SwosPlayer[] players);

    Task<List<SwosFindTeam>> FindTeams(SwosFindTeamQuery query);
    Task<List<SwosFindPlayer>> FindPlayers(SwosFindPlayerQuery query);
}

public class SwosService : ISwosService
{
    private const int offsetTeam = 2;
    private string swosPath = "";
    private string swosFileName = "";
    private int swosTeamCount = 0;
    private FileStream swosFile = null!;

    public bool IsCountryChangedBySwosCommunity { get; set; } = false;

    public void Dispose()
    {
        CloseSwosFile();
        GC.SuppressFinalize(this);
    }

    public bool SetSwosPath(string path)
    {
        if (Directory.Exists(path))
        {
            if (path.EndsWith(Path.DirectorySeparatorChar) || path.EndsWith(Path.AltDirectorySeparatorChar))
            {
                swosPath = path;
            }
            else
            {
                swosPath = path + Path.DirectorySeparatorChar;
            }

            return true;
        }

        return false;
    }

    public string GetSwosPath()
    {
        return swosPath;
    }

    public async Task<int> OpenSwosFile(string fileName)
    {
        try
        {
            if (swosFileName == fileName)
                return swosTeamCount;

            CloseSwosFile();

            swosFile = new FileStream(swosPath + fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            swosFile.Seek(1, SeekOrigin.Begin);
            var buffer = new byte[1];
            await swosFile.ReadAsync(buffer);

            swosTeamCount = buffer[0];
            swosFileName = fileName;
        }
        catch
        {
            swosTeamCount = -1;
        }

        return swosTeamCount;
    }

    public void CloseSwosFile()
    {
        if (swosFile != null)
        {
            swosFile.Close();
            swosFile.Dispose();
        }
        swosFileName = "";
    }

    public string GetSwosFileName()
    {
        return swosPath + swosFileName;
    }

    public async Task<int> ReadLocalTeamId(int teamId)
    {
        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + 1, SeekOrigin.Begin);
        var buffer = new byte[1];
        await swosFile.ReadAsync(buffer);

        return buffer[0];
    }

    public async Task<int> ReadGlobalTeamId(int teamId)
    {
        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + 2, SeekOrigin.Begin);
        var buffer = new byte[2];
        await swosFile.ReadAsync(buffer);

        return buffer[0] * 256 + buffer[1];
    }

    public async Task<string> ReadTeamName(int teamId)
    {
        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + 5, SeekOrigin.Begin);
        var buffer = new byte[SwosTeam.MaxNameLength];
        await swosFile.ReadAsync(buffer);

        var sb = new StringBuilder();
        for (int i = 0; i < buffer.Length; i++)
        {
            if (buffer[i] == 0)
                break;

            sb.Append((char)buffer[i]);
        }

        return new(sb.ToString());
    }

    public async Task<SwosCountry> ReadTeamCountry(int teamId)
    {
        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize, SeekOrigin.Begin);
        var buffer = new byte[1];
        await swosFile.ReadAsync(buffer);

        var teamCountry = buffer[0].ClubCountryToCountry();
        return IsCountryChangedBySwosCommunity ? teamCountry.CountryChangedBySwosCommunity() : teamCountry;
    }

    public async Task<byte> ReadTeamDivision(int teamId)
    {
        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + 25, SeekOrigin.Begin);
        var buffer = new byte[1];
        await swosFile.ReadAsync(buffer);

        return buffer[0];
    }

    public async Task<SwosKit> ReadTeamKit(int teamId, bool awayKit = false)
    {
        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + 26 + (awayKit ? 1 : 0) * SwosKit.SwosSize, SeekOrigin.Begin);
        var buffer = new byte[SwosKit.SwosSize];
        await swosFile.ReadAsync(buffer);

        return new SwosKit
        {
            KitType = (SwosKitType)buffer[0],
            ShirtMainColor = (SwosColor)buffer[1],
            ShirtExtraColor = (SwosColor)buffer[2],
            ShortsColor = (SwosColor)buffer[3],
            SocksColor = (SwosColor)buffer[4]
        };
    }

    public async Task<string> ReadTeamCoachName(int teamId)
    {
        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + 36, SeekOrigin.Begin);
        var buffer = new byte[SwosTeam.MaxCoachNameLength];
        await swosFile.ReadAsync(buffer);

        var sb = new StringBuilder();
        for (int i = 0; i < buffer.Length; i++)
        {
            if (buffer[i] == 0)
                break;

            sb.Append((char)buffer[i]);
        }

        return sb.ToString();
    }

    public async Task<SwosTactic> ReadTeamTactic(int teamId)
    {
        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + 24, SeekOrigin.Begin);
        var buffer = new byte[1];
        await swosFile.ReadAsync(buffer);

        return (SwosTactic)buffer[0];
    }

    public async Task<byte[]> ReadTeamPlayerPositions(int teamId)
    {
        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + 60, SeekOrigin.Begin);
        var buffer = new byte[SwosTeam.PlayersCount];
        await swosFile.ReadAsync(buffer);

        return buffer;
    }

    public async Task<SwosTeam> ReadTeam(int teamId, bool includePlayers = false)
    {
        var globalTeamId = await ReadGlobalTeamId(teamId);
        var localTeamId = await ReadLocalTeamId(teamId);
        var teamName = await ReadTeamName(teamId);
        var teamCountry = await ReadTeamCountry(teamId);
        var teamDivision = await ReadTeamDivision(teamId);
        var homeKit = await ReadTeamKit(teamId);
        var awayKit = await ReadTeamKit(teamId, awayKit: true);
        var coachName = await ReadTeamCoachName(teamId);
        var tactic = await ReadTeamTactic(teamId);
        var playerPositions = await ReadTeamPlayerPositions(teamId);
        var players = includePlayers ? await ReadPlayers(teamId) : new SwosPlayer[SwosTeam.PlayersCount];

        return new SwosTeam
        {
            GlobalId = globalTeamId,
            LocalId = localTeamId,
            Name = teamName,
            Country = teamCountry,
            Division = teamDivision,
            HomeKit = homeKit,
            AwayKit = awayKit,
            CoachName = coachName,
            Tactic = tactic,
            PlayerPositions = playerPositions,
            Players = players
        };
    }

    public async Task<byte> ReadPlayerNumber(int teamId, int playerId)
    {
        if (playerId < 0 || playerId >= SwosTeam.PlayersCount)
        {
            throw new ArgumentOutOfRangeException(nameof(playerId));
        }

        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + SwosTeam.HeaderSize + playerId * SwosPlayer.SwosSize + 2, SeekOrigin.Begin);
        var buffer = new byte[1];
        await swosFile.ReadAsync(buffer);

        return buffer[0];
    }

    public async Task<string> ReadPlayerName(int teamId, int playerId)
    {
        if (playerId < 0 || playerId >= SwosTeam.PlayersCount)
        {
            throw new ArgumentOutOfRangeException(nameof(playerId));
        }

        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + SwosTeam.HeaderSize + playerId * SwosPlayer.SwosSize + 3, SeekOrigin.Begin);
        var buffer = new byte[SwosPlayer.MaxNameLength];
        await swosFile.ReadAsync(buffer);

        var sb = new StringBuilder();
        for (int i = 0; i < buffer.Length; i++)
        {
            if (buffer[i] == 0)
                break;

            sb.Append((char)buffer[i]);
        }

        return sb.ToString();
    }

    public async Task<SwosCountry> ReadPlayerCountry(int teamId, int playerId)
    {
        if (playerId < 0 || playerId >= SwosTeam.PlayersCount)
        {
            throw new ArgumentOutOfRangeException(nameof(playerId));
        }

        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + SwosTeam.HeaderSize + playerId * SwosPlayer.SwosSize, SeekOrigin.Begin);
        var buffer = new byte[1];
        await swosFile.ReadAsync(buffer);

        var playerCountry = (SwosCountry)buffer[0];
        return IsCountryChangedBySwosCommunity ? playerCountry.CountryChangedBySwosCommunity() : playerCountry;
    }

    public async Task<(SwosPosition, SwosFace)> ReadPlayerPositionAndFace(int teamId, int playerId)
    {
        if (playerId < 0 || playerId >= SwosTeam.PlayersCount)
        {
            throw new ArgumentOutOfRangeException(nameof(playerId));
        }

        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + SwosTeam.HeaderSize + playerId * SwosPlayer.SwosSize + 26, SeekOrigin.Begin);
        var buffer = new byte[1];
        await swosFile.ReadAsync(buffer);

        var face = (SwosFace)(buffer[0] % 32);
        var position = (SwosPosition)(buffer[0] - face);

        return (position, face);
    }

    public async Task<Dictionary<SwosSkill, SwosSkillValue>> ReadPlayerSkills(int teamId, int playerId, SwosPosition playerPosition)
    {
        if (playerId < 0 || playerId >= SwosTeam.PlayersCount)
        {
            throw new ArgumentOutOfRangeException(nameof(playerId));
        }

        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + SwosTeam.HeaderSize + playerId * SwosPlayer.SwosSize + 28, SeekOrigin.Begin);
        var buffer = new byte[4];
        await swosFile.ReadAsync(buffer);

        var passing = buffer[0];
        var shooting = buffer[1] / 16;
        var heading = playerPosition == SwosPosition.D ? buffer[2] / 16 : buffer[1] % 16;
        var tackling = playerPosition == SwosPosition.D ? buffer[1] % 16 : buffer[2] / 16;
        var ballControl = buffer[2] % 16;
        var speed = buffer[3] / 16;
        var finishing = buffer[3] % 16;

        return new()
        {
            { SwosSkill.Passing, new SwosSkillValue((byte)(passing % 8), passing / 8 >= 1) },
            { SwosSkill.Shooting, new SwosSkillValue((byte)(shooting % 8), shooting / 8 >= 1) },
            { SwosSkill.Heading, new SwosSkillValue((byte)(heading % 8), heading / 8 >= 1) },
            { SwosSkill.Tackling, new SwosSkillValue((byte)(tackling % 8), tackling / 8 >= 1) },
            { SwosSkill.BallControl, new SwosSkillValue((byte)(ballControl % 8), ballControl / 8 >= 1) },
            { SwosSkill.Speed, new SwosSkillValue((byte)(speed % 8), speed / 8 >= 1) },
            { SwosSkill.Finishing, new SwosSkillValue((byte)(finishing % 8), finishing / 8 >= 1) }
        };
    }

    public async Task<byte> ReadPlayerRating(int teamId, int playerId)
    {
        if (playerId < 0 || playerId >= SwosTeam.PlayersCount)
        {
            throw new ArgumentOutOfRangeException(nameof(playerId));
        }

        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + SwosTeam.HeaderSize + playerId * SwosPlayer.SwosSize + 32, SeekOrigin.Begin);
        var buffer = new byte[1];
        await swosFile.ReadAsync(buffer);

        return buffer[0];
    }

    public async Task<SwosPlayer> ReadPlayer(int teamId, int playerId)
    {
        var playerNumber = await ReadPlayerNumber(teamId, playerId);
        var playerName = await ReadPlayerName(teamId, playerId);
        var playerCountry = await ReadPlayerCountry(teamId, playerId);
        var (playerPosition, playerFace) = await ReadPlayerPositionAndFace(teamId, playerId);
        var playerSkills = await ReadPlayerSkills(teamId, playerId, playerPosition);
        var playerRating = await ReadPlayerRating(teamId, playerId);

        return new SwosPlayer
        {
            Number = playerNumber,
            Name = playerName,
            Country = playerCountry,
            Face = playerFace,
            Position = playerPosition,
            Skills = playerSkills,
            Rating = playerRating
        };
    }

    public async Task<SwosPlayer[]> ReadPlayers(int teamId)
    {
        var players = new SwosPlayer[SwosTeam.PlayersCount];
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = await ReadPlayer(teamId, i);
        }

        return players;
    }

    public async Task WriteTeamName(int teamId, string teamName)
    {
        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + 5, SeekOrigin.Begin);
        var buffer = new byte[SwosTeam.MaxNameLength];

        for (int i = 0; i < buffer.Length; i++)
        {
            if (i < teamName.Length)
                buffer[i] = (byte)teamName[i];
            else
                buffer[i] = 0;
        }

        await swosFile.WriteAsync(buffer);
    }

    public async Task WriteTeamCountry(int teamId, SwosCountry teamCountry)
    {
        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize, SeekOrigin.Begin);
        var buffer = new byte[1];
        buffer[0] = (IsCountryChangedBySwosCommunity ? teamCountry.CountryChangedBySwosCommunity() : teamCountry).CountryToClubCountry();
        await swosFile.WriteAsync(buffer);
    }

    public async Task WriteTeamDivision(int teamId, byte teamDivision)
    {
        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + 25, SeekOrigin.Begin);
        var buffer = new byte[1];
        buffer[0] = teamDivision;
        await swosFile.WriteAsync(buffer);
    }

    public async Task WriteTeamKit(int teamId, SwosKit teamKit, bool awayKit = false)
    {
        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + 26 + (awayKit ? 1 : 0) * SwosKit.SwosSize, SeekOrigin.Begin);
        var buffer = new byte[SwosKit.SwosSize];
        buffer[0] = (byte)teamKit.KitType;
        buffer[1] = (byte)teamKit.ShirtMainColor;
        buffer[2] = (byte)teamKit.ShirtExtraColor;
        buffer[3] = (byte)teamKit.ShortsColor;
        buffer[4] = (byte)teamKit.SocksColor;
        await swosFile.WriteAsync(buffer);
    }

    public async Task WriteTeamCoachName(int teamId, string coachName)
    {
        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + 36, SeekOrigin.Begin);
        var buffer = new byte[SwosTeam.MaxCoachNameLength];

        for (int i = 0; i < buffer.Length; i++)
        {
            if (i < coachName.Length)
                buffer[i] = (byte)coachName[i];
            else
                buffer[i] = 0;
        }

        await swosFile.WriteAsync(buffer);
    }

    public async Task WriteTeamTactic(int teamId, SwosTactic teamTactic)
    {
        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + 24, SeekOrigin.Begin);
        var buffer = new byte[1];
        buffer[0] = (byte)teamTactic;
        await swosFile.WriteAsync(buffer);
    }

    public async Task WriteTeamPlayerPositions(int teamId, byte[] playerPositions)
    {
        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + 60, SeekOrigin.Begin);
        await swosFile.WriteAsync(playerPositions);
    }

    public async Task WriteTeam(int teamId, SwosTeam team)
    {
        await WriteTeamName(teamId, team.Name);
        await WriteTeamCountry(teamId, team.Country);
        await WriteTeamDivision(teamId, team.Division);
        await WriteTeamKit(teamId, team.HomeKit);
        await WriteTeamKit(teamId, team.AwayKit, awayKit: true);
        await WriteTeamCoachName(teamId, team.CoachName);
        await WriteTeamTactic(teamId, team.Tactic);
        await WriteTeamPlayerPositions(teamId, team.PlayerPositions);
    }

    public async Task WritePlayerNumber(int teamId, int playerId, byte playerNumber)
    {
        if (playerId < 0 || playerId >= SwosTeam.PlayersCount)
        {
            throw new ArgumentOutOfRangeException(nameof(playerId));
        }

        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + SwosTeam.HeaderSize + playerId * SwosPlayer.SwosSize + 2, SeekOrigin.Begin);
        var buffer = new byte[1];
        buffer[0] = playerNumber;
        await swosFile.WriteAsync(buffer);
    }

    public async Task WritePlayerName(int teamId, int playerId, string playerName)
    {
        if (playerId < 0 || playerId >= SwosTeam.PlayersCount)
        {
            throw new ArgumentOutOfRangeException(nameof(playerId));
        }

        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + SwosTeam.HeaderSize + playerId * SwosPlayer.SwosSize + 3, SeekOrigin.Begin);
        var buffer = new byte[SwosPlayer.MaxNameLength];

        for (int i = 0; i < buffer.Length; i++)
        {
            if (i < playerName.Length)
                buffer[i] = (byte)playerName[i];
            else
                buffer[i] = 0;
        }

        await swosFile.WriteAsync(buffer);
    }

    public async Task WritePlayerCountry(int teamId, int playerId, SwosCountry playerCountry)
    {
        if (playerId < 0 || playerId >= SwosTeam.PlayersCount)
        {
            throw new ArgumentOutOfRangeException(nameof(playerId));
        }

        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + SwosTeam.HeaderSize + playerId * SwosPlayer.SwosSize, SeekOrigin.Begin);
        var buffer = new byte[1];
        buffer[0] = (byte)(IsCountryChangedBySwosCommunity ? playerCountry.CountryChangedBySwosCommunity() : playerCountry);
        await swosFile.WriteAsync(buffer);
    }

    public async Task WritePlayerPositionAndFace(int teamId, int playerId, SwosPosition playerPosition, SwosFace playerFace)
    {
        if (playerId < 0 || playerId >= SwosTeam.PlayersCount)
        {
            throw new ArgumentOutOfRangeException(nameof(playerId));
        }

        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + SwosTeam.HeaderSize + playerId * SwosPlayer.SwosSize + 26, SeekOrigin.Begin);
        var buffer = new byte[1];
        buffer[0] = (byte)playerPosition;
        buffer[0] += (byte)playerFace;
        await swosFile.WriteAsync(buffer);
    }

    public async Task WritePlayerSkills(int teamId, int playerId, SwosPosition playerPosition, Dictionary<SwosSkill, SwosSkillValue> playerSkills)
    {
        if (playerId < 0 || playerId >= SwosTeam.PlayersCount)
        {
            throw new ArgumentOutOfRangeException(nameof(playerId));
        }

        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + SwosTeam.HeaderSize + playerId * SwosPlayer.SwosSize + 28, SeekOrigin.Begin);

        var passing = playerSkills[SwosSkill.Passing].ToByte();
        var shooting = playerSkills[SwosSkill.Shooting].ToByte();
        var heading = playerSkills[SwosSkill.Heading].ToByte();
        var tackling = playerSkills[SwosSkill.Tackling].ToByte();
        var ballControl = playerSkills[SwosSkill.BallControl].ToByte();
        var speed = playerSkills[SwosSkill.Speed].ToByte();
        var finishing = playerSkills[SwosSkill.Finishing].ToByte();

        var buffer = new byte[4];
        buffer[0] = passing;
        buffer[1] = (byte)(shooting * 16 + (playerPosition == SwosPosition.D ? tackling : heading));
        buffer[2] = (byte)((playerPosition == SwosPosition.D ? heading : tackling) * 16 + ballControl);
        buffer[3] = (byte)(speed * 16 + finishing);
        await swosFile.WriteAsync(buffer);
    }

    public async Task WritePlayerRating(int teamId, int playerId, byte playerRating)
    {
        if (playerId < 0 || playerId >= SwosTeam.PlayersCount)
        {
            throw new ArgumentOutOfRangeException(nameof(playerId));
        }

        swosFile.Seek(offsetTeam + teamId * SwosTeam.SwosSize + SwosTeam.HeaderSize + playerId * SwosPlayer.SwosSize + 32, SeekOrigin.Begin);
        var buffer = new byte[1];
        buffer[0] = playerRating;
        await swosFile.WriteAsync(buffer);
    }

    public async Task WritePlayer(int teamId, int playerId, SwosPlayer player)
    {
        await WritePlayerNumber(teamId, playerId, player.Number);
        await WritePlayerName(teamId, playerId, player.Name);
        await WritePlayerCountry(teamId, playerId, player.Country);
        await WritePlayerPositionAndFace(teamId, playerId, player.Position, player.Face);
        await WritePlayerSkills(teamId, playerId, player.Position, player.Skills);
        await WritePlayerRating(teamId, playerId, player.Rating);
    }

    public async Task WritePlayers(int teamId, SwosPlayer[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            await WritePlayer(teamId, i, players[i]);
        }
    }

    public async Task<List<SwosFindTeam>> FindTeams(SwosFindTeamQuery query)
    {
        var findTeams = new List<SwosFindTeam>();
        foreach (var (_, swosFile) in query.FileTypes.GetSwosFiles())
        {
            var teamsCount = await OpenSwosFile(swosFile);
            for (var teamId = 0; teamId < teamsCount; teamId++)
            {
                var findResult = true;

                if (findResult && (
                    query.GlobalIds.HasValue ||
                    query.MinGlobalId.HasValue ||
                    query.MaxGlobalId.HasValue))
                {
                    var globalId = await ReadGlobalTeamId(teamId);

                    if (findResult && query.GlobalIds.HasValue)
                        findResult = findResult && query.GlobalIds.Value.Contains(globalId);

                    if (findResult && query.MinGlobalId.HasValue)
                        findResult = findResult && (globalId >= query.MinGlobalId.Value);

                    if (findResult && query.MaxGlobalId.HasValue)
                        findResult = findResult && (globalId <= query.MaxGlobalId.Value);
                }

                if (findResult && (
                    query.LocalIds.HasValue ||
                    query.MinLocalId.HasValue ||
                    query.MaxLocalId.HasValue))
                {
                    var localId = await ReadLocalTeamId(teamId);

                    if (findResult && query.LocalIds.HasValue)
                        findResult = findResult && query.LocalIds.Value.Contains(localId);

                    if (findResult && query.MinLocalId.HasValue)
                        findResult = findResult && (localId >= query.MinGlobalId.Value);

                    if (findResult && query.MaxLocalId.HasValue)
                        findResult = findResult && (localId <= query.MaxGlobalId.Value);
                }

                if (findResult && query.NameLike.HasValue)
                {
                    var teamName = await ReadTeamName(teamId);
                    findResult = findResult && teamName.Contains(query.NameLike.Value, StringComparison.OrdinalIgnoreCase);
                }

                if (findResult && query.Countries.HasValue)
                {
                    var teamCountry = await ReadTeamCountry(teamId);
                    findResult = findResult && query.Countries.Value.Contains(teamCountry);
                }

                if (findResult && query.Divisions.HasValue)
                {
                    var teamDivision = await ReadTeamDivision(teamId);
                    findResult = findResult && query.Divisions.Value.Contains(teamDivision);
                }

                if (findResult && (
                    query.KitTypes.HasValue ||
                    query.ShirtMainColors.HasValue ||
                    query.ShirtExtraColors.HasValue ||
                    query.ShortsColors.HasValue ||
                    query.SocksColors.HasValue))
                {
                    var homeKit = await ReadTeamKit(teamId);
                    var awayKit = await ReadTeamKit(teamId, awayKit: true);

                    if (findResult && query.KitTypes.HasValue)
                        findResult = findResult && (
                            query.KitTypes.Value.Contains(homeKit.KitType) ||
                            query.KitTypes.Value.Contains(awayKit.KitType));

                    if (findResult && query.ShirtMainColors.HasValue)
                        findResult = findResult && (
                            query.ShirtMainColors.Value.Contains(homeKit.ShirtMainColor) ||
                            query.ShirtMainColors.Value.Contains(awayKit.ShirtMainColor));

                    if (findResult && query.ShirtExtraColors.HasValue)
                        findResult = findResult && (
                            query.ShirtExtraColors.Value.Contains(homeKit.ShirtExtraColor) ||
                            query.ShirtExtraColors.Value.Contains(awayKit.ShirtExtraColor));

                    if (findResult && query.ShortsColors.HasValue)
                        findResult = findResult && (
                            query.ShortsColors.Value.Contains(homeKit.ShortsColor) ||
                            query.ShortsColors.Value.Contains(awayKit.ShortsColor));

                    if (findResult && query.SocksColors.HasValue)
                        findResult = findResult && (
                            query.SocksColors.Value.Contains(homeKit.SocksColor) ||
                            query.SocksColors.Value.Contains(awayKit.SocksColor));
                }

                if (findResult && query.CoachNameLike.HasValue)
                {
                    var coachName = await ReadTeamCoachName(teamId);
                    findResult = findResult && coachName.Contains(query.CoachNameLike.Value, StringComparison.OrdinalIgnoreCase);
                }

                if (findResult && query.Tactics.HasValue)
                {
                    var teamTactic = await ReadTeamTactic(teamId);
                    findResult = findResult && query.Tactics.Value.Contains(teamTactic);
                }

                if (findResult)
                {
                    findTeams.Add(new SwosFindTeam
                    {
                        SwosFile = swosFile,
                        TeamId = teamId
                    });
                }
            }
        }

        return findTeams;
    }

    public async Task<List<SwosFindPlayer>> FindPlayers(SwosFindPlayerQuery query)
    {
        var findPlayers = new List<SwosFindPlayer>();
        foreach (var (_, swosFile) in query.FileTypes.GetSwosFiles())
        {
            var teamsCount = await OpenSwosFile(swosFile);
            for (var teamId = 0; teamId < teamsCount; teamId++)
                for (var playerId = 0; playerId < SwosTeam.PlayersCount; playerId++)
                {
                    var findResult = true;

                    if (findResult && (
                        query.Numbers.HasValue ||
                        query.MinNumber.HasValue ||
                        query.MaxNumber.HasValue))
                    {
                        var playerNumber = await ReadPlayerNumber(teamId, playerId);

                        if (findResult && query.Numbers.HasValue)
                            findResult = findResult && query.Numbers.Value.Contains(playerNumber);

                        if (findResult && query.MinNumber.HasValue)
                            findResult = findResult && (playerNumber >= query.MinNumber.Value);

                        if (findResult && query.MaxNumber.HasValue)
                            findResult = findResult && (playerNumber <= query.MaxNumber.Value);
                    }

                    if (findResult && query.NameLike.HasValue)
                    {
                        var playerName = await ReadPlayerName(teamId, playerId);
                        findResult = findResult && playerName.Contains(query.NameLike.Value, StringComparison.OrdinalIgnoreCase);
                    }

                    if (findResult && query.Countries.HasValue)
                    {
                        var playerCountry = await ReadPlayerCountry(teamId, playerId);
                        findResult = findResult && query.Countries.Value.Contains(playerCountry);
                    }

                    if (findResult && (
                        query.Faces.HasValue ||
                        query.Positions.HasValue ||
                        query.MinSkills.HasValue ||
                        query.MaxSkills.HasValue ||
                        query.PrimarySkills.HasValue))
                    {
                        var (playerPosition, playerFace) = await ReadPlayerPositionAndFace(teamId, playerId);

                        if (findResult && query.Faces.HasValue)
                            findResult = findResult && query.Faces.Value.Contains(playerFace);

                        if (findResult && query.Positions.HasValue)
                            findResult = findResult && query.Positions.Value.Contains(playerPosition);

                        if (findResult && (
                            query.MinSkills.HasValue ||
                            query.MaxSkills.HasValue ||
                            query.PrimarySkills.HasValue))
                        {
                            var skills = await ReadPlayerSkills(teamId, playerId, playerPosition);

                            if (findResult && query.MinSkills.HasValue)
                                findResult = findResult && query.MinSkills.Value.All(x => x.Value <= skills[x.Key].SkillValue);

                            if (findResult && query.MaxSkills.HasValue)
                                findResult = findResult && query.MaxSkills.Value.All(x => x.Value >= skills[x.Key].SkillValue);

                            if (findResult && query.PrimarySkills.HasValue)
                            {
                                var primarySkills = skills.Where(x => x.Value.PrimarySkill).Select(x => x.Key);
                                findResult = findResult && (query.PrimarySkills.Value.Length == query.PrimarySkills.Value.Intersect(primarySkills).Count());
                            }
                        }
                    }

                    if (findResult && (
                        query.MinRating.HasValue ||
                        query.MaxRating.HasValue))
                    {
                        var playerRating = await ReadPlayerRating(teamId, playerId);

                        if (findResult && query.MinRating.HasValue)
                            findResult = findResult && (playerRating >= query.MinRating.Value);

                        if (findResult && query.MaxRating.HasValue)
                            findResult = findResult && (playerRating <= query.MaxRating.Value);
                    }

                    if (findResult)
                    {
                        findPlayers.Add(new SwosFindPlayer
                        {
                            SwosFile = swosFile,
                            TeamId = teamId,
                            PlayerId = playerId
                        });
                    }
                }
        }

        return findPlayers;
    }
}