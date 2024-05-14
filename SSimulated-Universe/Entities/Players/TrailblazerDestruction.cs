using SSimulated_Universe.Events;
using SSimulated_Universe.Universe;
using SSimulated_Universe.Utility;

namespace SSimulated_Universe.Entities.Players;

class FarewellHit : BasicAttack
{
    private readonly Entity Target;
    
    public FarewellHit(Player subject, Entity target, Battle battle) : base(subject, battle)
    {
        Target = target;
    }

    public override void Run()
    {
        base.Run();
        
        Subject.Regenerate(20);
        
        var hits = Attack.SingleTarget(
            attacker: Subject, 
            target: Target,
            
            hitSplit: new List<double> { 1 },
            baseDamage: new BaseDamage 
            {
                WeaknessBreak = 30,
                DamageType = DamageType.Physical,
                BaseAttack = SMath.LevelMap(
                    level: Subject.LevelBasicAttack,
                    maxLevel: 7,
                    minValue: 0.5,
                    maxValue: 1.1
                )
            }
        );

        foreach (var hit in hits)
            hit.Target.TakeHit(hit);
    }
}

class RipHomeRun : Skill
{
    private readonly Entity Target;
    
    public RipHomeRun(Player subject, Entity target, Battle battle) : base(subject, battle)
    {
        Target = target;
    }

    public override void Run()
    {
        base.Run();

        Subject.Regenerate(30);

        var hits = Attack.Blast(
            attacker: Subject,
            target: Target,
            battle: Battle,
            
            hitSplit: new List<double> { 1 },
            baseDamage: new BaseDamage
            {
                DamageType = DamageType.Physical,
                BaseAttack = SMath.LevelMap(
                    level: Subject.LevelSkill,
                    maxLevel: 12,
                    minValue: 0.625, 
                    maxValue: 137.5
                ),
                WeaknessBreak = 60,
            },
            
            hitSplitAdjacent: new List<double> { 1 },
            baseDamageAdjacent: new BaseDamage
            {
                DamageType = DamageType.Physical,
                BaseAttack = SMath.LevelMap(
                    level: Subject.LevelSkill,
                    maxLevel: 12,
                    minValue: 0.625,
                    maxValue: 137.5
                ),
            }
        );
        
        foreach (var hit in hits)
            hit.Target.TakeHit(hit);
    }
}

class TrailblazerDestruction : Player
{
    // public AttackSingle StardustAce_Single(Entity target) => new(
    //     Battle,
    //     energy: 5,
    //     skillPointCost: 0,
    //     skillPointGain: 0,
    //     subject: this,
    //     Battle,
    //     @event: new AttackSingle(
    //         attacker: this,
    //         target,
    //         new BaseDamage
    //         {
    //             DamageType = DamageType.Physical,
    //             BaseAttack = SMath.LevelMap(
    //                 level: LevelUltimate,
    //                 maxLevel: 12,
    //                 minValue: 3,
    //                 maxValue: 4.8
    //             ),
    //             WeaknessBreak = 90,
    //         }
    //     )
    // );
    //
    // public EntityAction StardustAce_Blast(Entity target) => new(
    //     subject: this,
    //     Battle,
    //     energy: 5,
    //     skillPointCost: 0,
    //     skillPointGain: 0,
    //     @event: new AttackBlast(
    //         attacker: this,
    //         target,
    //         Battle,
    //         baseDamage: new BaseDamage
    //         {
    //             DamageType = DamageType.Physical,
    //             BaseAttack = SMath.LevelMap(
    //                 level: LevelSkill,
    //                 maxLevel: 12,
    //                 minValue: 1.8, 
    //                 maxValue: 2.88
    //             ),
    //             WeaknessBreak = 60,
    //         },
    //         adjacentBaseDamage: new BaseDamage
    //         {
    //             DamageType = DamageType.Physical,
    //             BaseAttack = SMath.LevelMap(
    //                 level: LevelSkill,
    //                 maxLevel: 12,
    //                 minValue: 1.08,
    //                 maxValue: 1.728
    //             ),
    //             WeaknessBreak = 60,
    //         }
    //     )
    // );
    //
    // public override Event YourTurn()
    // {
    //     var target = Battle.OppositeSideOf(this).EntityAt(0);
    //     return FarewellHit(target);
    // }

    public TrailblazerDestruction(Battle battle) : base(battle) { }

    public override void YourTurn()
    {
        
    }
}
