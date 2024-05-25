using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Environment;

public abstract class Effect : BattleObserver
{
    /// <summary>
    /// When an effect is removed, all the values this effect modified should be recovered.
    /// So all effects with modifiers must recover the values here.
    /// By default, we assume an effect have no need to recover.
    /// </summary>
    public virtual void CleanUp() { }

    protected readonly Battle _battle;
    protected Effect(Battle battle) => _battle = battle;
}

public abstract class EffectSelf<TSelf> : Effect
{
    protected readonly TSelf Self;

    protected EffectSelf(TSelf self, Battle battle) : base(battle) => Self = self;
}