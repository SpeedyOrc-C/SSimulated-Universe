namespace SSimulated_Universe.Universe;

public class Notifier
{
    private readonly Func<IEnumerable<BattleObserver>> _getObservers;

    public Notifier(Func<IEnumerable<BattleObserver>> getObservers)
    {
        _getObservers = getObservers;
    }

    public void Broadcast(Action<BattleObserver> notify)
    {
        var observers = _getObservers().ToList();
        foreach (var observer in observers) 
            notify(observer);
    }
}
