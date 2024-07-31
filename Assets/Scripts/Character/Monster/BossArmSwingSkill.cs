using System.Collections;
using System.Collections.Generic;
using INeverFall;
using UnityEngine;

public class BossArmSwingSkill : Skill
{
    private float _timer;
        
    private GameObject _fx;
        
    public override void Initialize(CharacterBase attacker, int attackPower)
    {
        _timer = 0;
        _collider.isTrigger = true;
        _DeactivateCollider();

        _attacker = attacker;
        _attackPower = attackPower;
        
        transform.SetParent(attacker.transform);
        transform.ResetLocal();

        _fx = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= 1.75f)
        {
            _ActivateCollider();
            _fx.SetActive(true);
        }

        if (_timer >= 1.8f)
        {
            _DeactivateCollider();
        }
    }
}
