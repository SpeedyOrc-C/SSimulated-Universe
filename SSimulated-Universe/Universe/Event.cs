namespace SSimulated_Universe.Universe;

public interface IEvent
{
    public Entity GetSubject { get; }
    public void Execute();
}

public abstract class Event<TEntity> : IEvent where TEntity : Entity
{
    public Entity GetSubject => Subject;

    public virtual void Execute()
    {
        Battle.Broadcast(o => o.BeforeEvent(this));
        ExecuteInner();
        Battle.Broadcast(o => o.AfterEvent(this));
    }

    protected abstract void ExecuteInner();

    protected readonly TEntity Subject;
    protected readonly Battle Battle;

    protected Event(TEntity subject, Battle battle)
    {
        Battle = battle;
        Subject = subject;
    }
}
