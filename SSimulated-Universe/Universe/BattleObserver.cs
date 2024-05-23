using SSimulated_Universe.Entities;
using SSimulated_Universe.Environment;

namespace SSimulated_Universe.Universe;

/// <summary>
/// Every entity and effect can listen to these events and do whatever they want.
/// <br/>
/// 正在观察战况的何同学……
/// </summary>
public class BattleObserver
{
    public virtual void EntityJoined(Entity entity) { }
    public virtual void EntityLeft(Entity entity) { }
    public virtual void EffectStarted(Effect effect) { }
    public virtual void EffectEnded(Effect effect) { }
    
    public virtual void BeforeTurnOf(Entity entity) { }
    
    /* These events are invoked as pairs:
     *
     *     BeforeTakeAttack  ────┐
     *                           │
     *         BeforeTakeHit ──┐ │
     *         AfterTakeHit  ──┘ │
     *                           │
     *         BeforeTakeHit ──┐ │
     *         AfterTakeHit  ──┘ │
     *                           │
     *         BeforeTakeHit ──┐ │
     *         AfterTakeHit  ──┘ │
     *             ......        │
     *                           │
     *     AfterTakeAttack   ────┘
     */
    public virtual void BeforeTakeAttack(Entity attacker, Entity target) { }
    public virtual void BeforeTakeHit(Entity attacker, Entity target) { }
    public virtual void AfterTakeHit(Entity attacker, Entity target) { }
    public virtual void AfterTakeAttack(Entity attacker, Entity target) { }
    
    public virtual void UnleashedBasicAttack(Entity subject) { }
    public virtual void UnleashedSkill(Entity subject) { }
    public virtual void UnleashedUltimate(Entity subject) { }
    public virtual void UnleashedFollowUp(Entity subject) { }
    
    public virtual void WeaknessBroken(Entity entity) { }
    public virtual void SkillPointChanged(Side side, int delta) { }
    public virtual void EnergyChanged(Entity entity, double delta) { }
    public virtual void HpChanged(Entity entity, double delta) { }
    public virtual void HpZeroed(Entity entity) { }
}
