using System;
using System.Collections;
using System.Collections.Generic;
using INeverFall.Monster;
using UnityEngine;

namespace INeverFall.Monster
{
    public class ThrowStoneAttackState : AttackState
    {
        private const float _attackDistance = 25f;
        
        public ThrowStoneAttackState(BossMonster controller) : base(controller)
        {
            _animation = BossAnimation.ThrowStone;
        }

        protected override void _Enter()
        {
        }

        protected override void _Update()
        {

        }

        protected override void _Exit()
        {
            
        }

        protected override float CooldownTime { get; set; } = 10f;

        public override bool IsReady => _isCoolTimeEnded &&
                                        _controller.IsTargetWithinAttackAngle &&
                                        !_controller.IsTargetWithinDistance(_attackDistance);
    }
}