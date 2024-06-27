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
        
        public override void DoAttack(Vector3 direction)
        {
            _direction = direction;
            StartCoroutine(nameof(_cStartAttack));
        }

        private IEnumerator _cStartAttack()
        {
            yield return new WaitForSeconds(_attackTiming);
            
            var skill = ResourceManager.Instance.Instantiate("TestSlash");
            skill.DemandComponent<Skill>().Initialize(_slashRoot, _direction);
        }
    }
}

