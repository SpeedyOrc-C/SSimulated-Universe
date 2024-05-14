
using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Environment.WeaknessBreakEffects;

public class WindShear : WeaknessBreakEffect
{
    public int StackCount;

    public WindShear(Entity giver, Entity target, int stackCount, uint duration, Battle battle)
        : base(giver, target, duration, battle)
    {
        StackCount = stackCount;
    }

    public WindShear(Entity giver, Entity target, int stackCount, Battle battle)
        : base(giver, target, 2, battle)
    {
        StackCount = stackCount;
    }

    protected override double BaseDamage => StackCount * Giver.LevelMultiplier;
}