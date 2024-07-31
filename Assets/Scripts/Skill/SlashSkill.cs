using System.Collections;
using System.Collections.Generic;
using INeverFall;
using UnityEngine;

public class SlashSkill : Skill
{
    private bool _isActivate;
    private Vector3 _attackDirection;
    
    public override void Initialize(CharacterBase attacker, int attackPower)
    {
        _attacker = attacker;
        _attackPower = attackPower;
    }
    
    public void Create(CharacterBase attacker, Transform parent, Vector3 direction)
    {
        Initialize(attacker, attacker.AttackPower);   
        
        transform.position = parent.position + new Vector3(0, -1f, 0);
        _attackDirection = direction;
        _isActivate = true;
    }

    private void Update()
    {
        // if (_isActivate)
        // {
        //     transform.position += _attackDirection * _speed * Time.deltaTime;
        // }
    }


}
