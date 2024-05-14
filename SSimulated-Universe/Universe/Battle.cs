using SSimulated_Universe.Entities;
using SSimulated_Universe.Environment;
using SSimulated_Universe.Events;

namespace SSimulated_Universe.Universe;

public class Battle
{
    private readonly Sides _sides;
    private readonly Notifier _notifier = new();
    private readonly Scheduler<Entity> _scheduler = new();
    private readonly HashSet<Effect> _effects = new();
    private readonly Stack<Event> _followUps = new();
    private readonly Queue<Event> _ultimates = new();
    private readonly Queue<Entity> _extraTurns = new();

    public Battle() => _sides = new Sides(_notifier);

    public void LeftPrepend(Entity entity)
    {
        _notifier.Add(entity);
        _sides.LeftPrepend(entity);
    }

    public void LeftAppend(Entity entity)
    {
        _notifier.Add(entity);
        _sides.LeftAppend(entity);
    }

    public void RightPrepend(Entity entity)
    {
        _notifier.Add(entity);
        _sides.RightPrepend(entity);
    }

    public void RightAppend(Entity entity)
    {
        _notifier.Add(entity);
        _sides.RightAppend(entity);
    }

    public void Add(Effect effect)
    {
        _notifier.Add(effect);
        _effects.Add(effect);
    }

    public void Remove(Effect effect)
    {
        if (!_effects.Remove(effect) || !_notifier.Remove(effect))
            throw new Exception("This effect doesn't exist.");

        effect.CleanUp();
        Broadcast(o => o.EffectEnded(effect));
    }

    public void Broadcast(Action<BattleObserver> notify) => _notifier.Broadcast(notify);
    public Side SideOf(Entity entity) => _sides.SideOf(entity);
    public Side OppositeSideOf(Entity entity) => _sides.OppositeSideOf(entity);

    public void TriggerFollowUp(Event followUp) => _followUps.Push(followUp);
    public void TriggerUltimate(Event ultimate) => _ultimates.Enqueue(ultimate);
    public void AddExtraTurn(Entity entity) => _extraTurns.Enqueue(entity);

    public void Tick()
    {
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

        var nextEntity = _extraTurns.Count > 0 ? _extraTurns.Dequeue() : _scheduler.Schedule(_sides.Entities);

        Broadcast(o => o.BeforeTurnOf(nextEntity));

        if (nextEntity.SkipNextTurn)
        {
            nextEntity.SkipNextTurn = false;
            return;
        }

        nextEntity.YourTurn();
    }
}
