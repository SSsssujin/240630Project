using System;
using System.Collections;
using System.Collections.Generic;
using INeverFall.Manager;
using UnityEngine;

namespace INeverFall
{
    [RequireComponent(typeof(Collider))]
    public abstract class Skill : MonoBehaviour
    {
        protected int _attackPower;
        protected bool _isChecking { get; private set; } = true;

        protected Collider _collider;
        protected CharacterBase _attacker;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public abstract void Initialize(CharacterBase attacker, int attackPower);

        protected void OnTriggerEnter(Collider other)
        {
            if (!_isChecking) return;
            
            if (other.TryGetComponent<IDamageable>(out var character))
            {
                if (_attacker == character as CharacterBase)
                {
                    return;
                }
                
                character.OnDamage(_attacker, _attackPower);
                _isChecking = false;

                _hitPoint = other.ClosestPoint(transform.position);
                _OnDamage();
            }
        }

        protected void _StartDestroy(float lifeTime)
        {
            StartCoroutine(_cStartDestroy(lifeTime));
        }
        
        private IEnumerator _cStartDestroy(float lifeTime)
        {
            yield return new WaitForSeconds(lifeTime);
            ResourceManager.Instance.Destroy(gameObject);
        }

        protected void _ActivateCollider()
        {
            _collider.enabled = true;
        }
        
        protected void _DeactivateCollider()
        {
            _collider.enabled = false;
        }

        protected Vector3 _hitPoint;

        protected virtual void _OnDamage() { }
    }
}
