using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Events;

public abstract class FollowUp<P> : PlayerAction<P> where P : Player
{
    public FollowUp(P subject, Battle battle) : base(subject, battle)
    {
    }

    public override void Run()
    {
        Battle.Broadcast(o => o.UnleashedFollowUp(Subject));
    }
}