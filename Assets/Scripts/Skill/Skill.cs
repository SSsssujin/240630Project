using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INeverFall
{
    [RequireComponent(typeof(Collider))]
    public abstract class Skill : MonoBehaviour
    {
        protected float _speed;
        protected int _attackPower;
        
        protected CharacterBase _attacker;

        protected abstract void _Initialize(CharacterBase attacker, float speed, int attackPower);

        protected void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IDamageable>(out var character))
            {
                if (_attacker == (CharacterBase)character)
                {
                    return;
                }
                
                character.OnDamage(_attacker, _attackPower);
            }
        }
    }
}
