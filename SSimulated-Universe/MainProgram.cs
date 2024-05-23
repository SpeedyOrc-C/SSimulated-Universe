using SSimulated_Universe.Entities.Players;
using SSimulated_Universe.Environment.WeaknessBreakEffects;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe;

class MainProgram
{
    public static void Main()
    {
        var battle = new Battle();
        
        var me = new TrailblazerDestruction(battle) {
            Level = 80,
        };
        
        var you = new TrailblazerDestruction(battle) {
            Level = 80,
        }; 
        
        battle.Left.Append(me);
        battle.Right.Append(you);

        me.MaxHp.Base = 4185;
        me.Attack.Base = 3255;
        if (me.Defence is not null)
            me.Defence.Base = 1174;
        
        you.MaxHp.Base = 4185;
        you.Attack.Base = 3255;
        if (you.Defence is not null)
            you.Defence.Base = 1174;

        new FarewellHit(me, you, battle).Run();
    }
}
