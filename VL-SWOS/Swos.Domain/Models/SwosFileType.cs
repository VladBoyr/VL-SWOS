namespace Swos.Domain.Models;

public enum SwosFileType
{
    AnyTeam,
    EuropeClub,
    NorthAmericaClub,
    SouthAmericaClub,
    AfricaClub,
    AsiaClub,
    OceaniaClub,
    Club,
    NationalTeam,
    EurocupTeam,
    CustomTeam,
    OtherTeam
}

public static class SwosFileTypeExtension
{
    public static Dictionary<byte, string> GetSwosFiles(this IEnumerable<SwosFileType> fileTypes)
    {
        IEnumerable<KeyValuePair<byte, string>> list = new List<KeyValuePair<byte, string>>();

        foreach (var fileType in fileTypes)
        {
            switch (fileType)
            {
                case SwosFileType.AnyTeam:
                    list = list.Union(AnyTeams);
                    break;

                case SwosFileType.EuropeClub:
                    list = list.Union(EuropeClubs);
                    break;

                case SwosFileType.NorthAmericaClub:
                    list = list.Union(NorthAmericaClubs);
                    break;

                case SwosFileType.SouthAmericaClub:
                    list = list.Union(SouthAmericaClubs);
                    break;

                case SwosFileType.AfricaClub:
                    list = list.Union(AfricaClubs);
                    break;

                case SwosFileType.AsiaClub:
                    list = list.Union(AsiaClubs);
                    break;

                case SwosFileType.OceaniaClub:
                    list = list.Union(OceaniaClubs);
                    break;

                case SwosFileType.Club:
                    list = list.Union(AllClubs);
                    break;

                case SwosFileType.NationalTeam:
                    list = list.Union(NationalTeams);
                    break;

                case SwosFileType.EurocupTeam:
                    list = list.Union(EurocupTeams);
                    break;

                case SwosFileType.CustomTeam:
                    list = list.Union(CustomTeams);
                    break;

                case SwosFileType.OtherTeam:
                    list = list.Union(OtherTeams);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null);
            }
        }

        return list.Distinct().ToDictionary();
    }

    public static readonly Dictionary<byte, string> EuropeClubs = new()
    {
        {0, "team.000"},    // Албания
        {1, "team.001"},    // Австрия
        {2, "team.002"},    // Бельгия
        {3, "team.003"},    // Болгария
        {4, "team.004"},    // Хорватия
        {5, "team.005"},    // Кипр
        {6, "team.006"},    // Чехия
        {7, "team.007"},    // Дания
        {8, "team.008"},    // Англия
        {10, "team.010"},   // Эстония
        {11, "team.011"},   // Фареры
        {12, "team.012"},   // Финляндия
        {13, "team.013"},   // Франция
        {14, "team.014"},   // Германия
        {15, "team.015"},   // Греция
        {16, "team.016"},   // Венгрия
        {17, "team.017"},   // Исландия
        {18, "team.018"},   // Ирландия
        {19, "team.019"},   // Израель
        {20, "team.020"},   // Италия
        {21, "team.021"},   // Латвия
        {22, "team.022"},   // Литва
        {23, "team.023"},   // Люксембург
        {24, "team.024"},   // Мальта
        {25, "team.025"},   // Голландия
        {26, "team.026"},   // Сев.Ирландия
        {27, "team.027"},   // Норвегия
        {28, "team.028"},   // Польша
        {29, "team.029"},   // Португалия
        {30, "team.030"},   // Румыния
        {31, "team.031"},   // Россия
        {32, "team.032"},   // Сан-Марино
        {33, "team.033"},   // Шотландия
        {34, "team.034"},   // Словения
        {35, "team.035"},   // Испания
        {36, "team.036"},   // Швеция
        {37, "team.037"},   // Швейцария
        {38, "team.038"},   // Турция
        {39, "team.039"},   // Украина
        {40, "team.040"},   // Уэльс
        {41, "team.041"},   // Югославия
        {76, "team.076"},   // Беларуссия
        {78, "team.078"}    // Словакия
    };

    public static readonly Dictionary<byte, string> NorthAmericaClubs = new()
    {
        {47, "team.047"},   // Коста-Рика
        {51, "team.051"},   // Сальвадор (клубы)
        {60, "team.060"},   // Мексика
        {73, "team.073"}    // США
    };

    public static readonly Dictionary<byte, string> SouthAmericaClubs = new()
    {
        {43, "team.043"},   // Аргентина
        {45, "team.045"},   // Боливия
        {46, "team.046"},   // Бразилия
        {48, "team.048"},   // Чили
        {49, "team.049"},   // Колумбия
        {50, "team.050"},   // Эквадор
        {64, "team.064"},   // Парагвай
        {65, "team.065"},   // Перу
        {66, "team.066"},   // Суринам
        {71, "team.071"},   // Уругвай
        {77, "team.077"}    // Венесуэла
    };

    public static readonly Dictionary<byte, string> AfricaClubs = new()
    {
        {42, "team.042"},   // Алжир
        {69, "team.069"},   // ЮАР
        {79, "team.079"}    // Гана
    };

    public static readonly Dictionary<byte, string> AsiaClubs = new()
    {
        {55, "team.055"},   // Япония
        {67, "team.067"},   // Тайвань
        {75, "team.075"}    // Индия
    };

    public static readonly Dictionary<byte, string> OceaniaClubs = new()
    {
        {44, "team.044"},   // Австралия
        {62, "team.062"}    // Новая Зеландия
    };

    public static readonly Dictionary<byte, string> AllClubs = EuropeClubs
        .Union(NorthAmericaClubs)
        .Union(SouthAmericaClubs)
        .Union(AfricaClubs)
        .Union(AsiaClubs)
        .Union(OceaniaClubs)
        .ToDictionary();

    public static readonly Dictionary<byte, string> NationalTeams = new()
    {
        {80, "team.080"},   // Европа
        {81, "team.081"},   // Африка
        {82, "team.082"},   // Южная Америка
        {83, "team.083"},   // Северная Америка
        {84, "team.084"},   // Азия
        {85, "team.085"}    // Океания
    };

    public static readonly Dictionary<byte, string> EurocupTeams = new()
    {
    };

    public static readonly Dictionary<byte, string> CustomTeams = new()
    {
        {72, "team.072"},   // Original Custom Teams
        {152, "team.cus"}   // Edited Custom Teams
    };

    public static readonly Dictionary<byte, string> OtherTeams = new()
    {
        {57, "team.057"},
        {59, "team.059"},
        {68, "team.068"},
        {70, "team.070"},
        {74, "team.074"}
    };

    public static readonly Dictionary<byte, string> AnyTeams = AllClubs
        .Union(NationalTeams)
        .Union(EurocupTeams)
        .Union(CustomTeams)
        .Union(OtherTeams)
        .ToDictionary();
}