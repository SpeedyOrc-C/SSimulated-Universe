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

    public virtual void CleanUp() => _battle.Remove(this);

    public override void BeforeTurnOf(Entity entity)
    {
        switch (Duration)
        {
            case > 1:
                Duration -= 1;
                break;

            case 1:
                CleanUp();
                break;

            case null:
                break;
        }
    }
}

public abstract class Debuff : Effect
{
    protected Debuff(Battle battle) : base(battle) { }
    
    protected Debuff(Entity giver, Entity target, uint? duration, Battle battle)
        : base(giver, target, duration, battle) { }
}

public abstract class Buff : Effect
{
    protected Buff(Battle battle) : base(battle) { }
    
    protected Buff(Entity giver, Entity target, uint? duration, Battle battle)
        : base(giver, target, duration, battle) { }
}
