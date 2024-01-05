namespace Swos.Domain.Models;

public enum SwosPlayerInjury : byte
{
    None = 0,
    HalfInjury = 1 * 32,
    InjuryFor1Game = 2 * 32,
    InjuryFor2Games = 3 * 32,
    InjuryFor3Games = 4 * 32,
    InjuryFor4Games = 5 * 32,
    InjuryForMoreGames = 6 * 32,
    InjuryForSeason = 7 * 32
}