using SSimulated_Universe.Universe;
using SSimulated_Universe.Utility;

namespace SSimulated_Universe.Entities;

public abstract class Player : Entity
{
    protected int _levelBasicAttack = 1;
    protected int _levelTalent = 1;
    protected int _levelUltimate = 1;
    protected int _levelSkill = 1;

    public abstract int EidolonBasicAttackAdd1 { get; }
    public abstract int EidolonSkillAdd2 { get; }
    public abstract int EidolonUltimateAdd2 { get; }
    public abstract int EidolonTalentAdd2 { get; }
    
    public int LevelBasicAttack
    {
        get => _levelBasicAttack + (Eidolon >= EidolonBasicAttackAdd1 ? 1 : 0);
        set => _levelBasicAttack = value - (Eidolon >= EidolonBasicAttackAdd1 ? 1 : 0);
    }
    
    public int LevelSkill
    {
        get => _levelSkill + (Eidolon >= EidolonSkillAdd2 ? 2 : 0);
        set => _levelSkill = value - (Eidolon >= EidolonSkillAdd2 ? 1 : 0);
    }
    
    public int LevelUltimate
    {
        get => _levelUltimate + (Eidolon >= EidolonUltimateAdd2 ? 2 : 0);
        set => _levelUltimate = value - (Eidolon >= EidolonUltimateAdd2 ? 2 : 0);
    }
    
    public int LevelTalent
    {
        get => _levelTalent + (Eidolon >= EidolonTalentAdd2 ? 2 : 0);
        set => _levelTalent = value - (Eidolon >= EidolonTalentAdd2 ? 2 : 0);
    }
    
    public int Eidolon = 0;

    protected Player(Battle battle) : base(battle)
    {
    }

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
}
