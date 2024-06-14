namespace SSimulated_Universe.Universe;

public enum PlayerActionType
{
    BasicAttack,
    Skill,
    Ultimate,
    FollowUp
}

public interface IPlayerAction : IEvent
{
    public new Player GetSubject { get; }
    public bool IsBasicAttack { get; }
    public bool IsSkill { get; }
    public bool IsUltimate { get; }
    public bool IsFollowUp { get; }
}

public abstract class PlayerAction<TPlayer>
    : Event<TPlayer>, IPlayerAction where TPlayer : Player
{
    public new Player GetSubject => Subject;

    public bool IsBasicAttack
    {
        get
        {
            var transformers = Subject.ActionTypeTransformers.Eval;

            return BaseIsBasicAttack
                || BaseIsSkill && transformers.Contains((PlayerActionType.Skill, PlayerActionType.BasicAttack))
                || BaseIsUltimate && transformers.Contains((PlayerActionType.Ultimate, PlayerActionType.BasicAttack))
                || BaseIsFollowUp && transformers.Contains((PlayerActionType.FollowUp, PlayerActionType.BasicAttack))
                ;
        }
    }

    public bool IsSkill
    {
        get
        {
            var transformers = Subject.ActionTypeTransformers.Eval;

            return BaseIsSkill
                || BaseIsBasicAttack && transformers.Contains((PlayerActionType.BasicAttack, PlayerActionType.Skill))
                || BaseIsUltimate && transformers.Contains((PlayerActionType.Ultimate, PlayerActionType.Skill))
                || BaseIsFollowUp && transformers.Contains((PlayerActionType.FollowUp, PlayerActionType.Skill))
                ;
        }
    }

    public bool IsUltimate
    {
        get
        {
            var transformers = Subject.ActionTypeTransformers.Eval;

            return BaseIsUltimate
                || BaseIsBasicAttack && transformers.Contains((PlayerActionType.BasicAttack, PlayerActionType.Ultimate))
                || BaseIsSkill && transformers.Contains((PlayerActionType.Skill, PlayerActionType.Ultimate))
                || BaseIsFollowUp && transformers.Contains((PlayerActionType.FollowUp, PlayerActionType.Ultimate))
                ;
        }
    }

    public bool IsFollowUp
    {
        get
        {
            var transformers = Subject.ActionTypeTransformers.Eval;

            return BaseIsFollowUp
                || BaseIsBasicAttack && transformers.Contains((PlayerActionType.BasicAttack, PlayerActionType.FollowUp))
                || BaseIsSkill && transformers.Contains((PlayerActionType.Skill, PlayerActionType.FollowUp))
                || BaseIsUltimate && transformers.Contains((PlayerActionType.Ultimate, PlayerActionType.FollowUp))
                ;
        }
    }

    protected virtual bool BaseIsBasicAttack => false;
    protected virtual bool BaseIsSkill => false;
    protected virtual bool BaseIsUltimate => false;
    protected virtual bool BaseIsFollowUp => false;

    public override void Execute()
    {
        Battle.Broadcast(o => o.BeforePlayerAction(this));
        base.Execute();
        Battle.Broadcast(o => o.AfterPlayerAction(this));
    }

    protected PlayerAction(TPlayer subject, Battle battle) : base(subject, battle) { }
}

public abstract class BasicAttack<TPlayer>
    : PlayerAction<TPlayer> where TPlayer : Player
{
    protected sealed override bool BaseIsBasicAttack => true;

    protected override void ExecuteInner()
    {
        Battle.SideOf(Subject).ChangeSkillPoint(1);
    }

    protected BasicAttack(TPlayer subject, Battle battle) : base(subject, battle) { }
}

public abstract class Skill<TPlayer>
    : PlayerAction<TPlayer> where TPlayer : Player
{
    protected sealed override bool BaseIsSkill => true;

    protected override void ExecuteInner()
    {
        if (!Battle.SideOf(Subject).UseSkillPoint(1))
            throw new Exception("No enough skill point.");
    }

    protected Skill(TPlayer subject, Battle battle) : base(subject, battle) { }
}

public abstract class Ultimate<TPlayer>
    : PlayerAction<TPlayer> where TPlayer : Player
{
    protected sealed override bool BaseIsUltimate => true;

    protected override void ExecuteInner()
    {
        if (Subject.Energy < Subject.MaxEnergy)
            throw new Exception("No enough energy.");

        Subject.Discharge();
        Subject.RegenerateBoosted(5);
    }

    protected Ultimate(TPlayer subject, Battle battle) : base(subject, battle) { }
}

public abstract class FollowUp<TPlayer>
    : PlayerAction<TPlayer> where TPlayer : Player
{
    protected sealed override bool BaseIsFollowUp => true;

    protected FollowUp(TPlayer subject, Battle battle) : base(subject, battle) { }
}