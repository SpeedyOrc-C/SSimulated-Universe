using SSimulated_Universe.Entities;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Events;

/*
How to calculate the damage?

DMG = Attacker Base DMG
    * Attacker CRIT Multiplier
    * Attacker DMG Boost Multiplier
    * Attacker Weaken Multiplier
    * Target DEF Multiplier
    * Target RES Multiplier
    * Target Vulnerability Multiplier
    * Target DMG Mitigation Multiplier
    * Target Broken Multiplier

where

Base DMG
    = Ability Multiplier * Ability's Value
    + Extra DMG

CRIT Multiplier
    | Scores a CRIT hit = 1 + CRIT DMG
    | Otherwise         = 1

DMG Boost Multiplier
    = 1
    + Elemental DMG Boost
    + Sum of All DMG Boost
    + DoT Boost

Weaken Multiplier = 1 - Weaken

DEF Multiplier = 1 - DEF / (DEF + Attacker's Level * 10 + 200)
Enemy's DEF = 200 + 10 * Level

RES Multiplier = 1 - Target RES + Attacker RES PEN

DMG Mitigation Multiplier = Product of (1 - ith DMG Mitigation)

Broken Multiplier
    | Has Toughness = 0.9
    | Toughness is Broken = 1
*/

public enum DamageType
{
    Physical,
    Fire,
    Ice,
    Lightning,
    Wind,
    Quantum,
    Imaginary
}

public class BaseDamage
{
    public DamageType DamageType;
    public double Base = 0;
    public double BaseHp = 0;
    public double BaseAttack = 0;
    public double BaseDefence = 0;
    public double WeaknessBreak = 0;

    public double By(Entity attacker) =>
          Base
        + BaseHp * attacker.MaxHp.Eval
        + BaseAttack * attacker.Attack.Eval
        + BaseDefence * attacker.RealDefence
    ;
}

public record Hit(
    Entity Attacker,
    Entity Target,
    double Damage,
    DamageType DamageType,
    double ToughnessDepletion
);

public class Attack
{
    /// <summary>
    /// A single attack may consist of multiple hits.
    /// Each hit's damage is a proportion of the total damage.
    /// And they have independent chances to be a critical damage.
    /// <br/>
    /// This method has <b>side effect</b> since random critical damage is calculated here.
    /// </summary>
    /// <returns>Split damages</returns>
    public static IEnumerable<Hit> SingleTarget(
        Entity attacker,
        Entity target,
        List<double> hitSplit,
        BaseDamage baseDamage) 
    {
        // TODO: DMG Mitigation, Weaken

        var normalizedHitSplit = hitSplit.Select(x => x / hitSplit.Count).ToList();

        var damage
            // Attacker
            = baseDamage.By(attacker)
            * attacker.DamageBoostMultiplier(baseDamage.DamageType)
            // Attacker & Target
            * attacker.ResistanceMultiplier(target, baseDamage.DamageType)
            * attacker.DefenceMultiplier(target)
            // Target
            * target.VulnerabilityMultiplier(baseDamage.DamageType)
            * target.BrokenMultiplier
            ;

        var weaknessBreak = 
            baseDamage.WeaknessBreak * attacker.WeaknessBreakEfficiency.Eval;
        
        var repeatedDamages = Enumerable
            .Repeat(0, normalizedHitSplit.Count)
            .Select(_ =>
            {
                var isCriticalAttack =
                    1 - Random.Shared.NextDouble() <= attacker.CriticalRate.Eval;
                var multiplierCritical =
                    1 + (isCriticalAttack ? attacker.CriticalDamage.Eval : 0);

                return damage * multiplierCritical;
            });
        
        var splitDamages =
            normalizedHitSplit
                .Zip(repeatedDamages)
                .Select(p => 
                    new Hit(
                        Attacker: attacker,
                        Target : target,
                        Damage: p.First * p.Second,
                        DamageType: baseDamage.DamageType,
                        ToughnessDepletion: p.First * weaknessBreak
                    )
                );
        
        return splitDamages;
    }
    
    public static IEnumerable<Hit> Blast(
        Entity attacker,
        Entity target,
        Battle battle,
        List<double> hitSplit,
        BaseDamage baseDamage,
        List<double> hitSplitAdjacent,
        BaseDamage baseDamageAdjacent)
    {
        var targetSide = battle.SideOf(target);
        var targetLeft = targetSide.FindLeft(target);
        var targetRight = targetSide.FindRight(target);
        
        var hits = 
            SingleTarget(attacker, target, hitSplit, baseDamage);
        
        var hitsLeft =
            targetLeft is not null
                ? SingleTarget(attacker, targetLeft, hitSplitAdjacent, baseDamageAdjacent) 
                : Enumerable.Empty<Hit>();

        var hitsRight =
            targetRight is not null
                ? SingleTarget(attacker, targetRight, hitSplitAdjacent, baseDamageAdjacent)
                : Enumerable.Empty<Hit>();
        
        return hits.Concat(hitsLeft).Concat(hitsRight);
    }

    public static IEnumerable<Hit> Bounce(
        Entity attacker,
        Entity targetFirst,
        Battle battle,
        int bounces,
        List<double> hitSplit,
        BaseDamage baseDamage)
    {
        if (bounces <= 1)
            throw new Exception("There must be at least 2 bounces.");

        var targetSide = battle.SideOf(targetFirst);

        var hitsHead = Enumerable
            .Repeat(SingleTarget(attacker, targetFirst, hitSplit, baseDamage), 1);

        var hitsTail = Enumerable
            .Repeat<>(null, bounces - 1)
            .Select(_ => SingleTarget(attacker, targetSide.PickRandomEntity(), hitSplit, baseDamage));

        return hitsHead.Concat(hitsTail).SelectMany(x => x);
    }
    
    public static IEnumerable<Hit> AoE(
        Entity attacker,
        Side targetSide,
        List<double> hitSplit,
        BaseDamage baseDamage) 
    =>
        targetSide.Entities
            .Select(target => SingleTarget(attacker, target, hitSplit, baseDamage))
            .SelectMany(x => x);
}
