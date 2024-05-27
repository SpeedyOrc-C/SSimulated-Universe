namespace SSimulated_Universe.Universe;

/// <summary>
/// Every entity and effect can listen to these events and
/// do whatever they want by overriding them.
/// Observers do nothing by default.
/// <br/>
/// 正在观察战况的何同学……
/// </summary>
public class BattleObserver
{
    public virtual void EntityJoined(Entity entity) { }
    /// <summary>
    /// An entity left from the battle.
    /// This does not mean it has died.
    /// </summary>
    public virtual void EntityLeft(Entity entity) { }
    public virtual void EffectStarted(Effect effectTimed) { }
    public virtual void EffectEnded(Effect effectTimed) { }

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
    /// <summary>
    /// An entity's HP fell to zero.
    /// This does not mean it has died.
    /// </summary>
    public virtual void HpZeroed(Entity entity) { }
    public virtual void Died(Entity entity) { }
}

public class VerboseObserver : Effect
{
    public override void EntityJoined(Entity entity) => 
        Console.WriteLine("EntityJoined");
    public override void EntityLeft(Entity entity) => 
        Console.WriteLine("EntityLeft");
    public override void EffectStarted(Effect effectTimed) => 
        Console.WriteLine("EffectStarted");
    public override void EffectEnded(Effect effectTimed) => 
        Console.WriteLine("EffectEnded");
    public override void BeforeTurnOf(Entity entity) => 
        Console.WriteLine("BeforeTurnOf");
    public override void BeforeTakeAttack(Entity attacker, Entity target) => 
        Console.WriteLine("BeforeTakeAttack");
    public override void BeforeTakeHit(Entity attacker, Entity target) => 
        Console.WriteLine("BeforeTakeHit");
    public override void AfterTakeHit(Entity attacker, Entity target) => 
        Console.WriteLine("AfterTakeHit");
    public override void AfterTakeAttack(Entity attacker, Entity target) => 
        Console.WriteLine("AfterTakeAttack");
    public override void UnleashedBasicAttack(Entity subject) => 
        Console.WriteLine("UnleashedBasicAttack");
    public override void UnleashedSkill(Entity subject) => 
        Console.WriteLine("UnleashedSkill");
    public override void UnleashedUltimate(Entity subject) => 
        Console.WriteLine("UnleashedUltimate");
    public override void UnleashedFollowUp(Entity subject) => 
        Console.WriteLine("UnleashedFollowUp");
    public override void WeaknessBroken(Entity entity) => 
        Console.WriteLine("WeaknessBroken");
    public override void SkillPointChanged(Side side, int delta) => 
        Console.WriteLine("SkillPointChanged");
    public override void EnergyChanged(Entity entity, double delta) => 
        Console.WriteLine("EnergyChanged");
    public override void HpChanged(Entity entity, double delta) => 
        Console.WriteLine("HpChanged " + delta);
    public override void HpZeroed(Entity entity) => 
        Console.WriteLine("HpZeroed");
    public override void Died(Entity entity) => 
        Console.WriteLine("Died");

    public override void Added() { }
    public override void Removed() { }
    public VerboseObserver(Battle battle) : base(battle) { }
}
