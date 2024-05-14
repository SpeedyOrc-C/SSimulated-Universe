using SSimulated_Universe.Entities;
using SSimulated_Universe.Environment;

namespace SSimulated_Universe.Universe;

/// <summary>
/// 正在观察战况的何同学……
/// </summary>
public class BattleObserver
{
    public virtual void BeforeTurnOf(Entity entity) { }
    
    public virtual void BeforeTakeAttack(Entity attacker, Entity target) { }
    public virtual void BeforeTakeHit(Entity attacker, Entity target) { }
    public virtual void AfterTakeHit(Entity attacker, Entity target) { }
    public virtual void AfterTakeAttack(Entity attacker, Entity target) { }
    
    public virtual void UnleashedBasicAttack(Entity subject) { }
    public virtual void UnleashedSkill(Entity subject) { }
    public virtual void UnleashedUltimate(Entity subject) { }
    public virtual void UnleashedFollowUp(Entity subject) { }

    public virtual void EffectEnded(Effect effect) { }

    public virtual void WeaknessBroken(Entity entity) { }
    public virtual void SkillPointChanged(Side side, int delta) { }
    public virtual void EnergyChanged(Entity entity, double delta) { }
    public virtual void HpChanged(Entity entity, double delta) { }
}
