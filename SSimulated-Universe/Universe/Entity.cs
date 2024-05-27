using SSimulated_Universe.Utility;
using SSimulated_Universe.Utility.Modifiable.Number;
using SSimulated_Universe.Utility.Modifiable.Set;

namespace SSimulated_Universe.Universe;

public abstract class Entity : BattleObserver, IRunner
{

    public readonly string Name;
    public readonly int    Level;
    public readonly bool   IsEliteOrBoss;

    public readonly ModifiableDouble          MaxHp;
    public readonly ModifiableDouble          Attack;
    public readonly ModifiableDouble          Defence;
    public readonly ModifiableDouble          Speed;
    public readonly ModifiableDouble          EnergyRegenerationRate;
    public readonly ModifiableDouble          BreakEffect;
    public readonly ModifiableDouble          WeaknessBreakEfficiency;
    public readonly ModifiableDouble          EffectHitRate;
    public readonly ModifiableDouble          EffectResistance;
    public readonly ModifiableDouble          CriticalRate;
    public readonly ModifiableDouble          CriticalDamage;
    public readonly double                    MaxEnergy;
    public readonly double                    MaxToughness;
    public readonly double                    ToughnessGaugeSize;
    public readonly ModifiableSet<DamageType> Weaknesses;
    public readonly TypesModifiableDoubles    Vulnerability;
    public readonly TypesModifiableDoubles    Resistance;
    public readonly TypesModifiableDoubles    ResistancePenetration;
    public readonly TypesModifiableDoubles    Boost;

    public double Hp;
    public double Energy;
    public double Toughness;
    public double Distance;

    public bool SkipNextTurn;
    public bool SuperBreakDamageEnabled;
    public bool ToughnessDepletionEnabled;

    public IDamageSource? LastDamageSource;

    public double RunnerSpeed => Speed.Eval;
    public double RunnerDistance { get => Distance; set => Distance = value; }

    public double DamageBoostMultiplier(DamageType damageType) =>
        1 + Boost.Of(damageType).Eval;

    public double VulnerabilityMultiplier(DamageType damageType) =>
        1 + Vulnerability.Of(damageType).Eval;

    public double LevelMultiplier =>
        Universe.LevelMultiplier.Table[Level];

    public double DefenceMultiplier(Entity target) =>
        1 - target.RealDefence / (target.RealDefence + Level * 10 + 200);

    public double ResistanceMultiplier(Entity target, DamageType damageType) =>
        1 + ResistancePenetration.Of(damageType).Eval - target.Resistance.Of(damageType).Eval;

    public bool DebuffInflictionTrial(double probability, Entity attacker) =>
        // TODO: Debuff Resistance
        SMath.BernoulliTrial(probability * (1 + attacker.EffectHitRate.Eval) * (1 - EffectResistance.Eval));

    public double MaxToughnessMultiplier => 0.5 + MaxToughness / 120;
    public double BrokenMultiplier => Toughness > 0 ? 0.9 : 1;

    public double RealDefence => Level * 10 + 200;

    public Side We => Battle.SideOf(this);
    public Side Opposite => Battle.OppositeSideOf(this);

    public abstract void YourTurn();
    protected abstract void CleanUp();

    public void TakeEffectDamage(double damage, EffectTimed effectTimed)
    {
        LastDamageSource = new DamageFromEffect(effectTimed);
        TakeDamage(damage);
    }

    public void Heal(double amount) => ChangeHp(amount);
    protected void TakeDamage(double amount) => ChangeHp(-amount);

    private void ChangeHp(double amount)
    {
        if (amount == 0) return;

        var oldHp = Hp;
        Hp = Math.Clamp(Hp + amount, 0, MaxHp.Eval);
        var delta = oldHp - Hp;

        if (delta == 0) return;

        Battle.Broadcast(o => o.HpChanged(this, delta));

        if (Hp > 0) return;

        HpZeroed();
        Battle.Broadcast(o => o.HpZeroed(this));
    }

    public void RegenerateBoosted(double amount) => ChangeEnergy(amount * EnergyRegenerationRate.Eval);
    public void Regenerate(double amount) => ChangeEnergy(amount);
    public void Discharge(double amount) => ChangeEnergy(-amount);
    public void Discharge() => ChangeEnergy(-Energy);

    private void ChangeEnergy(double amount)
    {
        if (amount == 0) return;

        var oldEnergy = Energy;
        Energy = Math.Clamp(Energy + amount, 0, MaxEnergy);

        var delta = oldEnergy - Energy;
        if (delta == 0) return;

        Battle.Broadcast(o => o.EnergyChanged(this, delta));
    }

    public abstract void HpZeroed();
    public abstract void TakeHit(Hit hit);

    protected readonly Battle Battle;

    public Entity(
        string name,
        int level,
        bool isEliteOrBoss,
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
        double maxToughness,
        double toughnessGaugeSize,
        HashSet<DamageType> weaknesses,
        TypesModifiableDoubles vulnerability,
        TypesModifiableDoubles resistance,
        TypesModifiableDoubles resistancePenetration,
        TypesModifiableDoubles boost,
        Battle battle)
    {
        Name = name;
        Level = level;
        IsEliteOrBoss = isEliteOrBoss;

        MaxHp = new ModifiableDouble(maxHp);
        Attack = new ModifiableDouble(attack);
        Defence = new ModifiableDouble(defence);
        Speed = new ModifiableDouble(speed);
        EnergyRegenerationRate = new ModifiableDouble(energyRegenerationRate);
        BreakEffect = new ModifiableDouble(breakEffect);
        WeaknessBreakEfficiency = new ModifiableDouble(1);
        EffectHitRate = new ModifiableDouble(effectHitRate);
        EffectResistance = new ModifiableDouble(effectResistance);
        CriticalRate = new ModifiableDouble(criticalRate);
        CriticalDamage = new ModifiableDouble(criticalDamage);
        MaxEnergy = maxEnergy;
        MaxToughness = maxToughness;
        ToughnessGaugeSize = toughnessGaugeSize;
        Weaknesses = new ModifiableSet<DamageType>(weaknesses);
        Vulnerability = vulnerability;
        Resistance = resistance;
        ResistancePenetration = resistancePenetration;
        Boost = boost;

        Hp = maxHp;
        Energy = 0;
        Toughness = MaxToughness;
        Distance = 0;

        SkipNextTurn = false;
        SuperBreakDamageEnabled = false;
        ToughnessDepletionEnabled = false;

        LastDamageSource = null;

        Battle = battle;
    }
}
