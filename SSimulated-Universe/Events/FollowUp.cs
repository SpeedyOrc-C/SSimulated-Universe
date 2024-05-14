using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Events;

public class FollowUp : PlayerAction
{
    public FollowUp(Player subject, Battle battle) : base(subject, battle)
    {
    }

    public override void Run()
    {
        Battle.Broadcast(o => o.UnleashedFollowUp(Subject));
    }
}