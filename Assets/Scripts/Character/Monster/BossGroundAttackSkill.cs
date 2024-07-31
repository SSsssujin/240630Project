using System;
using System.Collections;
using System.Collections.Generic;
using INeverFall.Monster;
using UnityEngine;

namespace INeverFall
{
    public class BossGroundAttackSkill : Skill
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
            
            // Set transform
            transform.localScale = Vector3.one * 1.5f;
            transform.position = attacker.transform.position +
                                 attacker.transform.forward * 7.5f;

            _fx = transform.GetChild(0).gameObject;
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= 1.87f)
            {
                _ActivateCollider();
                _fx.SetActive(true);
            }

            if (_timer >= 2f)
            {
               _DeactivateCollider();
            }
        }
    }
}