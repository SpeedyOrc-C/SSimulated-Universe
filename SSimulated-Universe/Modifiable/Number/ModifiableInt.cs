namespace SSimulated_Universe.Modifiable.Number;

public class ModifiableInt 
    : ModifiableNumber<int, ModifierIntImmediate, ModifierIntProportion>
{
    public ModifiableInt(int @base) : base(@base) { }

    protected override int _evaluate()
    {
        var sumImmediates = _modifiersImmediate.Aggregate(0, (a, m) => a + m.Value);
        var sumProportions = _modifiersProportion.Aggregate(0.0, (a, m) => a + m.Value);
        var result = Base + sumImmediates + Math.Round(Base * sumProportions);
        
        return (int) result;
    }
}