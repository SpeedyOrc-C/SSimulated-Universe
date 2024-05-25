
using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Environment.WeaknessBreakEffects;

public class Entanglement : WeaknessBreakEffectTimed
{
    private const int MaxStackCount = 5;

    public int StackCount = 1;

    public Entanglement(Entity giver, Entity target, Battle battle) : base(giver, target, 1, battle) { }
    
    protected override double BaseDamage =>
        0.6 * StackCount * Giver.LevelMultiplier * Target.MaxToughnessMultiplier;

    protected override void WhenTriggered(Entity entity)
    {
        base.WhenTriggered(entity);

        if (StackCount < MaxStackCount)
            StackCount += 1;
    }
}