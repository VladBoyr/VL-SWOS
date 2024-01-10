using Swos.CareerMod.Database.Models;
using Swos.CareerMod.Domain.Helpers;
using Swos.Domain.Models;

namespace Swos.CareerMod.Domain.Builders;

public interface ITeamBuilder
{
    Team BuildFromOriginal(SwosTeam originalTeam);
}

public class TeamBuilder : ITeamBuilder
{
    private readonly ITeamKitBuilder teamKitBuilder;
    private readonly ICoachBuilder coachBuilder;
    private readonly IPlayerBuilder playerBuilder;

    public TeamBuilder(ITeamKitBuilder teamKitBuilder, ICoachBuilder coachBuilder, IPlayerBuilder playerBuilder)
    {
        this.teamKitBuilder = teamKitBuilder;
        this.coachBuilder = coachBuilder;
        this.playerBuilder = playerBuilder;
    }

    private HashSet<byte> ratings = new();
    public Team BuildFromOriginal(SwosTeam originalTeam)
    {
        var teamRating = (byte)(originalTeam.Players.Sum(x => x.Rating) / originalTeam.Players.Length);

        var players = new List<Player>();
        foreach (var originalPlayer in originalTeam.Players)
        {
            players.Add(playerBuilder.BuildFromOriginal(originalPlayer, teamRating));
        }
        
        return new Team
        {
            GlobalId = originalTeam.GlobalId,
            LocalId = originalTeam.LocalId,
            FileName = originalTeam.FileName,
            Name = originalTeam.Name,
            Country = originalTeam.Country,
            HomeKit = teamKitBuilder.BuildFromOriginal(originalTeam.HomeKit),
            AwayKit = teamKitBuilder.BuildFromOriginal(originalTeam.AwayKit),
            Coach = coachBuilder.BuildFromOriginal(originalTeam.CoachName, originalTeam.Tactic, originalTeam.Country),
            Players = players,
            TeamRating = teamRating
        };
    }
}