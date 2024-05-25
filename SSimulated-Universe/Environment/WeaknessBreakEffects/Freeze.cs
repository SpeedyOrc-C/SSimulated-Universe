using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Environment.WeaknessBreakEffects;

public class Freeze : WeaknessBreakEffectTimed
{
    public Freeze(Entity giver, Entity target, Battle battle) : base(giver, target, 1, battle) { }

    protected override double BaseDamage => Giver.LevelMultiplier;

    protected override void WhenTriggered(Entity entity)
    {
        base.WhenTriggered(entity);

        entity.SkipNextTurn = true;
        entity.Distance = 0.5;
    }
}