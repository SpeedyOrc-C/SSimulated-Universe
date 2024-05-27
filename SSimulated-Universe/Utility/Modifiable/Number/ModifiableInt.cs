namespace SSimulated_Universe.Utility.Modifiable.Number;

public class ModifiableInt 
    : ModifiableNumber<int, ModifierIntImmediate, ModifierIntProportion>
{
    public ModifiableInt(int @base) : base(@base) { }

    protected override int _evaluate()
    {
        var sumImmediates = ModifiersImmediate.Aggregate(0, (a, m) => a + m.Value);
        var sumProportions = ModifiersProportion.Aggregate(0.0, (a, m) => a + m.Value);
        var result = Base + sumImmediates + Math.Round(Base * sumProportions);
        
        return (int) result;
    }
}