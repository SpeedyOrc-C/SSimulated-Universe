namespace SSimulated_Universe.Universe;

public abstract class PlayerAction<P> : Event where P : Player
{
    protected readonly P Subject;

    public abstract void Action();

    protected PlayerAction(P subject, Battle battle) : base(battle) => Subject = subject;
}

public abstract class BasicAttack<P> : PlayerAction<P> where P : Player
{
    protected BasicAttack(P subject, Battle battle) : base(subject, battle) { }

    public override void Run()
    {
        Battle.SideOf(Subject).ChangeSkillPoint(1);
        
        Battle.Broadcast(o => o.BeforeAction(Subject, ActionType.BasicAttack));
        Action();
        Battle.Broadcast(o => o.AfterAction(Subject, ActionType.BasicAttack));
    }
}

public abstract class Skill<P> : PlayerAction<P> where P : Player
{
    protected Skill(P subject, Battle battle) : base(subject, battle) { }

    public override void Run()
    {
        if (!Battle.SideOf(Subject).UseSkillPoint(1))
            throw new Exception("No enough skill point.");

        Battle.Broadcast(o => o.BeforeAction(Subject, ActionType.Skill));
        Action();
        Battle.Broadcast(o => o.AfterAction(Subject, ActionType.Skill));
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
        
        Battle.Broadcast(o => o.BeforeAction(Subject, ActionType.Ultimate));
        Action();
        Battle.Broadcast(o => o.AfterAction(Subject, ActionType.Ultimate));
    }
}

public abstract class FollowUp<P> : PlayerAction<P> where P : Player
{
    protected FollowUp(P subject, Battle battle) : base(subject, battle) { }

    public override void Run()
    {
        Battle.Broadcast(o => o.BeforeAction(Subject, ActionType.FollowUp));
        Action();
        Battle.Broadcast(o => o.AfterAction(Subject, ActionType.FollowUp));
    }
}
