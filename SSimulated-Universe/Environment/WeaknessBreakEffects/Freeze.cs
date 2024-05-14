using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Environment.WeaknessBreakEffects;

public class Freeze : WeaknessBreakEffect
{
    public Freeze(Entity giver, Entity target, uint? duration, Battle battle)
        : base(giver, target, duration, battle) { }

    protected override double BaseDamage => Giver.LevelMultiplier;

    protected override void WhenTriggered(Entity entity)
    {
        base.WhenTriggered(entity);

        entity.SkipNextTurn = true;
        entity.Distance = 0.5;
    }
}