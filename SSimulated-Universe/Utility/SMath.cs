namespace SSimulated_Universe.Utility;

public static class SMath
{
    public static double LinearMap(double level, double minLevel, double maxLevel, double minValue, double maxValue) =>
        minValue + (level - minLevel) / (maxLevel - minLevel) * (maxValue - minValue);

    public static bool BernoulliTrial(double probability) =>
        1 - Random.Shared.NextDouble() <= probability;
}
