using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Events;

public class BasicAttack<P> : PlayerAction<P> where P : Player
{
    public BasicAttack(P subject, Battle battle) : base(subject, battle) { }

    public override void Run()
    {
        Battle.SideOf(Subject).ChangeSkillPoint(1);
        Battle.Broadcast(o => o.UnleashedBasicAttack(Subject));
    }
}