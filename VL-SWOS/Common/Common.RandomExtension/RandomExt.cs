namespace Common.RandomExtension;

public class RandomExt
{
    public static T RandomValueByRatio<T>((T, double)[] valueAndRatioList)
    {
        if (valueAndRatioList == null)
            throw new NullReferenceException($"Значение '{nameof(valueAndRatioList)}' не указано");

        if (valueAndRatioList.Length == 0)
            throw new IndexOutOfRangeException($"Пустой '{nameof(valueAndRatioList)}' не допустим");

        if (valueAndRatioList.Length == 1)
            return valueAndRatioList.Single().Item1;

        var values = valueAndRatioList.Select(x => x.Item1).ToArray();
        var ratioList = NormalizeRatio(valueAndRatioList.Select(x => x.Item2).ToArray());

        var random = new Random().NextDouble();
        var index = 0;
        while (random > ratioList[index] && index < (valueAndRatioList.Length - 1))
        {
            random -= ratioList[index];
            index++;
        }

        return valueAndRatioList[index].Item1;
    }

    private static double[] NormalizeRatio(double[] ratioList)
    {
        if (ratioList.Any(x => x < 0))
            throw new ($"Отрицательный коэффициент в '{nameof(ratioList)}' не допустим");

        var ratioSum = ratioList.Sum();
        if (ratioSum == 0)
            throw new DivideByZeroException($"Сумма коэффициентов '{nameof(ratioList)}', равная 0, не допустима");

        var result = new double[ratioList.Length];
        for (var i = 0; i < result.Length; i++)
        {
            result[i] = ratioList[i] / ratioSum;
        }
        return result;
    }
}