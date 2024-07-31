using INeverFall;

namespace INeverFall.Monster
{
    public class BossMeleeSkill : Skill
    {
        public override void Initialize(CharacterBase attacker, int attackPower)
        {
            _attacker = attacker;
            _attackPower = attackPower;
        }
    }
}