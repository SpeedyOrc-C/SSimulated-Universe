using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Environment.WeaknessBreakEffects;

public class Shock : WeaknessBreakEffectTimed
{
    public Shock(Entity giver, Entity target, Battle battle) : base(giver, target, 2, battle) { }

    protected override double BaseDamage => 2 * Giver.LevelMultiplier;
}