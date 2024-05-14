namespace SSimulated_Universe.Modifiable.Number;

public sealed class ModifierDoubleProportion : ModifierNumber<ModifiableDouble>
{
    public readonly double Value;

    public ModifierDoubleProportion(double value)
    {
        Value = value;
    }

    protected override void Register(ModifiableDouble modifiable)
    {
        modifiable.AddModifierProportion(this);
    }

    protected override void Cancel(ModifiableDouble modifiable)
    {
        modifiable.RemoveModifierProportion(this);
    }
}