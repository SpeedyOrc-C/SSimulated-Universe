using SSimulated_Universe.Universe;

namespace SSimulated_Universe.Entities;

public abstract class Player : Entity
{
    public int LevelBasicAttack = 1;
    public int LevelTalent = 1;
    public int LevelUltimate = 1;
    public int LevelSkill = 1;

    protected Player(Battle battle) : base(battle)
    {
    }
}
