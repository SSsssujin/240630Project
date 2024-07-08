using System;
using System.Collections;
using System.Collections.Generic;
using INeverFall;
using UnityEngine;

public class BossStone : Skill
{
    private Vector3 _direction;
    private Rigidbody _rigidbody;
    
    protected override void _Initialize(CharacterBase attacker, float speed, int attackPower)
    {
        gameObject.layer = Layer.Rock;
        _rigidbody = GetComponent<Rigidbody>();
        _speed = speed;
        _attacker = attacker;
    }
    
    public void Create(CharacterBase attacker, Vector3 throwDirection)
    {
        _Initialize(attacker, 50, attacker.AttackPower);
        _rigidbody.isKinematic = true;
        _direction = throwDirection;
        gameObject.SetActive(true);
    }

    public void MoveInDirection()
    {
        _rigidbody.isKinematic = false;
        transform.SetParent(null);
        _rigidbody.velocity = _direction.normalized * _speed;
    }
}
