using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Environment;

public abstract class EffectTimed : Effect
{
    /// <summary>
    /// All Effects automatically counts down.
    /// When overriding this method, make sure to call this base method.
    /// </summary>
    public override void BeforeTurnOf(Entity entity)
    {
        if (Duration > 1)
            Duration -= 1;
        else 
            _battle.Remove(this);
    }
    
    private uint Duration;

    protected EffectTimed(uint duration, Battle battle) : base(battle)
    {
        Duration = duration;

        battle.Add(this);
    }
}

public abstract class EffectTimedSelf<TSelf> : EffectTimed where TSelf : Entity
{
    protected readonly TSelf Self;

    protected EffectTimedSelf(TSelf self, Battle battle, uint duration) 
        : base(duration, battle)
        => Self = self;
}

