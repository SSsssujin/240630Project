using System.Collections;
using System.Collections.Generic;
using INeverFall;
using INeverFall.Monster;
using UnityEngine;

namespace INeverFall.Monster
{
    public abstract class AttackState : State
    {
        protected readonly BossMonster _controller;

        protected bool _isCoolTimeEnded = true;
        protected float _entranceTimer = 0;
        protected BossAnimation _animation;

        // CoolTime
        private Coroutine _coolTimeCoroutine;
        private WaitForSeconds _coolTimeWFS;

        protected AttackState(BossMonster controller)
        {
            _controller = controller;
        }

        public sealed override void Enter()
        {
            _Enter();
            _StartCoolTimeCoroutine();
            _controller.Animator.PlayAttackAnimation(_animation);
        }

        public sealed override void Update()
        {
            _entranceTimer += Time.deltaTime;

            if (!_controller.Animator.IsSpecificAnimationPlaying(BossAnimation.ThrowStone)
                && _entranceTimer > _controller.Animator.GetCurrentAnimationLength())
            {
                _controller.StateMachine.TransitionTo(_controller.StateMachine.IdleState);
            }

            _Update();
        }

        public sealed override void Exit()
        {
            _entranceTimer = 0;
            _Exit();
            //_EndCoolTimeCoroutine();
        }

        private void _StartCoolTimeCoroutine()
        {
            _coolTimeCoroutine = _controller.StartCoroutine(_cStartCoolTimeCoroutine());
        }

        private void _EndCoolTimeCoroutine()
        {
            if (_coolTimeCoroutine != null)
            {
                _controller.StopCoroutine(_coolTimeCoroutine);
                _coolTimeCoroutine = null;
                _isCoolTimeEnded = false;
            }
        }

        private IEnumerator _cStartCoolTimeCoroutine()
        {
            _coolTimeWFS ??= new WaitForSeconds(CooldownTime);

            _isCoolTimeEnded = false;
            yield return _coolTimeWFS;
            _isCoolTimeEnded = true;

            // Debug.Log(this.GetType().Name + " is ready");
        }

        // 쿨타임 체크 로직 때문에 AttackState만 State 메서드들 따로 관리
        protected virtual void _Enter()
        {
        }

        protected virtual void _Update()
        {
        }

        protected virtual void _Exit()
        {
        }

        // 각 Attack State의 쿨타임을 반드시 설정해주기 위해 abstract 프로퍼티로
        protected abstract float CooldownTime { get; set; }

        public virtual bool IsReady { get; protected set; } = true;
    }
}