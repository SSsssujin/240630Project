using System.Collections;
using System.Collections.Generic;
using INeverFall;
using UnityEngine;

public class SlashSkill : Skill
{
    private bool _isActivate;
    private Vector3 _attackDirection;
    
    protected override void _Initialize(CharacterBase attacker, float speed, int attackPower)
    {
        _attacker = attacker;
        _speed = speed;
        _attackPower = attackPower;
    }
    
    public void Create(CharacterBase attacker, Transform parent, Vector3 direction)
    {
        _Initialize(attacker, 30, attacker.AttackPower);   

        
        transform.position = parent.position + new Vector3(0, -1f, 0);
        _attackDirection = direction;
        _isActivate = true;
    }

    private void Update()
    {
        if (_isActivate)
        {
            transform.position += _attackDirection * _speed * Time.deltaTime;
        }
    }


}
