using System.Collections;

namespace SSimulated_Universe.Universe;

public class Battle
{
    public IEnumerable<Entity> Entities => _left.Entities.Concat(_right.Entities);
    
    public void Tick()
    {
        // TODO: Let entities unleash their ultimates here.

        if (_followUps.Count > 0)
        {
            _followUps.Pop().Execute();
            return;
        }

        if (_ultimates.Count > 0)
        {
            _ultimates.Dequeue().Execute();
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

    public void Add(HitSplitter hitSplitter)
    {
        _hitSplitters.Add(hitSplitter);
    }

    public void Remove(Entity entity)
    {
        if (_left.Remove(entity))
            Broadcast(o => o.EntityLeft(entity));
        else if (_right.Remove(entity))
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

    public void Remove(HitSplitter hitSplitter)
    {
        _hitSplitters.Remove(hitSplitter);
    }

    public void Broadcast(Action<BattleObserver> notify) => _notifier.Broadcast(notify);

    public Side SideOf(Entity entity)
    {
        if (_left.Contains(entity)) return _left;
        if (_right.Contains(entity)) return _right;
        throw new Exception("Cannot find the entity.");
    }

    public Side OppositeSideOf(Entity entity)
    {
        if (_left.Contains(entity)) return _right;
        if (_right.Contains(entity)) return _left;
        throw new Exception("Cannot find the entity.");
    }

    public void Send(Hit hit)
    {
        var splitHits =
            _hitSplitters
                .Aggregate(new List<Hit> { hit },
                    (listHit, splitter) => listHit.SelectMany(splitter.Split).ToList());

        foreach (var splitHit in splitHits)
            splitHit.Sender.TakeHit(splitHit);
    }

    public void Send(IEnumerable<Hit> hits)
    {
        foreach (var hit in hits)
            hit.Receiver.TakeHit(hit);
    }

    public void TriggerFollowUp(IEvent followUp) => _followUps.Push(followUp);
    public void UnleashUltimate(IEvent ultimate) => _ultimates.Enqueue(ultimate);
    public void AddExtraTurn(Entity entity) => _extraTurns.Enqueue(entity);

    private readonly Side _left;
    private readonly Side _right;
    private readonly HashSet<Effect> _effects;
    private readonly Notifier _notifier;
    private readonly Scheduler<Entity> _scheduler;
    private readonly Stack<IEvent> _followUps;
    private readonly Queue<IEvent> _ultimates;
    private readonly Queue<Entity> _extraTurns;
    private readonly SortedSet<HitSplitter> _hitSplitters;

    public Battle()
    {
        _left = new Side(this);
        _right = new Side(this);

        _effects = new HashSet<Effect>();
        _scheduler = new Scheduler<Entity>();
        _followUps = new Stack<IEvent>();
        _ultimates = new Queue<IEvent>();
        _extraTurns = new Queue<Entity>();
        _hitSplitters = new SortedSet<HitSplitter>(new HitSplitterPriorityComparer());

        _notifier = new Notifier(() =>
            Entities
                .Select(x => x as BattleObserver)
                .Concat(_effects.Select(x => x as BattleObserver))
        );
    }
}
