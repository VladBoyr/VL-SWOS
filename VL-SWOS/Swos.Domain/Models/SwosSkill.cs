namespace Swos.Domain.Models;

public enum SwosSkill : byte
{
    Passing = 0,
    Shooting = 1,
    Heading = 2,
    Tackling = 3,
    BallControl = 4,
    Speed = 5,
    Finishing = 6
}

public static class SwosSkillExtension
{
    public static char ToChar(this SwosSkill skill)
    {
        return skill switch
        {
            SwosSkill.Passing => 'P',
            SwosSkill.Shooting => 'V',
            SwosSkill.Heading => 'H',
            SwosSkill.Tackling => 'T',
            SwosSkill.BallControl => 'C',
            SwosSkill.Speed => 'S',
            SwosSkill.Finishing => 'F',
            _ => throw new ArgumentOutOfRangeException(nameof(skill), skill, null)
        };
    }

    public static string ToRussianString(this SwosSkill skill)
    {
        return skill switch
        {
            SwosSkill.Passing => "Пас",
            SwosSkill.Shooting => "Удар",
            SwosSkill.Heading => "Игра головой",
            SwosSkill.Tackling => "Отбор",
            SwosSkill.BallControl => "Контроль мяча",
            SwosSkill.Speed => "Скорость",
            SwosSkill.Finishing => "Завершение",
            _ => throw new ArgumentOutOfRangeException(nameof(skill), skill, null)
        };
    }
}