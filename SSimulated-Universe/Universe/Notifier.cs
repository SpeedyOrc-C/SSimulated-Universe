namespace SSimulated_Universe.Universe;

public class Notifier
{
    private readonly HashSet<BattleObserver> _observers = new();

    public void Add(BattleObserver observer) => _observers.Add(observer);
    public bool Remove(BattleObserver observer) => _observers.Remove(observer);

    public void Broadcast(Action<BattleObserver> notify)
    {
        foreach (var observer in _observers) 
            notify(observer);
    }
}
