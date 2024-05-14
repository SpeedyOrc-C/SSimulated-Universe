using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Events;

public class Ultimate : PlayerAction
{
    public Ultimate(Player subject, Battle battle) : base(subject, battle) { }

    public override void Run()
    {
        Battle.Broadcast(o => o.UnleashedUltimate(Subject));
    }
}
