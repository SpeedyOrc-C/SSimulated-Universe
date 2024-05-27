namespace SSimulated_Universe.Utility.Modifiable.Number;

public sealed class ModifierDoubleImmediate : ModifierNumber<ModifiableDouble>
{
    public double Value;

    public ModifierDoubleImmediate(double value)
    {
        Value = value;
    }

    protected override void Register(ModifiableDouble modifiable)
    {
        modifiable.AddModifierImmediate(this);
    }

    protected override void Cancel(ModifiableDouble modifiable)
    {
        modifiable.RemoveModifierImmediate(this);
    }
}