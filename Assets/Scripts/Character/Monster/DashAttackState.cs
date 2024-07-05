using System.Collections;
using System.Collections.Generic;
using INeverFall;
using RPGCharacterAnims.Actions;
using UnityEngine;

namespace INeverFall.Monster
{
    public class DashAttackState : AttackState
    {
        private const float _attackDuration = 4.033f;
        
        private float _entranceTimer = 0;
        
        public DashAttackState(BossMonster controller) : base(controller)
        {
            
        }
 
        protected override void _Enter()
        {
            _controller.Animator.PlayAttackAnimation(BossAnimation.Dash);
        }

        protected override void _Update()
        {
            _entranceTimer += Time.deltaTime;
            
            if (!_controller.Animator.IsSpecificAnimationPlaying(BossAnimation.Dash) 
                && _entranceTimer > _controller.Animator.GetCurrentAnimationLength())
            {
                _controller.StateMachine.TransitionTo(_controller.StateMachine.IdleState);
            }
        }

        protected override void _Exit()
        {
            _entranceTimer = 0;
        }

        protected override float CooldownTime { get; set; } = 25f;
    }
}