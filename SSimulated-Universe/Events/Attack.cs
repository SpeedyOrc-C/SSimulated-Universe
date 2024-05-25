// type TargetsHits = [(Entity, [Hit])]
global using TargetsHits = 
    System.Collections.Generic.IEnumerable
        <System.Collections.Generic.KeyValuePair
            < SSimulated_Universe.Entities.Entity
            , System.Collections.Generic.IEnumerable<SSimulated_Universe.Events.Hit>>>;

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

public record BaseDamage
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
    double Damage,
    DamageType DamageType,
    double ToughnessDepletion,
    DamageMethod DamageMethod
);

public static class Attack
{
    public static readonly IReadOnlyList<double> One = new List<double> { 1 };
    
    /// <summary>
    /// A single attack may consist of multiple hits.
    /// Each hit's damage is a proportion of the total damage.
    /// And they have independent chances to be a critical damage.
    /// <br/>
    /// This method has <b>side effect</b> since random critical damage is calculated here.
    /// </summary>
    /// <returns>Split damages</returns>
    public static TargetsHits SingleTarget(
        Entity attacker,
        Entity target,
        DamageMethod damageMethod,
        IReadOnlyList<double> hitSplit,
        BaseDamage baseDamage)
    {
        // TODO: DMG Mitigation, Weaken
        
        var normalizedHitSplit = hitSplit.Select(x => x / hitSplit.Count);
        
        var repeatedDamages = Enumerable
            .Repeat(0, hitSplit.Count)
            .Select(_ =>
            {
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
                {
                    var (ratio, damage) = p;

                    var weaknessBreak = 
                        baseDamage.WeaknessBreak * attacker.WeaknessBreakEfficiency.Eval;
                    
                    return new Hit(
                        Attacker: attacker,
                        Damage: ratio * damage,
                        DamageType: baseDamage.DamageType,
                        ToughnessDepletion: ratio * weaknessBreak, 
                        DamageMethod: damageMethod);
                });
        
        return new Dictionary<Entity, IEnumerable<Hit>> { {target, splitDamages} };
    }
    
    public static TargetsHits Blast(
        Entity attacker,
        Entity target,
        Battle battle,
        DamageMethod damageMethod,
        IReadOnlyList<double> hitSplit,
        BaseDamage baseDamage,
        IReadOnlyList<double> hitSplitAdjacent,
        BaseDamage baseDamageAdjacent)
    {
        var targetSide = battle.SideOf(target);

        var targetLeft = targetSide.FindLeft(target);
        var targetRight = targetSide.FindRight(target);
        
        var hits = 
            SingleTarget(attacker, target, damageMethod, hitSplit, baseDamage);

        var hitsLeft =
            targetLeft is not null
                ? SingleTarget(attacker, targetLeft, damageMethod, hitSplitAdjacent, baseDamageAdjacent)
                : new Dictionary<Entity, IEnumerable<Hit>>();

        var hitsRight =
            targetRight is not null
                ? SingleTarget(attacker, targetRight, damageMethod, hitSplitAdjacent, baseDamageAdjacent)
                : new Dictionary<Entity, IEnumerable<Hit>>();
        
        return hits.Union(hitsLeft).Union(hitsRight);
    }

    public static TargetsHits Bounce(
        Entity attacker,
        Entity targetFirst,
        Battle battle,
        DamageMethod damageMethod,
        int bounces,
        List<double> hitSplit,
        BaseDamage baseDamage)
    {
        if (bounces == 1)
            throw new Exception("For a single bounce, use the single target one instead.");

        var targetSide = battle.SideOf(targetFirst);
        
        var hitsHead = SingleTarget(attacker, targetFirst, damageMethod, hitSplit, baseDamage);

        TargetsHits hitsTail = new Dictionary<Entity, IEnumerable<Hit>>();
        for (var i = 1; i <= bounces - 1; i += 1)
        {
            var hits = SingleTarget(attacker, targetSide.PickRandomEntity(), damageMethod, hitSplit, baseDamage);
            hitsTail = hitsTail.Union(hits);
        }

        return hitsHead.Union(hitsTail);
    }
    
    public static TargetsHits AoE(
        Entity attacker,
        Side targetSide,
        DamageMethod damageMethod,
        List<double> hitSplit,
        BaseDamage baseDamage) 
    =>
        targetSide.Entities
            .Select(target => SingleTarget(attacker, target, damageMethod, hitSplit, baseDamage))
            .SelectMany(x => x);
}
