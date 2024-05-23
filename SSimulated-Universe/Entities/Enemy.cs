using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Entities;

public abstract class Enemy : Entity
{
    protected Enemy(Battle battle) : base(battle)
    {
    }
    
    public override void HpZeroed(Entity entity)
    {
        // Normal enemies will leave (die) when their HP reach zero.
        if (entity == this)
            Battle.Remove(this);
    }
}
