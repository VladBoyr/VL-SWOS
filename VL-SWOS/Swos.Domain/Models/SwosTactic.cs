namespace Swos.Domain.Models;

public enum SwosTactic : byte
{
    Tactic442 = 0,
    Tactic541 = 1,
    Tactic451 = 2,
    Tactic532 = 3,
    Tactic352 = 4,
    Tactic433 = 5,
    Tactic424 = 6,
    Tactic343 = 7,
    TacticSweep = 8,
    Tactic523 = 9,
    TacticAttack = 10,
    TacticDefend = 11,
    TacticUserA = 12,
    TacticUserB = 13,
    TacticUserC = 14,
    TacticUserD = 15,
    TacticUserE = 16,
    TacticUserF = 17
}

public static class SwosTacticExtension
{
    public static string ToString(this SwosTactic tactic)
    {
        return tactic switch
        {
            SwosTactic.Tactic442 => "4-4-2",
            SwosTactic.Tactic541 => "5-4-1",
            SwosTactic.Tactic451 => "4-5-1",
            SwosTactic.Tactic532 => "5-3-2",
            SwosTactic.Tactic352 => "3-5-2",
            SwosTactic.Tactic433 => "4-3-3",
            SwosTactic.Tactic424 => "4-2-4",
            SwosTactic.Tactic343 => "3-4-3",
            SwosTactic.TacticSweep => "SWEEP",
            SwosTactic.Tactic523 => "5-2-3",
            SwosTactic.TacticAttack => "ATTACK",
            SwosTactic.TacticDefend => "DEFEND",
            SwosTactic.TacticUserA => "USER A",
            SwosTactic.TacticUserB => "USER B",
            SwosTactic.TacticUserC => "USER C",
            SwosTactic.TacticUserD => "USER D",
            SwosTactic.TacticUserE => "USER E",
            SwosTactic.TacticUserF => "USER F",
            _ => throw new ArgumentOutOfRangeException(nameof(tactic), tactic, null)
        };
    }
}