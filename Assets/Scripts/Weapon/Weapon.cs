using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INeverFall
{
    public abstract class Weapon : MonoBehaviour
    {
        protected WeaponType _weaponType;
        protected float _attackTiming;

        protected virtual void Start()
        {
            _SetWeaponType();
            _attackTiming = Utils.GetPreciseAttackTime(_weaponType);
        }

        protected abstract void _SetWeaponType();
        public abstract void DoAttack(Vector3 direction);
        
        public WeaponType WeaponType => _weaponType;
        protected Transform _slashRoot => transform.Find("Slash");
    }
}

