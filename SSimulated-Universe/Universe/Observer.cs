﻿namespace SSimulated_Universe.Universe;

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

    public virtual void BeforeEvent(IEvent @event) { }
    public virtual void BeforePlayerAction(IPlayerAction @event) { }
    public virtual void BeforeTakeHit(Hit hit, Entity target) { }
    public virtual void AfterTakeHit(Hit hit, Entity target) { }
    public virtual void AfterEvent(IEvent @event) { }
    public virtual void AfterPlayerAction(IPlayerAction @event) { }

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

// public class VerboseObserver : Effect
// {
//     public override void EntityJoined(Entity entity) =>
//         Console.WriteLine("EntityJoined");
//     public override void EntityLeft(Entity entity) =>
//         Console.WriteLine("EntityLeft");
//     public override void EffectStarted(Effect effectTimed) =>
//         Console.WriteLine("EffectStarted");
//     public override void EffectEnded(Effect effectTimed) =>
//         Console.WriteLine("EffectEnded");
//     public override void BeforeTurnOf(Entity entity) =>
//         Console.WriteLine("BeforeTurnOf");
//     public override void BeforeTakeHit(Entity attacker, Hit hit, Entity target) =>
//         Console.WriteLine("BeforeTakeHit");
//     public override void AfterTakeHit(Entity attacker, Hit hit, Entity target) =>
//         Console.WriteLine("AfterTakeHit");
//     public override void BeforeAction(Entity subject, ActionType actionType) =>
//         Console.WriteLine("BeforeAction " + actionType);
//     public override void AfterAction(Entity subject, ActionType actionType) =>
//         Console.WriteLine("AfterAction" + actionType);
//     public override void WeaknessBroken(Entity entity) =>
//         Console.WriteLine("WeaknessBroken");
//     public override void SkillPointChanged(Side side, int delta) =>
//         Console.WriteLine("SkillPointChanged");
//     public override void EnergyChanged(Entity entity, double delta) =>
//         Console.WriteLine("EnergyChanged");
//     public override void HpChanged(Entity entity, double delta) =>
//         Console.WriteLine("HpChanged " + delta);
//     public override void HpZeroed(Entity entity) =>
//         Console.WriteLine("HpZeroed");
//     public override void Died(Entity entity) =>
//         Console.WriteLine("Died");
//
//     public override void Added() { }
//     public override void Removed() { }
//     public VerboseObserver(Battle battle) : base(battle) { }
// }
