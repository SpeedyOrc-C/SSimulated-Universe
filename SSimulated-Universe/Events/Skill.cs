using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Events;

public class Skill : PlayerAction
{
    public Skill(Player subject, Battle battle) : base(subject, battle) { }

    public override void Run()
    {
        if (!Battle.SideOf(Subject).UseSkillPoint(1))
            throw new Exception("No enough skill point.");
        Battle.Broadcast(o => o.UnleashedSkill(Subject));
    }
}