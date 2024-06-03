using SSimulated_Universe.Utility;
using SSimulated_Universe.Utility.Modifiable.Number;

namespace SSimulated_Universe.Universe;

public abstract class Player : Entity
{
    // public override double MaxToughnessMultiplier => 1;
    // public override double RealDefence => Defence.Eval;
    // public override double BrokenMultiplier => 1;

    public readonly int LevelBasicAttack;
    public readonly int LevelTalent;
    public readonly int LevelUltimate;
    public readonly int LevelSkill;

    public abstract int EidolonBasicAttackAdd1 { get; }
    public abstract int EidolonSkillAdd2 { get; }
    public abstract int EidolonUltimateAdd2 { get; }
    public abstract int EidolonTalentAdd2 { get; }
    
    public readonly int Eidolon;

    public bool Eidolon1 => Eidolon >= 1;
    public bool Eidolon2 => Eidolon >= 2;
    public bool Eidolon3 => Eidolon >= 3;
    public bool Eidolon4 => Eidolon >= 4;
    public bool Eidolon5 => Eidolon >= 5;
    public bool Eidolon6 => Eidolon == 6;
    
    public double LevelBasicAttackMap(double minValue, double maxValue)
        => SMath.LinearMap(LevelBasicAttack, 1, 7, minValue, maxValue);
    
    public double LevelSkillMap(double minValue, double maxValue)
        => SMath.LinearMap(LevelSkill, 1, 12, minValue, maxValue);
    
    public double LevelUltimateMap(double minValue, double maxValue)
        => SMath.LinearMap(LevelUltimate, 1, 12, minValue, maxValue);
    
    public double LevelTalentMap(double minValue, double maxValue)
        => SMath.LinearMap(LevelTalent, 1, 12, minValue, maxValue);

    public override void HpZeroed() { }

    public override void TakeHit(Hit hit)
    {
        Battle.Broadcast(o => o.BeforeTakeHit(hit.Attacker, hit, this));

        LastDamageSource = new DamageFromEntity(hit.Attacker, hit.ActionType);
        TakeDamage(hit.Damage);

        Battle.Broadcast(o => o.AfterTakeHit(hit.Attacker, hit, this));
    }

    public Player(
        string name,
        int level,
        double maxHp,
        double attack,
        double defence,
        double speed,
        double energyRegenerationRate,
        double breakEffect,
        double effectHitRate,
        double effectResistance,
        double criticalRate,
        double criticalDamage,
        double maxEnergy,
        TypesModifiableDoubles vulnerability,
        TypesModifiableDoubles resistance,
        TypesModifiableDoubles resistancePenetration,
        TypesModifiableDoubles boost,
        Battle battle)

        : base(
            name,
            level,
            isEliteOrBoss: false,
            maxHp,
            attack,
            defence,
            speed,
            energyRegenerationRate,
            breakEffect,
            effectHitRate,
            effectResistance,
            criticalRate,
            criticalDamage,
            maxEnergy,
            maxToughness: 0,
            toughnessGaugeSize: 0,
            weaknesses: new HashSet<DamageType>(),
            vulnerability,
            resistance,
            resistancePenetration,
            boost,
            battle)
    {
    }
}
