using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Events;

public abstract class Ultimate<P> : PlayerAction<P> where P : Player
{
    public Ultimate(P subject, Battle battle) : base(subject, battle) { }

    /// <summary>
    /// Discharge all energy, regenerate 5 energy, and broadcast a signal.
    /// </summary>
    public override void Run()
    {
        Subject.Discharge();
        Subject.RegenerateBoosted(5);
        Battle.Broadcast(o => o.UnleashedUltimate(Subject));
    }
}
