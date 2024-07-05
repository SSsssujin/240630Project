using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INeverFall.Monster
{
    public class FrontAttackState : AttackState
    {
        private const float _attackDuration = 3.367f;
        
        private float _entranceTimer = 0;
        private BossAnimation _frontAttackAnimation;

        public FrontAttackState(BossMonster controller) : base(controller)
        {
            
        }
        
        protected override void _Enter()
        {
            _frontAttackAnimation = _GetRandomFrontAttackAnimation();
            
            var refPosition = _controller.transform.position + Vector3.forward * 5;
            if (Physics.Raycast( refPosition, Vector3.down, out var hit, Mathf.Infinity))
            {
                _controller.GroundAttackHandPosition = hit.point;
            }
                
            _controller.Animator.PlayAttackAnimation(_frontAttackAnimation);
        }

        protected override void _Update()
        {
            _entranceTimer += Time.deltaTime;
            
            if (!_controller.Animator.IsSpecificAnimationPlaying(_frontAttackAnimation) 
                && _entranceTimer > _controller.Animator.GetCurrentAnimationLength())
            {
                _controller.StateMachine.TransitionTo(_controller.StateMachine.IdleState);
            }
        }

        protected override void _Exit()
        {
            _entranceTimer = 0;
            _frontAttackAnimation = BossAnimation.None;
        }

        // FrontAttack에 해당하는 애니메이션 중에서 랜덤으로 하나만 뽑기
        private BossAnimation _GetRandomFrontAttackAnimation()
        {
            BossAnimation[] frontAttackAnimations = 
            {
                BossAnimation.GroundAttack,
                BossAnimation.ArmSwingAttack
            };
            return frontAttackAnimations[Random.Range(0, frontAttackAnimations.Length)];
        }

        protected override float CooldownTime { get; set; } = 0f;

        public bool IsAvailable => CooldownTime == 0;
    }
}