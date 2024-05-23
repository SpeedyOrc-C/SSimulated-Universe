using SSimulated_Universe.Entities;

namespace SSimulated_Universe.Universe;

public class Notifier
{
    private readonly Func<IEnumerable<BattleObserver>> GetObservers;

    public Notifier(Func<IEnumerable<BattleObserver>> getObservers)
    {
        GetObservers = getObservers;
    }

    public void Broadcast(Action<BattleObserver> notify)
    {
        foreach (var observer in GetObservers()) 
            notify(observer);
    }

    public void Broadcast(Entity excluded, Action<BattleObserver> notify)
    {
        foreach (var observer in GetObservers().Where(o => o != excluded)) 
            notify(observer);
    }
}
