namespace SSimulated_Universe.Universe;

public class Battle
{
    public readonly Side Left;
    public readonly Side Right;

    public IEnumerable<Entity> Entities => Left.Entities.Concat(Right.Entities);
    
    public void Tick()
    {
        // TODO: Let entities unleash their ultimates here.

        if (_followUps.Count > 0)
        {
            _followUps.Pop().Run();
            return;
        }

        if (_ultimates.Count > 0)
        {
            _ultimates.Dequeue().Run();
            return;
        }

        var nextEntity = _extraTurns.Count > 0 ? _extraTurns.Dequeue() : _scheduler.Schedule(Entities);

        Broadcast(o => o.BeforeTurnOf(nextEntity));

        if (nextEntity.SkipNextTurn)
        {
            nextEntity.SkipNextTurn = false;
            return;
        }

        nextEntity.YourTurn();
    }

    public void Add(Effect effect)
    {
        _effects.Add(effect);

        effect.Added();

        Broadcast(o => o.EffectStarted(effect));
    }

    public void Remove(Entity entity)
    {
        if (Left.Remove(entity))
            Broadcast(o => o.EntityLeft(entity));
        else if (Right.Remove(entity))
            Broadcast(o => o.EntityLeft(entity));
        else
            throw new Exception("This entity doesn't exist.");
    }

    public void Remove(Effect effect)
    {
        if (!_effects.Remove(effect))
            throw new Exception("This effect doesn't exist.");

        effect.Removed();

        Broadcast(o => o.EffectEnded(effect));
    }

    public void Broadcast(Action<BattleObserver> notify) => _notifier.Broadcast(notify);

    public Side SideOf(Entity entity)
    {
        if (Left.Contains(entity)) return Left;
        if (Right.Contains(entity)) return Right;
        throw new Exception("Cannot find the entity.");
    }

    public Side OppositeSideOf(Entity entity)
    {
        if (Left.Contains(entity)) return Right;
        if (Right.Contains(entity)) return Left;
        throw new Exception("Cannot find the entity.");
    }

    public void TriggerFollowUp(Event followUp) => _followUps.Push(followUp);
    public void UnleashUltimate(Event ultimate) => _ultimates.Enqueue(ultimate);
    public void AddExtraTurn(Entity entity) => _extraTurns.Enqueue(entity);

    private readonly HashSet<Effect> _effects = new();
    private readonly Notifier _notifier;
    private readonly Scheduler<Entity> _scheduler = new();
    private readonly Stack<Event> _followUps = new();
    private readonly Queue<Event> _ultimates = new();
    private readonly Queue<Entity> _extraTurns = new();

    public Battle()
    {
        Left = new Side(this);
        Right = new Side(this);

        _notifier = new Notifier(() =>
            Entities.Select(x => x as BattleObserver).Concat(_effects.Select(x => x as BattleObserver))
        );
    }
}
