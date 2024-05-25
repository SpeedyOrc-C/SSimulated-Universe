using SSimulated_Universe.Entities;
using SSimulated_Universe.Entities.Players;
using SSimulated_Universe.Events;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe;

internal class MainProgram
{
    public static void Main()
    {
        var battle = new Battle();
        
        var me = new TrailblazerDestruction(battle)
        {
            Level = 80,
            MaxHp = { Base = 4185 },
            Attack = { Base = 3255 },
            Defence = { Base = 1174 }
        };
        me.Reset();

        var you = new EnemyIdle(battle) { Level = 90, MaxToughness = 100};
        you.Weaknesses.Base.Add(DamageType.Physical);
        you.MaxHp.Base = 10000;
        you.Reset();

        var observer = new VerboseObserver(battle);
        
        battle.Left.Append(me);
        battle.Right.Append(you);
        battle.Add(observer);

        var farewellHit = new FarewellHit(me, you, battle);
        farewellHit.Run();
        farewellHit.Run();
        farewellHit.Run();
        farewellHit.Run();
        farewellHit.Run();
    }
}
