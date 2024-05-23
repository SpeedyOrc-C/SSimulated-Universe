using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Events;

public abstract class PlayerAction<P> : Event where P : Player
{
    protected readonly P Subject;

    public PlayerAction(P subject, Battle battle) : base(battle)
    {
        Subject = subject;
    }
}
