namespace SSimulated_Universe.Utility.Modifiable.Number;

public class ModifiableDouble 
    : ModifiableNumber<double, ModifierDoubleImmediate, ModifierDoubleProportion>
{
    public ModifiableDouble(double @base) : base(@base) { }

    protected override double _evaluate()
    {
        var sumImmediates = ModifiersImmediate.Aggregate(0.0, (a, m) => a + m.Value);
        var sumProportions = ModifiersProportion.Aggregate(0.0, (a, m) => a + m.Value);
        var result = Base + sumImmediates + Base * sumProportions;

        return result;
    }
}
