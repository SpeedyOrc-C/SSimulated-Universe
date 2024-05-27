using SSimulated_Universe.Effects;

namespace SSimulated_Universe.Universe;

public abstract class Enemy : Entity
{
    public override void TakeHit(Hit hit)
    {
        Battle.Broadcast(o => o.BeforeTakeHit(hit.Attacker, this));

        LastDamageSource = new DamageFromEntity(hit.Attacker, hit.DamageMethod);
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
                for (var i = 1; i <= brokenGaugesCount; i += 1)
                    TakeBreakDamage();

            if (hit.Attacker.SuperBreakDamageEnabled && Toughness == 0)
                TakeSuperBreakDamage();
        }

        Battle.Broadcast(o => o.AfterTakeHit(hit.Attacker, this));
        return;

        void TakeBreakDamage()
        {
            Battle.Broadcast(o => o.WeaknessBroken(this));

            var breakBaseDamage
                = hit.Attacker.LevelMultiplier
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
                    _                    => throw new InvalidDamageType()
                };

            var breakDamage
                = breakBaseDamage
                * (1 + BreakEffect.Eval)
                * hit.Attacker.DefenceMultiplier(this)
                * hit.Attacker.ResistanceMultiplier(this, hit.DamageType)
                * VulnerabilityMultiplier(hit.DamageType)
                * BrokenMultiplier
                ;

            TakeDamage(breakDamage);

            if (!DebuffInflictionTrial(EffectWeaknessBreak.BaseProbability, hit.Attacker)) return;

            EffectWeaknessBreak debuff = hit.DamageType switch
            {
                DamageType.Physical  => new Bleed(hit.Attacker, this, Battle),
                DamageType.Fire      => new Burn(hit.Attacker, this, Battle),
                DamageType.Ice       => new Freeze(hit.Attacker, this, Battle),
                DamageType.Lightning => new Shock(hit.Attacker, this, Battle),
                DamageType.Wind      => new WindShear(hit.Attacker, this, Battle),
                DamageType.Quantum   => new Entanglement(hit.Attacker, this, Battle),
                DamageType.Imaginary => new Imprisonment(hit.Attacker, this, Battle),
                _                    => throw new InvalidDamageType()
            };

            debuff.Add();
        }

        void TakeSuperBreakDamage()
        {
            var danceWithTheOneMultiplier =
                Battle.SideOf(this).Count switch
                {
                    1   => 0.6,
                    2   => 0.5,
                    3   => 0.4,
                    4   => 0.3,
                    > 5 => 0.2,
                    _   => throw new Exception("No entity at this side.")
                };

            var superBreakDamage
                = hit.Attacker.LevelMultiplier
                * hit.ToughnessDepletion / 30
                * (1 + BreakEffect.Eval)
                * danceWithTheOneMultiplier
                * hit.Attacker.DefenceMultiplier(this)
                * hit.Attacker.ResistanceMultiplier(this, hit.DamageType)
                * VulnerabilityMultiplier(hit.DamageType)
                ;

            TakeDamage(superBreakDamage);
        }
    }

    public override void HpZeroed()
    {
        if (LastDamageSource is null)
            throw new Exception("Cannot find out why this enemy died.");

        Battle.Broadcast(o => o.Died(this));
        Battle.Remove(this);
    }

    public Enemy(
        string name,
        int level,
        bool isEliteOrBoss,
        double maxHp,
        double attack,
        double defence,
        double speed,
        double effectHitRate,
        double effectResistance,
        double criticalRate,
        double criticalDamage,
        double maxToughness,
        double toughnessGaugeSize,
        HashSet<DamageType> weaknesses,
        TypesModifiableDoubles vulnerability,
        TypesModifiableDoubles resistance,
        TypesModifiableDoubles resistancePenetration,
        TypesModifiableDoubles boost,
        Battle battle)

        : base(
            name,
            level,
            isEliteOrBoss,
            maxHp,
            attack,
            defence,
            speed,
            energyRegenerationRate: 0,
            breakEffect: 0,
            effectHitRate,
            effectResistance,
            criticalRate,
            criticalDamage,
            maxEnergy: 0,
            maxToughness,
            toughnessGaugeSize,
            weaknesses,
            vulnerability,
            resistance,
            resistancePenetration,
            boost,
            battle)
    {
    }
}