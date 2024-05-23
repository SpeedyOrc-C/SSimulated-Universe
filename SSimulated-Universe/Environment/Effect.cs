using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Environment;

public abstract class Effect : BattleObserver
{
    private uint? Duration;
    private readonly Battle _battle;

    protected Effect(Battle battle, uint? duration = null)
    {
        Duration = duration;
        _battle = battle;

        battle.Add(this);
    }

    /// <summary>
    /// When an effect is removed, all the values this effect modified should be recovered.
    /// So all effects with modifiers must recover the values here.
    /// By default, we assume an effect have no need to recover.
    /// </summary>
    public virtual void CleanUp() { }

    /// <summary>
    /// All Effects automatically counts down.
    /// When overriding this method, make sure to call this base method.
    /// </summary>
    public override void BeforeTurnOf(Entity entity)
    {
        switch (Duration)
        {
            case > 1:
                Duration -= 1;
                break;

            case 1:
                _battle.Remove(this);
                break;

            case null:
                break;
        }
    }
}

public abstract class SelfEffect<T> : Effect where T : Entity
{
    protected readonly T Self;

    protected SelfEffect(T self, Battle battle, uint? duration = null) : base(battle, duration)
    {
        Self = self;
    }
}

