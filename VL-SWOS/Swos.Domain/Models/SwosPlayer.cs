﻿namespace Swos.Domain.Models;

public class SwosPlayer
{
    public const int SwosSize = 38;
    public const int MaxNameLength = 22;

    public byte Number { get; set; }
    public string Name { get; set; } = "";
    public SwosCountry Country { get; set; }
    public SwosFace Face { get; set; }
    public SwosPosition Position { get; set; }
    public Dictionary<SwosSkill, SwosSkillValue> Skills { get; set; } = [];
    public byte Rating { get; set; }
    public int Price => Rating.ToPrice();
    public Dictionary<SwosGoalType, byte> Goals { get; set; } = [];
    public SwosPlayerInjury Injury { get; set; }
    public SwosPlayerCard Card { get; set; }
    public SwosPlayerEurocupCard EurocupCard { get; set; }
    public int BonusRating { get; set; }
    public SwosPlayerTrial Trial { get; set; }
}