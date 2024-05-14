using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Environment.WeaknessBreakEffects;

public class Burn : WeaknessBreakEffect
{
    // public Burn(Battle.Battle battle) : base(battle) { }

    public Burn(Entity giver, Entity target, uint duration, Battle battle)
        : base(giver, target, duration, battle) { }

    public Burn(Entity giver, Entity target, Battle battle)
        : base(giver, target, 2, battle) { }

    protected override double BaseDamage => Giver.LevelMultiplier;
}