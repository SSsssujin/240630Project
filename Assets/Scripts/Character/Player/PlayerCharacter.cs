using System;
using System.Collections;
using UnityEngine;

namespace INeverFall.Player
{
    public class PlayerCharacter : CharacterBase
    {
        private const int _criticalDamage = 10;

        private bool _isInvincible;
        private Animator _animator;
        
        protected override void Start()
        {
            _maxHp = _hp = 100;
            _attackPower = 100;

            _animator = GetComponent<Animator>();
        }

        public override void OnDamage(CharacterBase owner, int damage)
        {
            if (_isInvincible || !GameManager.Instance.IsGamePlaying) return;
            
            base.OnDamage(owner, damage);
            
            var currentHpRate = (float)_hp / _maxHp;
            HpChanged?.Invoke(currentHpRate);
            
            // Damaged animation
            _animator.SetTrigger(AnimationID.Damage);
            var isCritical = true;//damage >= _criticalDamage;
            _animator.SetBool(AnimationID.IsCritical, isCritical);
            
            StartCoroutine(_cFlashDamagedColor());
            StartCoroutine(_cCheckInvincible(isCritical ? 3.5f : 1.2f));
        }

        // 데미지 애니메이션 재생 동안 무적
        private IEnumerator _cCheckInvincible(float time)
        {
            _isInvincible = true;
            _animator.SetFloat(AnimationID.VelocityX,0);
            _animator.SetFloat(AnimationID.VelocityZ,0);
            yield return new WaitForSeconds(time);
            _isInvincible = false;
        }

        private IEnumerator _cFlashDamagedColor()
        {
            var mat = transform.Find("Renderer")
                .GetComponentInChildren<SkinnedMeshRenderer>()
                .material;
            mat.color = Color.red;
            yield return new WaitForSeconds(0.25f);
            mat.color = Color.white;
        }

        protected override void _OnDead()
        {
            _animator.SetTrigger(AnimationID.Death);
            PlayerDead?.Invoke();
        }

        public event Action PlayerDead;
        public event Action<float> HpChanged;

        public bool IsInvincible => _isInvincible;
    }
}