// // Hello Star Rail!
//
// using SSimulated_Universe.Universe;
// using SSimulated_Universe.Utility.Modifiable.Number;
//
// namespace SSimulated_Universe.Players;
//
// public class FarewellHit : BasicAttack<TrailblazerDestruction>
// {
//     private readonly Enemy Target;
//
//     public FarewellHit(TrailblazerDestruction subject, Enemy target, Battle battle) : base(subject, battle)
//     {
//         Target = target;
//     }
//
//     public override void Run()
//     {
//         base.Run();
//
//         Subject.RegenerateBoosted(20);
//
//         var targetsHits = Attack.SingleTarget(
//             attacker: Subject,
//             target: Target,
//             damageMethod: DamageMethod.BasicAttack,
//             hitSplit: Attack.One,
//             baseDamage: new BaseDamage
//             {
//                 WeaknessBreak = 30,
//                 DamageType = DamageType.Physical,
//                 BaseAttack = Subject.LevelBasicAttackMap(0.5, 1.1)
//             }
//         );
//
//         Subject.GiveTargetsHits(targetsHits);
//     }
// }
//
// public class RipHomeRun : Skill<TrailblazerDestruction>
// {
//     private readonly Enemy Target;
//     private static readonly ModifierDoubleImmediate DamageBooster = new(0.25);
//
//     public RipHomeRun(TrailblazerDestruction subject, Enemy target, Battle battle) : base(subject, battle)
//     {
//         Target = target;
//     }
//
//     public override void Run()
//     {
//         base.Run();
//
//         Subject.RegenerateBoosted(30);
//
//         var targetsHits = Attack.Blast(
//             attacker: Subject,
//             target: Target,
//             damageMethod: DamageMethod.Skill,
//             battle: Battle,
//
//             hitSplit: Attack.One,
//             baseDamage: new BaseDamage
//             {
//                 DamageType = DamageType.Physical,
//                 BaseAttack = Subject.LevelSkillMap(0.625, 1.375),
//                 WeaknessBreak = 60
//             },
//
//             hitSplitAdjacent: Attack.One,
//             baseDamageAdjacent: new BaseDamage
//             {
//                 DamageType = DamageType.Physical,
//                 BaseAttack = Subject.LevelSkillMap(0.625, 1.375),
//                 WeaknessBreak = 30
//             }
//         );
//
//         if (Subject.FightingWill)
//         {
//             DamageBooster.Modify(Subject.Boost.Physical);
//             {
//                 Subject.GiveTargetsHits(targetsHits);
//             }
//             DamageBooster.Dismiss(Subject.Boost.Physical);
//         }
//         else
//         {
//             Subject.GiveTargetsHits(targetsHits);
//         }
//     }
// }
//
// public class StardustAce_BlowoutFarewellHit : Ultimate<TrailblazerDestruction>
// {
//     private readonly Enemy Target;
//
//     public StardustAce_BlowoutFarewellHit(TrailblazerDestruction subject, Enemy target, Battle battle) : base(subject, battle)
//     {
//         Target = target;
//     }
//
//     public override void Run()
//     {
//         base.Run();
//
//         var hits = Attack.SingleTarget(
//             attacker: Subject,
//             target: Target,
//             damageMethod: DamageMethod.Ultimate,
//             hitSplit: Attack.One,
//             baseDamage: new BaseDamage
//             {
//                 DamageType = DamageType.Physical,
//                 BaseAttack = Subject.LevelUltimateMap(3, 4.8),
//                 WeaknessBreak = 90
//             }
//         );
//
//         Subject.GiveTargetsHits(hits);
//     }
// }
//
// public class StardustAce_BlowoutRipHomeRun : Ultimate<TrailblazerDestruction>
// {
//     private readonly Enemy Target;
//     private static readonly ModifierDoubleImmediate DamageBooster = new(0.25);
//
//     public StardustAce_BlowoutRipHomeRun(TrailblazerDestruction subject, Enemy target, Battle battle) : base(subject, battle)
//     {
//         Target = target;
//     }
//
//     public override void Run()
//     {
//         base.Run();
//
//         var targetsHits = Attack.Blast(
//             attacker: Subject,
//             target: Target,
//             damageMethod: DamageMethod.Ultimate,
//             battle: Battle,
//
//             hitSplit: Attack.One,
//             baseDamage: new BaseDamage
//             {
//                 DamageType = DamageType.Physical,
//                 BaseAttack = Subject.LevelUltimateMap(1.80, 2.88),
//                 WeaknessBreak = 60
//             },
//
//             hitSplitAdjacent: Attack.One,
//             baseDamageAdjacent: new BaseDamage
//             {
//                 DamageType = DamageType.Physical,
//                 BaseAttack = Subject.LevelUltimateMap(1.08, 1.728),
//                 WeaknessBreak = 60
//             }
//         );
//
//         if (Subject.FightingWill)
//         {
//             DamageBooster.Modify(Subject.Boost.Physical);
//             {
//                 Subject.GiveTargetsHits(targetsHits);
//             }
//             DamageBooster.Dismiss(Subject.Boost.Physical);
//         }
//         else
//         {
//             Subject.GiveTargetsHits(targetsHits);
//         }
//     }
// }
//
// public class PerfectPickoff : EffectSelf<TrailblazerDestruction>
// {
//     public const int MaxStackCount = 2;
//
//     private int StackCount;
//     private readonly ModifierDoubleImmediate AttackModifier = new(0);
//     private readonly ModifierDoubleImmediate DefenceModifier = new(0);
//
//     public PerfectPickoff(TrailblazerDestruction self, Battle battle) : base(self, battle) { }
//
//     public override void WeaknessBroken(Entity entity)
//     {
//         if (entity.LastDamageSource is not DamageFromEntity damageSourceEntity) return;
//         if (damageSourceEntity.Source != Self) return;
//
//         if (StackCount == 0)
//         {
//             AttackModifier.Modify(Self.Attack);
//
//             if (Self.Perseverance)
//                 DefenceModifier.Modify(Self.Defence);
//         }
//
//         if (StackCount < MaxStackCount)
//             StackCount += 1;
//
//         AttackModifier.Value = Self.LevelTalentMap(0.1, 0.22) * StackCount;
//         DefenceModifier.Value = 0.1 * StackCount;
//     }
//
//     public override void Added() { }
//
//     public override void Removed()
//     {
//         AttackModifier.DismissAll();
//     }
// }
//
// public class TrailblazerDestruction : Player
// {
//     private static readonly ModifierDoubleImmediate Add20P = new(0.2);
//     private bool AFallingStarTriggered;
//
//     // Talent
//     private readonly PerfectPickoff PerfectPickoff;
//
//     // Extra abilities
//     public bool ReadyForBattle = false;
//     public bool Perseverance = false;
//     public bool FightingWill = false;
//
//     public override int EidolonSkillAdd2 => 3;
//     public override int EidolonTalentAdd2 => 3;
//     public override int EidolonBasicAttackAdd1 => 5;
//     public override int EidolonUltimateAdd2 => 5;
//
//     public void GiveTargetsHits(TargetsHits targetsHits)
//     {
//         var targetsHavePhysicalWeakness = false;
//         AFallingStarTriggered = false;
//
//         foreach (var (target, hits) in targetsHits)
//         {
//             if (target.Weaknesses.Eval.Contains(DamageType.Physical))
//                 targetsHavePhysicalWeakness = true;
//
//             if (Eidolon4 && target.Toughness == 0)
//             {
//                 Add20P.Modify(CriticalRate);
//                 {
//                     GiveTargetHits(target, hits);
//                 }
//                 Add20P.Dismiss(CriticalRate);
//             }
//             else
//             {
//                 GiveTargetHits(target, hits);
//             }
//         }
//
//         if (Eidolon2 && targetsHavePhysicalWeakness)
//             Heal(0.05 * Attack.Eval);
//     }
//
//     private static void GiveTargetHits(Enemy target, IEnumerable<Hit> hits)
//     {
//         foreach (var hit in hits)
//             target.TakeHit(hit);
//     }
//
//     public override void EntityJoined(Entity entity)
//     {
//         if (entity != this) return;
//         if (!ReadyForBattle) return;
//
//         RegenerateBoosted(15);
//     }
//
//     public override void Died(Entity entity)
//     {
//         if (!Eidolon1) return;
//         if (AFallingStarTriggered) return;
//         if (entity.LastDamageSource is not DamageFromEntity damageSourceEntity) return;
//         if (damageSourceEntity.Source != this) return;
//
//         RegenerateBoosted(10);
//         AFallingStarTriggered = true;
//     }
//
//     public override void YourTurn()
//     {
//         // TODO: Please say something!
//     }
//
//     protected override void CleanUp()
//     {
//         PerfectPickoff.Removed();
//     }
//
//     public TrailblazerDestruction(Battle battle) : base(battle)
//     {
//         PerfectPickoff = new PerfectPickoff(this, Battle);
//     }
// }
