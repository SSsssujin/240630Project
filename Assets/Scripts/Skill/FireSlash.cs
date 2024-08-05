using INeverFall;
using INeverFall.Manager;

public class FireSlash : Skill
{
    public override void Initialize(CharacterBase attacker, int attackPower)
    {
        _attacker = attacker;
        _attackPower = attackPower;

        _StartDestroy(4);
    }

    protected override void _OnDamage()
    {
        if (ResourceManager.Instance.Instantiate("HitEffect")
            .TryGetComponent<HitEffect>(out var hitEffect))
        {
            hitEffect.Activate(_hitPoint);
        }
    }
}
