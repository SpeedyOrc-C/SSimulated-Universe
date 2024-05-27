namespace SSimulated_Universe.Universe;

public abstract class Event
{
    public abstract void Run();
    
    protected readonly Battle Battle;

    protected Event(Battle battle)
    {
        Battle = battle;
    }
}
