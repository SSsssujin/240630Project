using System;
using System.Collections;
using INeverFall.Manager;
using UnityEngine;

namespace INeverFall
{
    public class TwoHandSword : Weapon
    {
        protected override void _SetWeaponType() => _weaponType = WeaponType.TwoHandSword;

        private Vector3 _direction;
        private CharacterBase _attacker;
        
        public override void DoAttack(CharacterBase attacker, Vector3 direction)
        {
            _attacker = attacker;
            _direction = direction;
            
            StartCoroutine(nameof(_cStartAttack));
        }

        private IEnumerator _cStartAttack()
        {
            yield return new WaitForSeconds(_attackTiming);
            
            var skill = ResourceManager.Instance.Instantiate("TestSlash");
            skill.DemandComponent<SlashSkill>().Create(_attacker, _slashRoot, _direction);
        }
    }
}

