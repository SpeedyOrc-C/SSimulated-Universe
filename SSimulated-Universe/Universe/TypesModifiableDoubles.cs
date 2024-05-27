using SSimulated_Universe.Utility.Modifiable.Number;

namespace SSimulated_Universe.Universe;

public class TypesModifiableDoubles
{
    public readonly ModifiableDouble Physical;
    public readonly ModifiableDouble Fire;
    public readonly ModifiableDouble Ice;
    public readonly ModifiableDouble Lightning;
    public readonly ModifiableDouble Wind;
    public readonly ModifiableDouble Quantum;
    public readonly ModifiableDouble Imaginary;

    public TypesModifiableDoubles()
    {
        Physical = new ModifiableDouble(0);
        Fire = new ModifiableDouble(0);
        Ice = new ModifiableDouble(0);
        Lightning = new ModifiableDouble(0);
        Wind = new ModifiableDouble(0);
        Quantum = new ModifiableDouble(0);
        Imaginary = new ModifiableDouble(0);
    }

    public TypesModifiableDoubles(
        ModifiableDouble physical,
        ModifiableDouble fire,
        ModifiableDouble ice,
        ModifiableDouble lightning,
        ModifiableDouble wind,
        ModifiableDouble quantum,
        ModifiableDouble imaginary)
    {
        Physical = physical;
        Fire = fire;
        Ice = ice;
        Lightning = lightning;
        Wind = wind;
        Quantum = quantum;
        Imaginary = imaginary;
    }

    public ModifiableDouble Of(DamageType t) => t switch
    {
        DamageType.Physical  => Physical,
        DamageType.Fire      => Fire,
        DamageType.Ice       => Ice,
        DamageType.Lightning => Lightning,
        DamageType.Wind      => Wind,
        DamageType.Quantum   => Quantum,
        DamageType.Imaginary => Imaginary,
        _                    => throw new InvalidDamageType()
    };
}
