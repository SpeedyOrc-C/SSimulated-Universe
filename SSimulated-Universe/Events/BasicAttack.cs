using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Events;

public class BasicAttack : PlayerAction
{
    public BasicAttack(Player subject, Battle battle) : base(subject, battle) { }

    public override void Run()
    {
        Battle.SideOf(Subject).ChangeSkillPoint(1);
        Battle.Broadcast(o => o.UnleashedBasicAttack(Subject));
    }
}