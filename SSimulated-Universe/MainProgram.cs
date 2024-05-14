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
            // LevelBasicAttack = 7,
            //
            // MaxHp = new(4185),
            // Attack = new (3255),
            // Defence = new (1174),
        };
    }
}
