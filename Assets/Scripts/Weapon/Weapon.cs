using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INeverFall
{
    public abstract class Weapon : MonoBehaviour
    {
        protected float _attackTiming;
        protected WeaponType _weaponType;

        protected virtual void Start()
        {
            _SetWeaponType();
            _attackTiming = Utils.GetPreciseAttackTime(_weaponType);
        }

        protected abstract void _SetWeaponType();
        public abstract void DoAttack(CharacterBase attacker, Vector3 direction);
        
        public WeaponType WeaponType => _weaponType;
        protected Transform _slashRoot => transform.Find("Slash");
    }
}

