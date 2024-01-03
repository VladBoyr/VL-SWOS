﻿namespace Swos.Domain.Models;

public enum SwosCountry : byte
{
    ALB = 0,
    AUT = 1,
    BEL = 2,
    BUL = 3,
    CRO = 4,
    CYP = 5,
    TCH = 6,
    DEN = 7,
    ENG = 8,
    EST = 9,
    FAR = 10,
    FIN = 11,
    FRA = 12,
    GER = 13,
    GRE = 14,
    HUN = 15,
    ISL = 16,
    ISR = 17,
    ITA = 18,
    LAT = 19,
    LIT = 20,
    LUX = 21,
    MLT = 22,
    HOL = 23,
    NIR = 24,
    NOR = 25,
    POL = 26,
    POR = 27,
    ROM = 28,
    RUS = 29,
    SMR = 30,
    SCO = 31,
    SLO = 32,
    SWE = 33,
    TUR = 34,
    UKR = 35,
    WAL = 36,
    YUG = 37,
    BLS = 38,
    SVK = 39,
    ESP = 40,
    ARM = 41,
    BOS = 42,
    AZB = 43,
    GEO = 44,
    SUI = 45,
    IRL = 46,
    MAC = 47,
    TRK = 48,
    LIE = 49,
    MOL = 50,
    CRC = 51,
    SAL = 52,
    GUA = 53,
    HON = 54,
    BHM = 55,
    MEX = 56,
    PAN = 57,
    USA = 58,
    BAH = 59,
    NIC = 60,
    BER = 61,
    JAM = 62,
    TRI = 63,
    CAN = 64,
    BAR = 65,
    ELS = 66,
    SVC = 67,
    ARG = 68,
    BOL = 69,
    BRA = 70,
    CHL = 71,
    COL = 72,
    ECU = 73,
    PAR = 74,
    SUR = 75,
    URU = 76,
    VNZ = 77,
    GUY = 78,
    PER = 79,
    ALG = 80,
    SAF = 81,
    BOT = 82,
    BFS = 83,
    BUR = 84,
    LES = 85,
    ZAI = 86,
    ZAM = 87,
    GHA = 88,
    SEN = 89,
    CIV = 90,
    TUN = 91,
    MLI = 92,
    MDG = 93,
    CAM = 94,
    CHD = 95,
    UGA = 96,
    LIB = 97,
    MOZ = 98,
    KEN = 99,
    SUD = 100,
    SWA = 101,
    ANG = 102,
    TOG = 103,
    ZIM = 104,
    EGY = 105,
    TAN = 106,
    NIG = 107,
    ETH = 108,
    GAB = 109,
    SIE = 110,
    BEN = 111,
    CON = 112,
    GUI = 113,
    SRL = 114,
    MAR = 115,
    GAM = 116,
    MLW = 117,
    JAP = 118,
    TAI = 119,
    IND = 120,
    BAN = 121,
    BRU = 122,
    IRA = 123,
    JOR = 124,
    SRI = 125,
    SYR = 126,
    KOR = 127,
    IRN = 128,
    VIE = 129,
    MLY = 130,
    SAU = 131,
    YEM = 132,
    KUW = 133,
    LAO = 134,
    NKR = 135,
    OMA = 136,
    PAK = 137,
    PHI = 138,
    CHI = 139,
    SGP = 140,
    MAU = 141,
    MYA = 142,
    PAP = 143,
    TAD = 144,
    UZB = 145,
    QAT = 146,
    UAE = 147,
    AUS = 148,
    NZL = 149,
    FIJ = 150,
    SOL = 151,
    CUS = 152
}

public static class SwosCountryExtension
{
    public static string ToRussianString(this SwosCountry country)
    {
        return country switch
        {
            SwosCountry.ALB => "Албания",
            SwosCountry.AUT => "Австрия",
            SwosCountry.BEL => "Бельгия",
            SwosCountry.BUL => "Болгария",
            SwosCountry.CRO => "Хорватия",
            SwosCountry.CYP => "Кипр",
            SwosCountry.TCH => "Чехия",
            SwosCountry.DEN => "Дания",
            SwosCountry.ENG => "Англия",
            SwosCountry.EST => "Эстония",
            SwosCountry.FAR => "Фарерские острова",
            SwosCountry.FIN => "Финляндия",
            SwosCountry.FRA => "Франция",
            SwosCountry.GER => "Германия",
            SwosCountry.GRE => "Греция",
            SwosCountry.HUN => "Венгрия",
            SwosCountry.ISL => "Исландия",
            SwosCountry.ISR => "Израиль",
            SwosCountry.ITA => "Италия",
            SwosCountry.LAT => "Латвия",
            SwosCountry.LIT => "Литва",
            SwosCountry.LUX => "Люксембург",
            SwosCountry.MLT => "Мальта",
            SwosCountry.HOL => "Нидерланды",
            SwosCountry.NIR => "Северная Ирландия",
            SwosCountry.NOR => "Норвегия",
            SwosCountry.POL => "Польша",
            SwosCountry.POR => "Португалия",
            SwosCountry.ROM => "Румыния",
            SwosCountry.RUS => "Россия",
            SwosCountry.SMR => "Сан-Марино",
            SwosCountry.SCO => "Шотландия",
            SwosCountry.SLO => "Словения",
            SwosCountry.SWE => "Швеция",
            SwosCountry.TUR => "Турция",
            SwosCountry.UKR => "Украина",
            SwosCountry.WAL => "Уэльс",
            SwosCountry.YUG => "Югославия",
            SwosCountry.BLS => "Беларусь",
            SwosCountry.SVK => "Словакия",
            SwosCountry.ESP => "Испания",
            SwosCountry.ARM => "Армения",
            SwosCountry.BOS => "Босния",
            SwosCountry.AZB => "Азербайджан",
            SwosCountry.GEO => "Грузия",
            SwosCountry.SUI => "Швейцария",
            SwosCountry.IRL => "Ирландия",
            SwosCountry.MAC => "Македония",
            SwosCountry.TRK => "Туркменистан",
            SwosCountry.LIE => "Лихтенштейн",
            SwosCountry.MOL => "Молдова",
            SwosCountry.CRC => "Коста-Рика",
            SwosCountry.SAL => "Сальвадор (клубы)",
            SwosCountry.GUA => "Гватемала",
            SwosCountry.HON => "Гондурас/Гонконг",
            SwosCountry.BHM => "Багамские острова",
            SwosCountry.MEX => "Мексика",
            SwosCountry.PAN => "Панама",
            SwosCountry.USA => "США",
            SwosCountry.BAH => "Бахрейн",
            SwosCountry.NIC => "Никарагуа",
            SwosCountry.BER => "Бермудские острова",
            SwosCountry.JAM => "Ямайка",
            SwosCountry.TRI => "Тринидад и Тобаго",
            SwosCountry.CAN => "Канада",
            SwosCountry.BAR => "Барбадос",
            SwosCountry.ELS => "Сальвадор (сборная)",
            SwosCountry.SVC => "Сент-Винсент и Гренадины",
            SwosCountry.ARG => "Аргентина",
            SwosCountry.BOL => "Боливия",
            SwosCountry.BRA => "Бразилия",
            SwosCountry.CHL => "Чили",
            SwosCountry.COL => "Колумбия",
            SwosCountry.ECU => "Эквадор",
            SwosCountry.PAR => "Парагвай",
            SwosCountry.SUR => "Суринам",
            SwosCountry.URU => "Уругвай",
            SwosCountry.VNZ => "Венесуэла",
            SwosCountry.GUY => "Гайана",
            SwosCountry.PER => "Перу",
            SwosCountry.ALG => "Алжир",
            SwosCountry.SAF => "ЮАР",
            SwosCountry.BOT => "Ботсвана",
            SwosCountry.BFS => "Буркина-Фасо",
            SwosCountry.BUR => "Бурунди",
            SwosCountry.LES => "Лесото",
            SwosCountry.ZAI => "Заир",
            SwosCountry.ZAM => "Замбия",
            SwosCountry.GHA => "Гана",
            SwosCountry.SEN => "Сенегал",
            SwosCountry.CIV => "Кот-д'Ивуар",
            SwosCountry.TUN => "Тунис",
            SwosCountry.MLI => "Мали",
            SwosCountry.MDG => "Мадагаскар",
            SwosCountry.CAM => "Камерун",
            SwosCountry.CHD => "Чад",
            SwosCountry.UGA => "Уганда",
            SwosCountry.LIB => "Либерия",
            SwosCountry.MOZ => "Мозамбик",
            SwosCountry.KEN => "Кения",
            SwosCountry.SUD => "Судан",
            SwosCountry.SWA => "Свазиленд",
            SwosCountry.ANG => "Ангола",
            SwosCountry.TOG => "Того",
            SwosCountry.ZIM => "Зимбабве",
            SwosCountry.EGY => "Египет",
            SwosCountry.TAN => "Танзания",
            SwosCountry.NIG => "Нигер/Нигерия",
            SwosCountry.ETH => "Эфиопия",
            SwosCountry.GAB => "Габон",
            SwosCountry.SIE => "Сьера-Леоне (сборная)",
            SwosCountry.BEN => "Бенин",
            SwosCountry.CON => "Конго",
            SwosCountry.GUI => "Гвинея",
            SwosCountry.SRL => "Сьера-Леоне (клубы)",
            SwosCountry.MAR => "Марокко",
            SwosCountry.GAM => "Гамбия",
            SwosCountry.MLW => "Малави",
            SwosCountry.JAP => "Япония",
            SwosCountry.TAI => "Тайвань",
            SwosCountry.IND => "Индия/Индонезия",
            SwosCountry.BAN => "Бангладеш",
            SwosCountry.BRU => "Бруней",
            SwosCountry.IRA => "Ирак/Иран (сборная)",
            SwosCountry.JOR => "Иордания",
            SwosCountry.SRI => "Шри-Ланка",
            SwosCountry.SYR => "Сирия",
            SwosCountry.KOR => "Южная Корея",
            SwosCountry.IRN => "Иран (клубы)",
            SwosCountry.VIE => "Вьетнам",
            SwosCountry.MLY => "Малайзия",
            SwosCountry.SAU => "Саудовская Аравия",
            SwosCountry.YEM => "Йемен",
            SwosCountry.KUW => "Кувейт",
            SwosCountry.LAO => "Лаос",
            SwosCountry.NKR => "Северная Корея",
            SwosCountry.OMA => "Оман",
            SwosCountry.PAK => "Пакистан",
            SwosCountry.PHI => "Филиппины",
            SwosCountry.CHI => "Китай",
            SwosCountry.SGP => "Сингапур",
            SwosCountry.MAU => "Мавритания/Маврикий",
            SwosCountry.MYA => "Мьянма",
            SwosCountry.PAP => "Папуа-Новая Гвинея",
            SwosCountry.TAD => "Таджикистан",
            SwosCountry.UZB => "Узбекистан",
            SwosCountry.QAT => "Катар",
            SwosCountry.UAE => "ОАЭ",
            SwosCountry.AUS => "Австралия",
            SwosCountry.NZL => "Новая Зеландия",
            SwosCountry.FIJ => "Фиджи",
            SwosCountry.SOL => "Соломоновы острова",
            SwosCountry.CUS => "Кастомная страна",
            _ => throw new ArgumentOutOfRangeException(nameof(country), country, null)
        };
    }

    public static SwosCountry CountryChangedBySwosCommunity(this SwosCountry country)
    {
        return country switch
        {
            SwosCountry.BOS => SwosCountry.FAR,
            SwosCountry.FAR => SwosCountry.BOS,

            SwosCountry.MAC => SwosCountry.MLT,
            SwosCountry.MLT => SwosCountry.MAC,

            SwosCountry.SMR => SwosCountry.YUG,
            SwosCountry.YUG => SwosCountry.SMR,

            SwosCountry.CRC => SwosCountry.VNZ,
            SwosCountry.VNZ => SwosCountry.CRC,

            SwosCountry.SAL => SwosCountry.USA,
            SwosCountry.USA => SwosCountry.SAL,

            SwosCountry.HON => SwosCountry.SUR,
            SwosCountry.SUR => SwosCountry.HON,

            SwosCountry.ALG => SwosCountry.NIG,
            SwosCountry.NIG => SwosCountry.ALG,

            SwosCountry.CAM => SwosCountry.SAF,
            SwosCountry.SAF => SwosCountry.CAM,

            SwosCountry.GHA => SwosCountry.SEN,
            SwosCountry.SEN => SwosCountry.GHA,

            SwosCountry.MAR => SwosCountry.NZL,
            SwosCountry.NZL => SwosCountry.MAR,

            _ => country
        };
    }

    public static SwosCountry ClubCountryToCountry(this byte clubCountry)
    {
        return clubCountry switch
        {
            0 => SwosCountry.ALB,
            1 => SwosCountry.AUT,
            2 => SwosCountry.BEL,
            3 => SwosCountry.BUL,
            4 => SwosCountry.CRO,
            5 => SwosCountry.CYP,
            6 => SwosCountry.TCH,
            7 => SwosCountry.DEN,
            8 => SwosCountry.ENG,
            10 => SwosCountry.EST,
            11 => SwosCountry.FAR,
            12 => SwosCountry.FIN,
            13 => SwosCountry.FRA,
            14 => SwosCountry.GER,
            15 => SwosCountry.GRE,
            16 => SwosCountry.HUN,
            17 => SwosCountry.ISL,
            18 => SwosCountry.IRL,
            19 => SwosCountry.ISR,
            20 => SwosCountry.ITA,
            21 => SwosCountry.LAT,
            22 => SwosCountry.LIT,
            23 => SwosCountry.LUX,
            24 => SwosCountry.MLT,
            25 => SwosCountry.HOL,
            26 => SwosCountry.NIR,
            27 => SwosCountry.NOR,
            28 => SwosCountry.POL,
            29 => SwosCountry.POR,
            30 => SwosCountry.ROM,
            31 => SwosCountry.RUS,
            32 => SwosCountry.SMR,
            33 => SwosCountry.SCO,
            34 => SwosCountry.SLO,
            35 => SwosCountry.ESP,
            36 => SwosCountry.SWE,
            37 => SwosCountry.SUI,
            38 => SwosCountry.TUR,
            39 => SwosCountry.UKR,
            40 => SwosCountry.WAL,
            41 => SwosCountry.YUG,
            42 => SwosCountry.ALG,
            43 => SwosCountry.ARG,
            44 => SwosCountry.AUS,
            45 => SwosCountry.BOL,
            46 => SwosCountry.BRA,
            47 => SwosCountry.CRC,
            48 => SwosCountry.CHL,
            49 => SwosCountry.COL,
            50 => SwosCountry.ECU,
            51 => SwosCountry.SAL,
            55 => SwosCountry.JAP,
            60 => SwosCountry.MEX,
            62 => SwosCountry.NZL,
            64 => SwosCountry.PAR,
            65 => SwosCountry.PER,
            66 => SwosCountry.SUR,
            67 => SwosCountry.TAI,
            69 => SwosCountry.SAF,
            71 => SwosCountry.URU,
            72 => SwosCountry.CUS,
            73 => SwosCountry.USA,
            75 => SwosCountry.IND,
            76 => SwosCountry.BLS,
            77 => SwosCountry.VNZ,
            78 => SwosCountry.SVK,
            79 => SwosCountry.GHA,
            _ => SwosCountry.CUS
        };
    }

    public static byte CountryToClubCountry(this SwosCountry country)
    {
        return country switch
        {
            SwosCountry.ALB => 0,
            SwosCountry.AUT => 1,
            SwosCountry.BEL => 2,
            SwosCountry.BUL => 3,
            SwosCountry.CRO => 4,
            SwosCountry.CYP => 5,
            SwosCountry.TCH => 6,
            SwosCountry.DEN => 7,
            SwosCountry.ENG => 8,
            SwosCountry.EST => 10,
            SwosCountry.FAR => 11,
            SwosCountry.FIN => 12,
            SwosCountry.FRA => 13,
            SwosCountry.GER => 14,
            SwosCountry.GRE => 15,
            SwosCountry.HUN => 16,
            SwosCountry.ISL => 17,
            SwosCountry.ISR => 19,
            SwosCountry.ITA => 20,
            SwosCountry.LAT => 21,
            SwosCountry.LIT => 22,
            SwosCountry.LUX => 23,
            SwosCountry.MLT => 24,
            SwosCountry.HOL => 25,
            SwosCountry.NIR => 26,
            SwosCountry.NOR => 27,
            SwosCountry.POL => 28,
            SwosCountry.POR => 29,
            SwosCountry.ROM => 30,
            SwosCountry.RUS => 31,
            SwosCountry.SMR => 32,
            SwosCountry.SCO => 33,
            SwosCountry.SLO => 34,
            SwosCountry.SWE => 36,
            SwosCountry.TUR => 38,
            SwosCountry.UKR => 39,
            SwosCountry.WAL => 40,
            SwosCountry.YUG => 41,
            SwosCountry.BLS => 76,
            SwosCountry.SVK => 78,
            SwosCountry.ESP => 35,
            SwosCountry.SUI => 37,
            SwosCountry.IRL => 18,
            SwosCountry.CRC => 47,
            SwosCountry.SAL => 51,
            SwosCountry.MEX => 60,
            SwosCountry.USA => 73,
            SwosCountry.ARG => 43,
            SwosCountry.BOL => 45,
            SwosCountry.BRA => 46,
            SwosCountry.CHL => 48,
            SwosCountry.COL => 49,
            SwosCountry.ECU => 50,
            SwosCountry.PAR => 64,
            SwosCountry.SUR => 66,
            SwosCountry.URU => 71,
            SwosCountry.VNZ => 77,
            SwosCountry.PER => 65,
            SwosCountry.ALG => 42,
            SwosCountry.SAF => 69,
            SwosCountry.GHA => 79,
            SwosCountry.JAP => 55,
            SwosCountry.TAI => 67,
            SwosCountry.IND => 75,
            SwosCountry.AUS => 44,
            SwosCountry.NZL => 62,
            _ => 72
        };
    }

    public readonly static HashSet<SwosCountry> CountryShortname = [SwosCountry.POR, SwosCountry.ESP, SwosCountry.BRA];
}