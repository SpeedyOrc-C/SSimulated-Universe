namespace SSimulated_Universe.Universe;

public enum LightConeRarity { R3, R4, R5, R5Herta }
public class InvalidLightConeRarity : Exception { }

/*
以世界之名
LVL HP  ATK DEF
1   48  26  21
4   69  38  30
6   84  46  36
*/

public class LightCone
{
    public readonly double Hp;
    public readonly double Attack;
    public readonly double Defence;
    public readonly LightConeRarity Rarity;

    public LightCone(double hp, double attack, double defence, LightConeRarity rarity)
    {
        var cap = rarity switch
        {
            LightConeRarity.R3 => 18,
            LightConeRarity.R4 => 23,
            LightConeRarity.R5 => 28,
            LightConeRarity.R5Herta => 26,
            _ => throw new InvalidLightConeRarity()
        };

        var value = hp / 4.8 + attack / 2.4 + defence / 3;

        Hp = hp;
        Attack = attack;
        Defence = defence;
        Rarity = rarity;
    }
}