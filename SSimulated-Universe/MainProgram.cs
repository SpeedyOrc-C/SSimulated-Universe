using SSimulated_Universe.Universe;

namespace SSimulated_Universe;

internal class MainProgram
{
    public static void Main()
    {
        var r = new Feet(
            Stat.AttackP,
            15,
            new HashSet<SubStat>
            {
                new(Stat.Speed, 2),
                new(Stat.CriticalRate, 0.071),
                new(Stat.CriticalDamage, 0.181),
                new(Stat.EffectHitRate, 0.082),
            },
            RelicRarity.R5
        );

        return;
    }
}
