using SSimulated_Universe.Events;
using SSimulated_Universe.Modifiable.Number;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Entities.Players;

public class FarewellHit : BasicAttack<TrailblazerDestruction>
{
    private readonly Entity Target;
    
    public FarewellHit(TrailblazerDestruction subject, Entity target, Battle battle) : base(subject, battle)
    {
        Target = target;
    }

    public override void Run()
    {
        base.Run();
        
        Subject.RegenerateBoosted(20);

        var targetsHits = Attack.SingleTarget(
            attacker: Subject,
            target: Target,

            hitSplit: Attack.NoSplit,
            baseDamage: new BaseDamage
            {
                WeaknessBreak = 30,
                DamageType = DamageType.Physical,
                BaseAttack = Subject.LevelBasicAttackMap(0.5, 1.1)
            }
        );
        
        Subject.GiveHits(targetsHits);
    }
}

public class RipHomeRun : Skill<TrailblazerDestruction>
{
    private readonly Entity Target;
    
    public RipHomeRun(TrailblazerDestruction subject, Entity target, Battle battle) : base(subject, battle)
    {
        Target = target;
    }

    public override void Run()
    {
        base.Run();

        Subject.RegenerateBoosted(30);

        var targetsHits = Attack.Blast(
            attacker: Subject,
            target: Target,
            battle: Battle,
            
            hitSplit: new List<double> { 1 },
            baseDamage: new BaseDamage
            {
                DamageType = DamageType.Physical,
                BaseAttack = Subject.LevelSkillMap(0.625, 1.375),
                WeaknessBreak = 60,
            },
            
            hitSplitAdjacent: new List<double> { 1 },
            baseDamageAdjacent: new BaseDamage
            {
                DamageType = DamageType.Physical,
                BaseAttack = Subject.LevelSkillMap(0.625, 1.375),
                WeaknessBreak = 30,
            }
        );
    }
}

public class StardustAce1 : Ultimate<TrailblazerDestruction>
{
    private readonly Entity Target;
    
    public StardustAce1(TrailblazerDestruction subject, Entity target, Battle battle) : base(subject, battle)
    {
        Target = target;
    }

    public override void Run()
    {
        base.Run();

        var hits = Attack.SingleTarget(
            attacker: Subject,
            target: Target,
            hitSplit: Attack.NoSplit,
            baseDamage: new BaseDamage
            {
                DamageType = DamageType.Physical,
                BaseAttack = Subject.LevelUltimateMap(3, 4.8),
                WeaknessBreak = 90,
            }
        );
        
        Subject.GiveHits(hits);
    }
}

public class StardustAce3 : Ultimate<TrailblazerDestruction>
{
    private readonly Entity Target;
    
    public StardustAce3(TrailblazerDestruction subject, Entity target, Battle battle) : base(subject, battle)
    {
        Target = target;
    }

    public override void Run()
    {
        base.Run();

        var targetsHits = Attack.Blast(
            attacker: Subject,
            target: Target,
            battle: Battle,
            hitSplit: Attack.NoSplit,
            baseDamage: new BaseDamage
            {
                DamageType = DamageType.Physical,
                BaseAttack = Subject.LevelUltimateMap(1.80, 2.88),
                WeaknessBreak = 60,
            },
            hitSplitAdjacent: Attack.NoSplit,
            baseDamageAdjacent: new BaseDamage
            {
                DamageType = DamageType.Physical,
                BaseAttack = Subject.LevelUltimateMap(1.08, 1.728),
                WeaknessBreak = 60,
            }
        );
        
        Subject.GiveHits(targetsHits);
    }
}

public class TrailblazerDestruction : Player
{
    private static readonly ModifierDoubleImmediate Add20P = new (0.2);
    
    public override int EidolonBasicAttackAdd1 => 5;
    public override int EidolonSkillAdd2 => 3;
    public override int EidolonUltimateAdd2 => 5;
    public override int EidolonTalentAdd2 => 3;
    
    public void GiveHits(TargetsHits targetsHits)
    {
        if (Eidolon4)
        {
            foreach (var (target, hits) in targetsHits)
            {
                if (target.Weaknesses.Eval.Contains(DamageType.Physical))
                {
                    Add20P.Modify(CriticalRate);
                    {
                        foreach (var hit in hits) 
                            hit.Target.TakeHit(hit);
                    }
                    Add20P.Dismiss(CriticalRate);
                }
                else
                {
                    foreach (var hit in hits)
                        hit.Target.TakeHit(hit);
                }
            }
        }
        else
        {
            foreach (var (_, hits) in targetsHits)
            foreach (var hit in hits)
                hit.Target.TakeHit(hit);
        }
    }

    public override void YourTurn()
    {
        throw new NotImplementedException();
    }

    public TrailblazerDestruction(Battle battle) : base(battle) { }
}
