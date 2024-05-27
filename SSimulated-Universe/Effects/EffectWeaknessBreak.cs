using SSimulated_Universe.Universe;
using SSimulated_Universe.Utility.Modifiable.Number;

namespace SSimulated_Universe.Effects;

public abstract class EffectWeaknessBreak : EffectTimed
{
    public const double BaseProbability = 1.5;

    public override void BeforeTurnOf(Entity entity)
    {
        if (entity != Enemy) return;

        BeforeTurnOfTarget();
        base.BeforeTurnOf(entity);
    }

    protected virtual void BeforeTurnOfTarget() =>
        Enemy.TakeEffectDamage(Damage, this);

    private double Damage =>
        BaseDamage
        * (1 + Player.BreakEffect.Eval)
        * Player.DefenceMultiplier(Enemy)
        * Player.ResistanceMultiplier(Enemy, DamageType.Physical)
        * Enemy.VulnerabilityMultiplier(DamageType.Physical)
        * Enemy.BrokenMultiplier;

    protected abstract double BaseDamage { get; }

    protected readonly Entity Player;
    protected readonly Entity Enemy;

    protected EffectWeaknessBreak(Entity player, Entity enemy, uint duration, Battle battle)
        : base(duration, battle)
    {
        Player = player;
        Enemy = enemy;
    }
}

public class Burn : EffectWeaknessBreak
{
    public override void Added() { }
    public override void Removed() { }

    public Burn(Entity player, Entity enemy, Battle battle) : base(player, enemy, 2, battle) { }

    protected override double BaseDamage => Player.LevelMultiplier;
}

public class Shock : EffectWeaknessBreak
{
    public override void Added() { }
    public override void Removed() { }

    public Shock(Entity player, Entity enemy, Battle battle) : base(player, enemy, 2, battle) { }

    protected override double BaseDamage => 2 * Player.LevelMultiplier;
}

public class Bleed : EffectWeaknessBreak
{
    public override void Added() { }
    public override void Removed() { }

    public Bleed(Entity player, Entity enemy, Battle battle) : base(player, enemy, 2, battle) { }

    protected override double BaseDamage
    {
        get
        {
            var damageCap = 2 * Player.LevelMultiplier * Enemy.MaxToughnessMultiplier;
            var damage = Enemy.MaxHp.Eval * (Enemy.IsEliteOrBoss ? 0.07 : 0.16);
            return Math.Min(damageCap, damage);
        }
    }
}

public class WindShear : EffectWeaknessBreak
{
    public override void Added() { }
    public override void Removed() { }

    private readonly int _stackCount;

    public WindShear(Entity player, Entity enemy, Battle battle)
        : base(player, enemy, 2, battle)
        => _stackCount = enemy.IsEliteOrBoss ? 3 : 1;

    protected override double BaseDamage => _stackCount * Player.LevelMultiplier;
}

public class Entanglement : EffectWeaknessBreak
{
    public override void Added()
    {
        Enemy.RunnerDistance -= 0.2 * Player.BreakEffect.Eval;
    }

    public override void Removed() { }

    private const int MaxStackCount = 5;

    private int _stackCount = 1;

    public Entanglement(Entity player, Entity enemy, Battle battle) : base(player, enemy, 1, battle) { }

    protected override double BaseDamage =>
        0.6 * _stackCount * Player.LevelMultiplier * Enemy.MaxToughnessMultiplier;

    public override void AfterTakeAttack(Entity attacker, Entity target)
    {
        if (target != Enemy) return;
        if (_stackCount == MaxStackCount) return;

        _stackCount += 1;
    }
}

public class Freeze : EffectWeaknessBreak
{
    public override void Added() { }
    public override void Removed() { }

    public Freeze(Entity player, Entity enemy, Battle battle) : base(player, enemy, 1, battle) { }

    protected override double BaseDamage => Player.LevelMultiplier;

    protected override void BeforeTurnOfTarget()
    {
        base.BeforeTurnOfTarget();

        Enemy.SkipNextTurn = true;
        Enemy.Distance = 0.5;
    }
}

/// <summary>
/// Delays action by 30% * (1 + break effect).
/// <br/>
/// Reduces the speed by 10%.
/// </summary>
public class Imprisonment : EffectWeaknessBreak
{
    public override void Added()
    {
        Enemy.RunnerDistance -= 0.3 * Player.BreakEffect.Eval;
        _sub10P.Modify(Enemy.Speed);
    }

    public override void Removed()
    {
        _sub10P.DismissAll();
    }

    private readonly ModifierDoubleProportion _sub10P = new(-0.1);

    protected override double BaseDamage => 0;

    public Imprisonment(Entity player, Entity enemy, Battle battle) : base(player, enemy, 1, battle) { }
}
