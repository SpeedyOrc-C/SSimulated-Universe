using SSimulated_Universe.Utility;

namespace SSimulated_Universe.Universe;

public enum RelicRarity { R2, R3, R4, R5 }
public class InvalidRelicRarity : Exception { }

public class SubStat
{
    public readonly Stat Stat;
    public readonly int LevelLow;
    public readonly int LevelMed;
    public readonly int LevelHigh;

    public int Level => LevelLow + LevelMed + LevelHigh;

    public SubStat(Stat stat, int levelLow, int levelMed, int levelHigh)
    {
        if (levelLow < 0 || levelMed < 0 || levelHigh < 0)
            throw new Exception("Sub-stat values cannot be negative.");

        if (levelLow == 0 && levelMed == 0 && levelHigh == 0)
            throw new Exception("Sub-stat values cannot be all zero.");

        Stat = stat;
        LevelLow = levelLow;
        LevelMed = levelMed;
        LevelHigh = levelHigh;
    }

    public SubStat(Stat stat, double value, RelicRarity rarity = RelicRarity.R5)
    {
        Stat = stat;

        var (valueLow, valueMed, valueHigh) = rarity switch
        {
            RelicRarity.R5 => stat switch
            {
                Stat.Speed =>
                    (2, 2.3, 2.6),
                Stat.HpI =>
                    (33.87, 38.103755, 42.33751),
                Stat.AttackI or Stat.DefenceI =>
                    (16.935, 19.051877, 21.168745),
                Stat.HpP or Stat.AttackP =>
                    (0.03456, 0.03888, 0.0432),
                Stat.DefenceP =>
                    (0.0432, 0.0486, 0.054),
                Stat.BreakEffect =>
                    (0.05184, 0.05832, 0.0648),
                Stat.EffectHitRate or Stat.EffectResistance =>
                    (0.03456, 0.03888, 0.0432),
                Stat.CriticalRate =>
                    (0.02592, 0.02916, 0.0324),
                Stat.CriticalDamage =>
                    (0.05184, 0.05832, 0.0648),
                _ => throw new Exception("Invalid sub-stat type.")
            },
            RelicRarity.R4 => stat switch
            {
                Stat.Speed =>
                    (1.6, 1.8, 2),
                Stat.HpI =>
                    (27.096, 30.483, 33.87),
                Stat.AttackI or Stat.DefenceI =>
                    (13.548, 15.2415, 16.935),
                Stat.HpP or Stat.AttackP =>
                    (0.027648, 0.031104, 0.03456),
                Stat.DefenceP =>
                    (0.03456, 0.03888, 0.0432),
                Stat.BreakEffect =>
                    (0.041472, 0.046656, 0.05184),
                Stat.EffectHitRate or Stat.EffectResistance =>
                    (0.027648, 0.031104, 0.03456),
                Stat.CriticalRate =>
                    (0.020736, 0.023328, 0.02592),
                Stat.CriticalDamage =>
                    (0.041472, 0.046656, 0.05184),
                _ => throw new Exception("Invalid sub-stat type.")
            },
            RelicRarity.R3 => stat switch
            {
                Stat.Speed =>
                    (1.2, 1.3, 1.4),
                Stat.HpI =>
                    (20.322, 22.862253, 25.402506),
                Stat.AttackI or Stat.DefenceI =>
                    (10.161, 11.431126, 12.701252),
                Stat.HpP or Stat.AttackP =>
                    (0.020736, 0.023328, 0.02592),
                Stat.DefenceP =>
                    (0.02592, 0.02916, 0.0324),
                Stat.BreakEffect =>
                    (0.031104, 0.034942, 0.03888),
                Stat.EffectHitRate or Stat.EffectResistance =>
                    (0.020736, 0.023328, 0.02592),
                Stat.CriticalRate =>
                    (0.015552, 0.017496, 0.01944),
                Stat.CriticalDamage =>
                    (0.031104, 0.034992, 0.03888),
                _ => throw new Exception("Invalid sub-stat type.")
            },
            RelicRarity.R2 => stat switch
            {
                Stat.Speed =>
                    (1, 1.1, 1.2),
                Stat.HpI =>
                    (13.548, 15.2415, 16.935),
                Stat.AttackI or Stat.DefenceI =>
                    (16.774, 7.62075, 8.4675),
                Stat.HpP or Stat.AttackP =>
                    (0.013824, 0.015552, 0.01728),
                Stat.DefenceP =>
                    (0.01728, 0.01944, 0.0216),
                Stat.BreakEffect =>
                    (0.020736, 0.023328, 0.02592),
                Stat.EffectHitRate or Stat.EffectResistance =>
                    (0.013824, 0.015552, 0.01728),
                Stat.CriticalRate =>
                    (0.010368, 0.011664, 0.01296),
                Stat.CriticalDamage =>
                    (0.020736, 0.023328, 0.02592),
                _ => throw new Exception("Invalid sub-stat type.")
            },
            _ => throw new InvalidRelicRarity()
        };

        var tolerance = stat switch
        {
            Stat.Speed or Stat.HpI or Stat.AttackI or Stat.DefenceP => 0.5,
            _ => 0.005
        };

        for (var low  = 0; low  <= 5            ; low += 1)
        for (var med  = 0; med  <= 5 - low      ; med += 1)
        for (var high = 0; high <= 5 - low - med; high += 1)
        {
            var level = low + med + high;

            if (level is < 1 or > 5) continue;

            var guessed = (valueLow * low) + (valueMed * med) + (valueHigh * high);

            if (Math.Abs(guessed - value) > tolerance) continue;

            LevelLow = low;
            LevelMed = med;
            LevelHigh = high;

            return;
        }

        throw new Exception("Invalid sub-stat value, this could be a relic from a private server.");
    }
}

public enum Stat
{
    // Suffixes: -I for Immediate value, -P for Proportional value.
    HpI, HpP,
    AttackI, AttackP,
    DefenceI, DefenceP,
    EffectHitRate,
    EffectResistance,
    OutgoingHealingBoost,
    CriticalRate,
    CriticalDamage,
    Speed,
    BreakEffect,
    EnergyRegenerationRate,
    DamageBoostPhysical,
    DamageBoostFire,
    DamageBoostIce,
    DamageBoostWind,
    DamageBoostLightning,
    DamageBoostQuantum,
    DamageBoostImaginary,
}

public abstract class Relic
{
    public readonly Stat MainStat;
    public readonly int MainLevel;
    public readonly HashSet<SubStat> SubStats;
    public readonly RelicRarity Rarity;

    public Relic(Stat mainStat, int mainLevel, HashSet<SubStat> subStats, RelicRarity rarity)
    {
        if (mainLevel < 1)
            throw new Exception("Relic level cannot be lower than 1.");

        switch (rarity)
        {
            case RelicRarity.R5 when mainLevel > 15:
                throw new Exception("R5 relic cannot have level higher than 15.");
            case RelicRarity.R4 when mainLevel > 12:
                throw new Exception("R4 relic cannot have level higher than 12.");
            case RelicRarity.R3 when mainLevel > 9:
                throw new Exception("R3 relic cannot have level higher than 9.");
            case RelicRarity.R2 when mainLevel > 6:
                throw new Exception("R2 relic cannot have level higher than 6.");
        }

        if (subStats.Count is < 1 or > 4)
            throw new Exception("A relic can only have 1 to 4 sub-stats.");

        if (subStats.Select(x => x.Stat).Count() < subStats.Count)
            throw new Exception("A relic cannot have duplicated sub-stats.");

        switch (rarity)
        {
            case RelicRarity.R5 when subStats.Count < Math.Min(4, 3 + mainLevel / 3):
                throw new Exception("Invalid number of sub-stats for a R5 relic.");
            case RelicRarity.R4 when subStats.Count < Math.Min(4, 2 + mainLevel / 3):
                throw new Exception("Invalid number of sub-stats for a R4 relic.");
            case RelicRarity.R3 when subStats.Count < Math.Min(4, 1 + mainLevel / 3):
                throw new Exception("Invalid number of sub-stats for a R3 relic.");
            case RelicRarity.R2 when subStats.Count < Math.Min(4, mainLevel / 3):
                throw new Exception("Invalid number of sub-stats for a R2 relic.");
        }

        // TODO: More sanity check here...

        MainStat = mainStat;
        MainLevel = mainLevel;
        SubStats = subStats;
        Rarity = rarity;
    }

    public int MaxLevel => Rarity switch
    {
        RelicRarity.R2 => 6,
        RelicRarity.R3 => 9,
        RelicRarity.R4 => 12,
        RelicRarity.R5 => 15,
        _ => throw new InvalidRelicRarity()
    };

    public double LevelMap(
        double min5, double max5,
        double min4, double max4,
        double min3, double max3,
        double min2, double max2
    ) => Rarity switch
        {
            RelicRarity.R5 => SMath.LinearMap(MainLevel, 1, MaxLevel, min5, max5),
            RelicRarity.R4 => SMath.LinearMap(MainLevel, 1, MaxLevel, min4, max4),
            RelicRarity.R3 => SMath.LinearMap(MainLevel, 1, MaxLevel, min3, max3),
            RelicRarity.R2 => SMath.LinearMap(MainLevel, 1, MaxLevel, min2, max2),
            _ => throw new InvalidRelicRarity()
        };

    public double Speed => LevelMap(4.032,25.032,3.2256,16.4256,2.4192,11.4192,1.6128,7.6128);
    public double Hp => LevelMap(112.896,705.6,90.3168,469.6474,67.7376,281.111,45.1584,139.991);
    public double Attack => LevelMap(56.448,352.8,45.1584,234.8237,33.8688,140.5555,22.5792,69.9955);
    public double HpP => LevelMap(0.069120,0.432,0.055296,0.287544,0.041472,0.172107,0.027648,0.085710);
    public double AttackP => LevelMap(0.069120,0.432,0.055296,0.287544,0.041472,0.172107,0.027648,0.085710);
    public double DefenceP => LevelMap(0.0864,0.540,0.06912,0.359424,0.05184,0.215136,0.03456,0.107136);
    public double BreakEffect => LevelMap(0.103680,0.648,0.082944,0.431304,0.062208,0.258165,0.041472,0.128562);
    public double EffectHitRate => LevelMap(0.069120,0.432,0.055296,0.287544,0.041472,0.172107,0.027648,0.085710);
    public double EnergyRegenerationRate => LevelMap(0.031104,0.194394,0.024883,0.129391,0.018662,0.07745,0.012442,0.038572);
    public double OutgoingHealingBoost => LevelMap(0.055296,0.345606,0.044237,0.230033,0.033178,0.137686,0.022118,0.068564);
    public double DamageBoostPhysical => LevelMap(0.062208,0.388803,0.049766,0.258782,0.037325,0.154901,0.024883,0.077137);
    public double DamageBoostFire => LevelMap(0.062208,0.388803,0.049766,0.258782,0.037325,0.154901,0.024883,0.077137);
    public double DamageBoostIce => LevelMap(0.062208,0.388803,0.049766,0.258782,0.037325,0.154901,0.024883,0.077137);
    public double DamageBoostWind => LevelMap(0.062208,0.388803,0.049766,0.258782,0.037325,0.154901,0.024883,0.077137);
    public double DamageBoostLightning => LevelMap(0.062208,0.388803,0.049766,0.258782,0.037325,0.154901,0.024883,0.077137);
    public double DamageBoostQuantum => LevelMap(0.062208,0.388803,0.049766,0.258782,0.037325,0.154901,0.024883,0.077137);
    public double DamageBoostImaginary => LevelMap(0.062208,0.388803,0.049766,0.258782,0.037325,0.154901,0.024883,0.077137);
    public double CriticalRate => LevelMap(0.05184,0.324,0.041472,0.215652,0.031104,0.129078,0.020736,0.064284);
    public double CriticalDamage => LevelMap(0.10368,0.648,0.082944,0.431304,0.062208,0.258165,0.041472,0.128562);
}

public class Head : Relic
{
    public Head(Stat mainStat, int mainLevel, HashSet<SubStat> subStats, RelicRarity rarity)
        : base(mainStat, mainLevel, subStats, rarity)
    {
        if (mainStat is not Stat.HpI) throw new Exception("Head's main stat must be HP.");
    }
}

public class Hands : Relic
{
    public Hands(Stat mainStat, int mainLevel, HashSet<SubStat> subStats, RelicRarity rarity)
        : base(mainStat, mainLevel, subStats, rarity)
    {
        if (mainStat is not Stat.AttackI) throw new Exception("Hands' main stat must be ATK%.");
    }
}

public class Body : Relic
{
    public Body(Stat mainStat, int mainLevel, HashSet<SubStat> subStats, RelicRarity rarity)
        : base(mainStat, mainLevel, subStats, rarity)
    {
        var ok = mainStat
            is Stat.HpP
            or Stat.AttackP
            or Stat.DefenceP
            or Stat.EffectHitRate
            or Stat.OutgoingHealingBoost
            or Stat.CriticalRate
            or Stat.CriticalDamage;

        if (!ok) throw new Exception("Body's main stat must be one of " +
                                      "HP%, ATK%, DEF%, Effect Hit Rate, Outgoing Healing Boost, CRIT Rate, CRIT DMG.");
    }
}

public class Feet : Relic
{
    public Feet(Stat mainStat, int mainLevel, HashSet<SubStat> subStats, RelicRarity rarity)
        : base(mainStat, mainLevel, subStats, rarity)
    {
        var ok = mainStat
            is Stat.HpP
            or Stat.AttackP
            or Stat.DefenceP
            or Stat.Speed;

        if (!ok) throw new Exception("Feet's main stat must be one of " +
                                      "HP%, ATK%, DEF%, Speed.");
    }
}

public class PlanarSphere : Relic
{
    public PlanarSphere(Stat mainStat, int mainLevel, HashSet<SubStat> subStats, RelicRarity rarity)
        : base(mainStat, mainLevel, subStats, rarity)
    {
        var ok = mainStat
            is Stat.HpP
            or Stat.AttackP
            or Stat.DefenceP
            or Stat.DamageBoostPhysical
            or Stat.DamageBoostFire
            or Stat.DamageBoostIce
            or Stat.DamageBoostWind
            or Stat.DamageBoostLightning
            or Stat.DamageBoostQuantum
            or Stat.DamageBoostImaginary;

        if (!ok) throw new Exception("Planar Sphere's main stat must be one of " +
                                      "HP%, ATK%, DEF%, Damage Boost.");
    }
}

public class LinkRope : Relic
{
    public LinkRope(Stat mainStat, int mainLevel, HashSet<SubStat> subStats, RelicRarity rarity)
        : base(mainStat, mainLevel, subStats, rarity)
    {
        var ok = mainStat
            is Stat.HpP
            or Stat.AttackP
            or Stat.DefenceP
            or Stat.BreakEffect
            or Stat.EnergyRegenerationRate;

        if (!ok) throw new Exception("Link Rope's main stat must be one of " +
                                      "HP%, ATK%, DEF%, Break Effect, Energy Regeneration Rate.");
    }
}
