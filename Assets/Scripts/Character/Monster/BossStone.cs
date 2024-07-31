using System;
using System.Collections;
using System.Collections.Generic;
using INeverFall;
using INeverFall.Manager;
using INeverFall.Player;
using UnityEngine;

public class BossStone : Skill
{
    private const float _force = 15;
    
    private Vector3 _direction;
    private Rigidbody _rigidbody;
    private PlayerCharacter _player;
    
    private bool _isFlying;
    
    public override void Initialize(CharacterBase attacker, int attackPower)
    {
        gameObject.layer = Layer.Rock;
        _rigidbody = GetComponent<Rigidbody>();
        _attacker = attacker;
    }
    
    public void Create(CharacterBase attacker, Vector3 throwDirection, PlayerCharacter player)
    {
        Initialize(attacker, attacker.AttackPower);
        _player = player;
        _rigidbody.isKinematic = true;
        _direction = throwDirection;
        gameObject.SetActive(true);
        _StartDestroy(10);
    }

    public void MoveInDirection()
    {
        _rigidbody.isKinematic = false;
        transform.SetParent(null);
        _rigidbody.velocity = _direction.normalized * 10;
        _isFlying = true;
    }

    private void Update()
    {
        if (_isFlying)
        {
            float forceMagnitude = 20f;
            Vector3 directionToPlayer = (_player.transform.position - transform.position).normalized;
            Vector3 force = directionToPlayer * forceMagnitude;
            _rigidbody.AddForce(force);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // When hit with objects
        if (other.gameObject.layer is Layer.Walkable or Layer.Environment)
        {
            _isFlying = false;
        }
    }
}
