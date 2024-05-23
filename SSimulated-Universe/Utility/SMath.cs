namespace SSimulated_Universe.Utility;

public static class SMath
{
    public static double LinearMap(double level, double minLevel, double maxLevel, double minValue, double maxValue) 
        => minValue + (level - minLevel) / (maxLevel - minLevel) * (maxValue - minValue);
}
