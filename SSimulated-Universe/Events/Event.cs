using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Events;

public abstract class Event
{
    public abstract void Run();
    
    protected readonly Battle Battle;

    protected Event(Battle battle)
    {
        Battle = battle;
    }
}
