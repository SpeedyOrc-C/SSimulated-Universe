using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Events;

public abstract class PlayerAction<P> : Event where P : Player
{
    protected readonly P Subject;

    protected PlayerAction(P subject, Battle battle) : base(battle) => Subject = subject;
}

public abstract class BasicAttack<P> : PlayerAction<P> where P : Player
{
    protected BasicAttack(P subject, Battle battle) : base(subject, battle) { }

    public override void Run()
    {
        Battle.SideOf(Subject).ChangeSkillPoint(1);
        Battle.Broadcast(o => o.UnleashedBasicAttack(Subject));
    }
}

public abstract class Skill<P> : PlayerAction<P> where P : Player
{
    protected Skill(P subject, Battle battle) : base(subject, battle) { }

    public override void Run()
    {
        if (!Battle.SideOf(Subject).UseSkillPoint(1))
            throw new Exception("No enough skill point.");
        
        Battle.Broadcast(o => o.UnleashedSkill(Subject));
    }
}

public abstract class Ultimate<P> : PlayerAction<P> where P : Player
{
    protected Ultimate(P subject, Battle battle) : base(subject, battle) { }

    /// <summary>
    /// Discharge all energy, regenerate 5 energy, and broadcast a signal.
    /// </summary>
    public override void Run()
    {
        Subject.Discharge();
        Subject.RegenerateBoosted(5);
        Battle.Broadcast(o => o.UnleashedUltimate(Subject));
    }
}

public abstract class FollowUp<P> : PlayerAction<P> where P : Player
{
    protected FollowUp(P subject, Battle battle) : base(subject, battle) { }

    public override void Run()
    {
        Battle.Broadcast(o => o.UnleashedFollowUp(Subject));
    }
}
