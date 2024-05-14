namespace SSimulated_Universe.Modifiable.Number;

public class ModifiableDouble 
    : ModifiableNumber<double, ModifierDoubleImmediate, ModifierDoubleProportion>
{
    public ModifiableDouble(double @base) : base(@base) { }

    protected override double _evaluate()
    {
        var sumImmediates = _modifiersImmediate.Aggregate(0.0, (a, m) => a + m.Value);
        var sumProportions = _modifiersProportion.Aggregate(0.0, (a, m) => a + m.Value);
        var result = Base + sumImmediates + Base * sumProportions;

        return result;
    }
}
