using SSimulated_Universe.Entities;
using SSimulated_Universe.Events;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Environment.WeaknessBreakEffects;

public abstract class WeaknessBreakEffect : Debuff
{
    protected WeaknessBreakEffect(Battle battle) : base(battle) { }
    
    protected WeaknessBreakEffect(Entity giver, Entity target, uint? duration, Battle battle)
        : base(giver, target, duration, battle) { }

    private double Damage =>
          BaseDamage
        * (1 + Giver.BreakEffect.Eval)
        * Giver.DefenceMultiplier(Target)
        * Giver.ResistanceMultiplier(Target, DamageType.Physical)
        * Target.VulnerabilityMultiplier(DamageType.Physical)
        * Target.BrokenMultiplier;

    protected abstract double BaseDamage { get; }

    protected virtual void WhenTriggered(Entity entity) => entity.TakeDamage(Damage);
}