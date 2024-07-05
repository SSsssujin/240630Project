using System.Collections;
using System.Collections.Generic;
using INeverFall.Monster;
using UnityEngine;

namespace INeverFall.Monster
{
    public class ThrowStoneAttackState : AttackState
    {
        private float _entranceTimer = 0;
        
        public ThrowStoneAttackState(BossMonster controller) : base(controller)
        {
        }

        protected override void _Enter()
        {
            _controller.Animator.PlayAttackAnimation(BossAnimation.ThrowStone);
        }

        protected override void _Update()
        {
            _entranceTimer += Time.deltaTime;

            if (!_controller.Animator.IsSpecificAnimationPlaying(BossAnimation.ThrowStone)
                && _entranceTimer > _controller.Animator.GetCurrentAnimationLength())
            {
                _controller.StateMachine.TransitionTo(_controller.StateMachine.IdleState);
            }
        }

        protected override void _Exit()
        {
            _entranceTimer = 0;
        }

        protected override float CooldownTime { get; set; } = 30f;
    }
}