﻿
using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Environment.WeaknessBreakEffects;

public class Entanglement : WeaknessBreakEffect
{
    public static readonly int MaxStackCount = 5;
    
    public int StackCount = 1;

    public Entanglement(Entity giver, Entity target, uint? duration, Battle battle)
        : base(giver, target, duration, battle) { }
    
    protected override double BaseDamage =>
        0.6 * StackCount * Giver.LevelMultiplier * Target.MaxToughnessMultiplier;

    protected override void WhenTriggered(Entity entity)
    {
        base.WhenTriggered(entity);

        if (StackCount < MaxStackCount)
            StackCount += 1;
    }
}