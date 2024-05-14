using SSimulated_Universe.Events;
using SSimulated_Universe.Modifiable.Number;

namespace SSimulated_Universe.Entities;

public class DamageTypeModifiableDoubles
{
    public readonly ModifiableDouble Physical;
    public readonly ModifiableDouble Fire;
    public readonly ModifiableDouble Ice;
    public readonly ModifiableDouble Lightning;
    public readonly ModifiableDouble Wind;
    public readonly ModifiableDouble Quantum;
    public readonly ModifiableDouble Imaginary;

    public DamageTypeModifiableDoubles()
    {
        Physical = new(0);
        Fire = new(0);
        Ice = new(0);
        Lightning = new(0);
        Wind = new(0);
        Quantum = new(0);
        Imaginary = new(0);
    }

    public DamageTypeModifiableDoubles(ModifiableDouble physical, ModifiableDouble fire, ModifiableDouble ice,
        ModifiableDouble lightning, ModifiableDouble wind, ModifiableDouble quantum, ModifiableDouble imaginary)
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
        _ => throw new ArgumentOutOfRangeException(nameof(t), t, "Invalid damage type.")
    };
}
