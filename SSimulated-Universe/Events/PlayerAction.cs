using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Events;

public abstract class PlayerAction : Event
{
    protected readonly Player Subject;

    public PlayerAction(Player subject, Battle battle) : base(battle)
    {
        Subject = subject;
    }
}
