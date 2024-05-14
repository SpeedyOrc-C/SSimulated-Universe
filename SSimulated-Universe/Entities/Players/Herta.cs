using SSimulated_Universe.Events;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Entities.Players;

public class Herta : Player
{
    public override void YourTurn()
    {
        throw new NotImplementedException();
    }
    
    public Herta(Battle battle) : base(battle) { }
}