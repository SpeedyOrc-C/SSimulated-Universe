// Hello Star Rail!

using SSimulated_Universe.Universe;
using SSimulated_Universe.Utility.Modifiable.Number;

namespace SSimulated_Universe.Players;

public class FarewellHit : BasicAttack<TrailblazerDestruction>
{
    private readonly Entity _target;

    public FarewellHit(TrailblazerDestruction subject, Entity target, Battle battle) : base(subject, battle)
    {
        _target = target;
    }

    protected override void ExecuteInner()
    {
        base.ExecuteInner();

        Subject.RegenerateBoosted(20);

        var targetsHits = Attack.SingleTarget(
            attacker: Subject,
            target: _target,
            hitSplit: Attack.One,
            baseDamage: new BaseDamage
            {
                WeaknessBreak = 30,
                DamageType = DamageType.Physical,
                BaseAttack = Subject.LevelBasicAttackMap(0.5, 1.1)
            }
        );

        Subject.Send(targetsHits);
    }
}

public class RipHomeRun : Skill<TrailblazerDestruction>
{
    private readonly Entity _target;
    private static readonly ModifierDoubleImmediate DamageBooster = new(0.25);

    public RipHomeRun(TrailblazerDestruction subject, Entity target, Battle battle) : base(subject, battle)
    {
        _target = target;
    }

    protected override void ExecuteInner()
    {
        base.ExecuteInner();

        Subject.RegenerateBoosted(30);

        var targetsHits = Attack.Blast(
            attacker: Subject,
            target: _target,
            battle: Battle,
            hitSplit: Attack.One,
            baseDamage: new BaseDamage
            {
                DamageType = DamageType.Physical,
                BaseAttack = Subject.LevelSkillMap(0.625, 1.375),
                WeaknessBreak = 60
            },
            hitSplitAdjacent: Attack.One,
            baseDamageAdjacent: new BaseDamage
            {
                DamageType = DamageType.Physical,
                BaseAttack = Subject.LevelSkillMap(0.625, 1.375),
                WeaknessBreak = 30
            }
        );

        if (Subject.FightingWill)
        {
            DamageBooster.Modify(Subject.Boost.Physical);
            {
                Subject.Send(targetsHits);
            }
            DamageBooster.Dismiss(Subject.Boost.Physical);
        }
        else
        {
            Subject.Send(targetsHits);
        }
    }
}

public class StardustAceBlowoutFarewellHit : Ultimate<TrailblazerDestruction>
{
    private readonly Entity _target;

    public StardustAceBlowoutFarewellHit(TrailblazerDestruction subject, Entity target, Battle battle) : base(subject,
        battle)
    {
        _target = target;
    }

    protected override void ExecuteInner()
    {
        base.ExecuteInner();

        var hits = Attack.SingleTarget(
            attacker: Subject,
            target: _target,
            hitSplit: Attack.One,
            baseDamage: new BaseDamage
            {
                DamageType = DamageType.Physical,
                BaseAttack = Subject.LevelUltimateMap(3, 4.8),
                WeaknessBreak = 90
            }
        );

        Subject.Send(hits);
    }
}

public class StardustAceBlowoutRipHomeRun : Ultimate<TrailblazerDestruction>
{
    private readonly Entity _target;
    private static readonly ModifierDoubleImmediate DamageBooster = new(0.25);

    public StardustAceBlowoutRipHomeRun(TrailblazerDestruction subject, Entity target, Battle battle) : base(subject,
        battle)
    {
        _target = target;
    }

    protected override void ExecuteInner()
    {
        base.ExecuteInner();

        var targetsHits = Attack.Blast(
            attacker: Subject,
            target: _target,
            battle: Battle,
            hitSplit: Attack.One,
            baseDamage: new BaseDamage
            {
                DamageType = DamageType.Physical,
                BaseAttack = Subject.LevelUltimateMap(1.80, 2.88),
                WeaknessBreak = 60
            },
            hitSplitAdjacent: Attack.One,
            baseDamageAdjacent: new BaseDamage
            {
                DamageType = DamageType.Physical,
                BaseAttack = Subject.LevelUltimateMap(1.08, 1.728),
                WeaknessBreak = 60
            }
        );

        if (Subject.FightingWill)
        {
            DamageBooster.Modify(Subject.Boost.Physical);
            {
                Subject.Send(targetsHits);
            }
            DamageBooster.Dismiss(Subject.Boost.Physical);
        }
        else
        {
            Subject.Send(targetsHits);
        }
    }
}

public class TrailblazerDestruction : Player
{
    private static readonly ModifierDoubleImmediate Add20P = new(0.2);
    private bool AFallingStarTriggered;

    // Talent
    private const int MaxStackCount = 2;
    private int PerfectPickOffStackCount;
    private readonly ModifierDoubleImmediate PerfectPickOffAttackModifier = new(0);
    private readonly ModifierDoubleImmediate PerfectPickOffDefenceModifier = new(0);

    // Extra abilities
    private bool ReadyForBattle = false;
    private bool Perseverance = false;
    public bool FightingWill = false;

    public override int EidolonSkillAdd2 => 3;
    public override int EidolonTalentAdd2 => 3;
    public override int EidolonBasicAttackAdd1 => 5;
    public override int EidolonUltimateAdd2 => 5;

    public void Send(TargetsHits targetsHits)
    {
        var targetsHavePhysicalWeakness = false;
        AFallingStarTriggered = false;

        foreach (var (target, hits) in targetsHits)
        {
            if (target.Weaknesses.Eval.Contains(DamageType.Physical))
                targetsHavePhysicalWeakness = true;

            if (Eidolon4 && target.Toughness == 0)
            {
                Add20P.Modify(CriticalRate);
                {
                    Battle.Send(hits);
                }
                Add20P.Dismiss(CriticalRate);
            }
            else
            {
                Battle.Send(hits);
            }
        }

        if (Eidolon2 && targetsHavePhysicalWeakness)
            Heal(0.05 * Attack.Eval);
    }

    public override void EntityJoined(Entity entity)
    {
        if (entity != this) return;
        if (!ReadyForBattle) return;

        RegenerateBoosted(15);
    }

    public override void Died(Entity entity)
    {
        if (!Eidolon1) return;
        if (AFallingStarTriggered) return;
        if (entity.LastDamageSource is not DamageFromEntity damageSourceEntity) return;
        if (damageSourceEntity.Source != this) return;

        RegenerateBoosted(10);
        AFallingStarTriggered = true;
    }

    public override void WeaknessBroken(Entity entity)
    {
        if (entity.LastDamageSource is not DamageFromEntity damageSourceEntity) return;
        if (damageSourceEntity.Source != this) return;

        if (PerfectPickOffStackCount == 0)
        {
            PerfectPickOffAttackModifier.Modify(Attack);

            if (Perseverance)
                PerfectPickOffDefenceModifier.Modify(Defence);
        }

        if (PerfectPickOffStackCount >= MaxStackCount) return;

        PerfectPickOffStackCount += 1;

        PerfectPickOffAttackModifier.Value = LevelTalentMap(0.1, 0.22) * PerfectPickOffStackCount;
        PerfectPickOffDefenceModifier.Value = 0.1 * PerfectPickOffStackCount;
    }

    public override void YourTurn()
    {
        
    }

    protected override void CleanUp()
    {
    }

    public TrailblazerDestruction(string name, int level, double maxHp, double attack, double defence, double speed,
        double energyRegenerationRate, double breakEffect, double effectHitRate, double effectResistance,
        double criticalRate, double criticalDamage, double maxEnergy, TypesModifiableDoubles vulnerability,
        TypesModifiableDoubles resistance, TypesModifiableDoubles resistancePenetration, TypesModifiableDoubles boost,
        Battle battle)
        : base(name, level, maxHp, attack, defence, speed, energyRegenerationRate, breakEffect, effectHitRate,
            effectResistance, criticalRate, criticalDamage, maxEnergy, vulnerability, resistance, resistancePenetration,
            boost, battle)
    {
    }
}
