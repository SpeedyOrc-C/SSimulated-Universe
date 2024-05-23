using SSimulated_Universe.Entities;
using SSimulated_Universe.Environment;
using SSimulated_Universe.Events;

namespace SSimulated_Universe.Universe;

public class Battle
{
    public readonly Side Left;
    public readonly Side Right;
    
    private readonly HashSet<Effect> _effects = new();
    
    private readonly Notifier _notifier;
    private readonly Scheduler<Entity> _scheduler = new();
    
    private readonly Stack<Event> _followUps = new();
    private readonly Queue<Event> _ultimates = new();
    private readonly Queue<Entity> _extraTurns = new();

    public Battle()
    {
        _notifier = new Notifier(GetObservers);
        Left = new Side(_notifier);
        Right = new Side(_notifier);
    }

    public IEnumerable<Entity> Entities => Left.Entities.Concat(Right.Entities);

    public IEnumerable<BattleObserver> GetObservers() => 
        Entities.Select(x => x as BattleObserver)
        .Concat(_effects.Select(x => x as BattleObserver));

    public void Add(Effect effect)
    {
        _effects.Add(effect);
        Broadcast(o => o.EffectStarted(effect));
    }

    public void Remove(Entity entity)
    {
        if (Left.Remove(entity))
        {
            Broadcast(o => o.EntityLeft(entity));
            return;
        }

        if (Right.Remove(entity))
        {
            Broadcast(o => o.EntityLeft(entity));
            return;
        }

        throw new Exception("This entity doesn't exist.");
    }

    public void Remove(Effect effect)
    {
        if (!_effects.Remove(effect))
            throw new Exception("This effect doesn't exist.");

        effect.CleanUp();
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
}
