namespace SSimulated_Universe.Universe;

public abstract class Effect : BattleObserver
{
    /// <summary>
    /// Add this effect to the battle.
    /// </summary>
    public void Add() => Battle.Add(this);

    /// <summary>
    /// Remove this effect from the battle.
    /// </summary>
    public void Remove() => Battle.Remove(this);

    /// <summary>
    /// Do something after this effect is added.
    /// <br/>
    /// <b>Never call this by yourself.</b>
    /// </summary>
    public abstract void Added();

    /// <summary>
    /// Do something after this effect is removed.
    /// <br/>
    /// <b>If this effect modified any values, remember to stop modifying them here.</b>
    /// <br/>
    /// <b>Never call this by yourself.</b>
    /// </summary>
    public abstract void Removed();

    protected readonly Battle Battle;
    protected Effect(Battle battle) => Battle = battle;
}

public abstract class EffectTimed : Effect
{
    /// <summary>
    /// Decrease the duration by 1.
    /// If it reaches 0, remove it from the battle.
    /// </summary>
    public override void BeforeTurnOf(Entity entity)
    {
        if (_duration > 1)
            _duration -= 1;
        else
            Remove();
    }

    private uint _duration;

    protected EffectTimed(uint duration, Battle battle) : base(battle) => _duration = duration;
}

public abstract class EffectSelf<TSelf> : Effect
{
    protected readonly TSelf Self;

    protected EffectSelf(TSelf self, Battle battle) : base(battle) => Self = self;
}

public abstract class EffectTimedSelf<TSelf> : EffectTimed where TSelf : Entity
{
    protected readonly TSelf Self;

    protected EffectTimedSelf(TSelf self, Battle battle, uint duration) : base(duration, battle) => Self = self;
}
