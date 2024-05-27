namespace SSimulated_Universe.Utility.Modifiable.Number;

public sealed class ModifierIntImmediate : ModifierNumber<ModifiableInt>
{
    public readonly int Value;

    public ModifierIntImmediate(int value)
    {
        Value = value;
    }

    protected override void Register(ModifiableInt modifiable)
    {
        modifiable.AddModifierImmediate(this);
    }

    protected override void Cancel(ModifiableInt modifiable)
    {
        modifiable.RemoveModifierImmediate(this);
    }
}