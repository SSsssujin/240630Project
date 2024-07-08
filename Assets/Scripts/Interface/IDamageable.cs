using INeverFall;
using UnityEngine;

public interface IDamageable
{
    public void OnDamage(CharacterBase owner, int damage);
}
