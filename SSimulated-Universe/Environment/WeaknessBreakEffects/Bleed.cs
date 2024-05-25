using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Environment.WeaknessBreakEffects;

public class Bleed : WeaknessBreakEffectTimed
{
    public Bleed(Entity giver, Entity target, Battle battle) : base(giver, target, 2, battle) { }

    protected override double BaseDamage
    {
        get
        {
            var cap = 2 * Giver.LevelMultiplier * Giver.MaxToughnessMultiplier;
            var uncappedBaseDamage = Target.MaxHp.Eval * (Target.IsEliteOrBoss ? 0.07 : 0.16);
            return Math.Min(cap, uncappedBaseDamage);
        }
    }
}