using SSimulated_Universe.Entities;
using SSimulated_Universe.Modifiable.Number;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Environment;

public abstract class Effect : BattleObserver
{
    public Entity Giver;
    public Entity Target;
    public uint? Duration;
    private readonly Battle _battle;

    public Effect(Battle battle)
    {
        _battle = battle;
    }

    public Effect(Entity giver, Entity target, uint? duration, Battle battle)
    {
        Giver = giver;
        Target = target;
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
    /// 
    /// </summary>
    /// <param name="entity"></param>
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
