using INeverFall;

public class FireSlash : Skill
{
    public override void Initialize(CharacterBase attacker, int attackPower)
    {
        _attacker = attacker;
        _attackPower = attackPower;

        _StartDestroy(4);
    }
}
