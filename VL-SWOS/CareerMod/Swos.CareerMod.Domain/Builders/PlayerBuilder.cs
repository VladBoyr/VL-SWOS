using Swos.CareerMod.Database.Models;
using Swos.CareerMod.Domain.Helpers;
using Swos.Domain.Models;

namespace Swos.CareerMod.Domain.Builders;

public interface IPlayerBuilder
{
    Player BuildFromOriginal(SwosPlayer originalPlayer, byte teamRating);
}

public class PlayerBuilder : IPlayerBuilder
{
    private readonly ISkillsBuilder skillsBuilder;

    public PlayerBuilder(ISkillsBuilder skillsBuilder)
    {
        this.skillsBuilder = skillsBuilder;
    }

    public Player BuildFromOriginal(SwosPlayer originalPlayer, byte teamRating)
    {
        var random = new Random();

        var maxPlayerAge = 33 + AgeHelper.PositionAgeBonus(originalPlayer.Position);
        var minPlayerAge = originalPlayer.Rating * 10 / (teamRating + 1);
        if (minPlayerAge > 20)
            minPlayerAge = 20;
        minPlayerAge = minPlayerAge + 13 + AgeHelper.PositionAgeBonus(originalPlayer.Position);

        return new Player
        {
            Number = originalPlayer.Number,
            Name = originalPlayer.Name,
            Age = (byte)random.Next(0, maxPlayerAge + 1),
            Country = originalPlayer.Country,
            Face = originalPlayer.Face,
            Position = originalPlayer.Position,
            Skills = skillsBuilder.BuildFromOriginal(originalPlayer.Skills).ToArray(),
            // YouthRating = ,
            Rating = originalPlayer.Rating,
            BonusRating = 0
        };

        /*
         YearBegin := YearBegin + 13 - ((Teams[Nm,Num].Player[NumPl].Amplua + 2) div 3);
         if YearBegin < 16 then YearBegin := 16;
         if YearBegin > 30 then YearBegin := 30;
         Teams[Nm,Num].Player[NumPl].Age:=Rand(YearBegin,30);
         Youth := Rand(0,10) - (Teams[Nm,Num].Player[NumPl].Age - 16);
         if Youth < 0 then Youth := 0;
         Teams[Nm,Num].Player[NumPl].YouthRating := Youth;
         */
    }
}