namespace SSimulated_Universe.Modifiable.Number;

public sealed class ModifierIntProportion : ModifierNumber<ModifiableInt>
{
    public readonly double Value;

    public ModifierIntProportion(double value)
    {
        Value = value;
    }

    protected override void Register(ModifiableInt modifiable)
    {
        modifiable.AddModifierProportion(this);
    }

    protected override void Cancel(ModifiableInt modifiable)
    {
        modifiable.RemoveModifierProportion(this);
    }
}