namespace SSimulated_Universe.Utility;

public class SMath
{
    public static double LinearMap(double level, double minLevel, double maxLevel, double minValue, double maxValue) 
        => minValue + (level - minLevel) / (maxLevel - minLevel) * (maxValue - minValue);

    public static double LevelMap(double level, double maxLevel, double minValue, double maxValue)
        => LinearMap(level, 1, maxLevel, minValue, maxValue);
}
