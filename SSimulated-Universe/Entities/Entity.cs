using SSimulated_Universe.Events;
using SSimulated_Universe.Modifiable.Number;
using SSimulated_Universe.Modifiable.Set;
using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Entities;

public abstract class Entity : BattleObserver, IRunner
{
    public string Name = "Nameless";
    public bool IsEliteOrBoss = false;
    public bool SkipNextTurn = false;

    public ushort Level = 1;
    public double? MaxEnergy = 100;
    public double Distance;

    public double MaxToughness = 100;
    /// <summary>
    /// When this value is lower than the max toughness,
    /// indicates that this entity has multiple toughness gauges.
    /// </summary>
    public double ToughnessGaugeSize = 100;
    public bool ToughnessDepletionEnabled = true;

    public readonly ModifiableSet<DamageType> Weaknesses = new(new HashSet<DamageType>());

    public readonly ModifiableDouble MaxHp = new(1000);
    public readonly ModifiableDouble Attack = new(1000);
    public readonly ModifiableDouble? Defence = new(1000);
    public readonly ModifiableDouble Speed = new(100);
    public readonly ModifiableDouble EnergyRegenerationRate = new(1);
    public readonly ModifiableDouble WeaknessBreakEfficiency = new(1);
    public readonly ModifiableDouble BreakEffect = new(0);
    public readonly ModifiableDouble EffectHitRate = new(0);
    public readonly ModifiableDouble EffectResistance = new(0);
    public readonly ModifiableDouble CriticalRate = new(0);
    public readonly ModifiableDouble CriticalDamage = new(0);

    public readonly ModifiableDouble BoostShieldGivenOut = new(0);
    public readonly ModifiableDouble BoostShieldReceived = new(0);
    
    public readonly DamageTypeModifiableDoubles Vulnerability = new();
    public readonly DamageTypeModifiableDoubles Resistance = new();
    public readonly DamageTypeModifiableDoubles ResistancePenetration = new();
    public readonly DamageTypeModifiableDoubles Boost = new();

    public double Hp { get; private set; }
    public double Energy { get; private set; }
    public double Toughness { get; private set; }
    
    public readonly Battle Battle;

    public Entity(Battle battle)
    {
        Battle = battle;
    }

    public Side OppositeSide => Battle.OppositeSideOf(this);
    
    /// <summary>
    /// If there is a DEF, this property returns as it is.
    /// Since monsters don't have DEF, their DEF are calculated from their levels.
    /// <br/>
    /// <br/>
    /// <i>Monster's DEF = Level * 10 + 200</i>
    /// </summary>
    public double RealDefence => Defence?.Eval ?? Level * 10 + 200;
    public double RunnerSpeed => Speed.Eval;
    public double RunnerDistance { get => Distance; set => Distance = value; }

    public double DamageBoostMultiplier(DamageType damageType) =>
        1 + Boost.Of(damageType).Eval;

    public double VulnerabilityMultiplier(DamageType damageType) =>
        1 + Vulnerability.Of(damageType).Eval;

    public double MaxToughnessMultiplier =>
        0.5 + MaxToughness / 120;

    public double LevelMultiplier =>
        Entities.LevelMultiplier.Table[Level];

    public double DefenceMultiplier(Entity target) =>
        1 - target.RealDefence / (target.RealDefence + Level * 10 + 200);

    public double ResistanceMultiplier(Entity target, DamageType damageType) =>
        1 + ResistancePenetration.Of(damageType).Eval - target.Resistance.Of(damageType).Eval;

    public double BrokenMultiplier => Toughness > 0 ? 0.9 : 1;
    
    public abstract void YourTurn();

    public void Reset()
    {
        Hp = MaxHp.Eval;
        Toughness = MaxToughness;
        Energy = MaxEnergy ?? 0;
    }

    public void TakeHit(Hit hit)
    {
        Battle.Broadcast(o => o.BeforeTakeHit(hit.Attacker, this));
        
        TakeDamage(hit.Damage);

        var isDepletingToughness =
            ToughnessDepletionEnabled &&
            Weaknesses.Eval.Contains(hit.DamageType) &&
            Toughness > 0;

        if (isDepletingToughness)
        {
            var oldToughness = Toughness;
            Toughness = Math.Max(0, Toughness - hit.ToughnessDepletion);

            var brokenGaugesCount =
                (int) Math.Ceiling(oldToughness / ToughnessGaugeSize) -
                (int) Math.Ceiling(Toughness / ToughnessGaugeSize);

            if (brokenGaugesCount > 0)
            {
                for (var i = 1; i <= brokenGaugesCount; i += 1)
                    Battle.Broadcast(o => o.WeaknessBroken(this));

                var breakBaseDamage
                    = LevelMultiplier
                    * MaxToughnessMultiplier
                    * hit.DamageType switch
                    {
                        DamageType.Physical  => 2,
                        DamageType.Fire      => 2,
                        DamageType.Ice       => 1,
                        DamageType.Lightning => 1,
                        DamageType.Wind      => 1.5,
                        DamageType.Quantum   => 0.5,
                        DamageType.Imaginary => 0.5,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                var breakDamage 
                    = breakBaseDamage
                    * (1 + BreakEffect.Eval)
                    * hit.Attacker.DefenceMultiplier(this)
                    * hit.Attacker.ResistanceMultiplier(this, hit.DamageType)
                    * VulnerabilityMultiplier(hit.DamageType)
                    * BrokenMultiplier;
                    ;

                TakeDamage(breakDamage);
            }

            if (Toughness == 0)
            {
                // TODO: Dance With the One Multiplier
                var danceWithTheOneMultiplier =
                    Battle.SideOf(this).Count switch
                    {
                        1 => 0.6,
                        2 => 0.5,
                        3 => 0.4,
                        4 => 0.3,
                        > 5 => 0.2,
                        _ => throw new Exception("No entity at this side.")
                    };

                var superBreakDamage
                    = LevelMultiplier
                    * hit.ToughnessDepletion / 30
                    * (1 + BreakEffect.Eval)
                    * hit.Attacker.DefenceMultiplier(this)
                    * hit.Attacker.ResistanceMultiplier(this, hit.DamageType)
                    * VulnerabilityMultiplier(hit.DamageType)
                    ;

                TakeDamage(superBreakDamage);
            }
        }

        Battle.Broadcast(o => o.AfterTakeHit(hit.Attacker, this));
    }

    public void Heal(double amount) =>
        ChangeHp(amount);

    public void TakeDamage(double amount) =>
        ChangeHp(-amount);

    public void ChangeHp(double delta)
    {
        if (delta == 0) return;

        var oldHp = Hp;
        Hp = Math.Clamp(Hp + delta, 0, MaxHp.Eval);

        var realDelta = oldHp - Hp;
        if (realDelta == 0) return;

        Battle.Broadcast(o => o.HpChanged(this, realDelta));
    }

    public void Regenerate(double amount) => ChangeEnergy(amount * EnergyRegenerationRate.Eval);
    public void Discharge(double amount) => ChangeEnergy(-amount);

    /// <summary>
    /// Regenerate energy.
    /// Final energy may be larger due to entity's regeneration rate.
    /// </summary>
    public void ChangeEnergy(double delta)
    {
        if (delta == 0) return;

        var oldEnergy = Energy;
        Energy = Math.Clamp(Energy + delta, 0, MaxEnergy ?? 0);

        var realDelta = oldEnergy - Energy;
        if (realDelta == 0) return;

        Battle.Broadcast(o => o.EnergyChanged(this, realDelta));
    }
}
