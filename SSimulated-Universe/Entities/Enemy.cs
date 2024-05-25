using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Entities;

public abstract class Enemy : Entity
{
    public override double RealDefence => Level * 10 + 200;
    
    protected Enemy(Battle battle) : base(battle)
    {
    }
    
    /// <summary>
    /// All enemy will die by default.
    /// Override it to allow exceptions like resurrection. 
    /// </summary>
    protected override void HpZeroed()
    {
        if (LastDamageSource is null)
            throw new Exception("Cannot find out why this enemy died.");
        
        Battle.Broadcast(o => o.Died(this));
        Battle.Remove(this);
    }
}

public class EnemyIdle : Enemy
{
    public EnemyIdle(Battle battle) : base(battle) { }
    public override void YourTurn() { }
    protected override void CleanUp()
    { }
}
