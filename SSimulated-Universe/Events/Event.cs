using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Events;

public abstract class Event
{
    protected Battle Battle;

    protected Event(Battle battle)
    {
        Battle = battle;
    }

    public abstract void Run();
}
