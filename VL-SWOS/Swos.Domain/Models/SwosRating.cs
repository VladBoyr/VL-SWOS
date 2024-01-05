namespace Swos.Domain.Models;

public static class SwosRatingExtension
{
    public static int ToPrice(this byte rating)
    {
        return rating switch
        {
            0 => 25000,
            1 => 25000,
            2 => 30000,
            3 => 40000,
            4 => 50000,
            5 => 65000,
            6 => 75000,
            7 => 85000,
            8 => 100000,
            9 => 110000,
            10 => 130000,
            11 => 150000,
            12 => 160000,
            13 => 180000,
            14 => 200000,
            15 => 250000,
            16 => 300000,
            17 => 350000,
            18 => 450000,
            19 => 500000,
            20 => 550000,
            21 => 600000,
            22 => 650000,
            23 => 700000,
            24 => 750000,
            25 => 800000,
            26 => 850000,
            27 => 950000,
            28 => 1000000,
            29 => 1100000,
            30 => 1300000,
            31 => 1500000,
            32 => 1600000,
            33 => 1800000,
            34 => 1900000,
            35 => 2000000,
            36 => 2250000,
            37 => 2750000,
            38 => 3000000,
            39 => 3500000,
            40 => 4500000,
            41 => 5000000,
            42 => 6000000,
            43 => 7000000,
            44 => 8000000,
            45 => 9000000,
            46 => 10000000,
            47 => 12000000,
            48 => 15000000,
            49 => 15000000,
            _ => 0
        };
    }

    public static int ToBonusRating(this byte swosData)
    {
        if (swosData > 127)
            return -(256 - swosData) / 15;

        return swosData / 15;
    }

    public static byte FromBonusRating(this int bonusRating)
    {
        if (bonusRating < 0)
            return (byte)(256 - (-bonusRating * 15));

        return (byte)(bonusRating * 15);
    }
}