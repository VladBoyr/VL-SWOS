using Swos.CareerMod.Database.Models;
using Swos.Domain.Models;

namespace Swos.CareerMod.Domain.Builders;

public interface ICoachBuilder
{
    Coach BuildFromOriginal(string coachName, SwosTactic tactic, SwosCountry teamCountry);
}

public class CoachBuilder : ICoachBuilder
{
    public Coach BuildFromOriginal(string coachName, SwosTactic tactic, SwosCountry teamCountry)
    {
        var random = new Random();

        const byte minAge = 30;
        const byte maxAge = 60;
        const byte minRating = 0;
        const byte maxRating = 49;

        return new Coach
        {
            Name = coachName,
            Age = (byte)random.Next(minAge, maxAge + 1),
            Country = teamCountry,
            Rating = (byte)random.Next(minRating, maxRating + 1),
            Tactic = tactic
        };
    }
}